using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Infrastructure.Data;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SuiviEvaluation.Infrastructure.Persistence
{
    public class LivrablesRealisesProjetService : ILivrablesRealisesProjetService
    {
        private readonly EvaluationDbContext _db;

        public LivrablesRealisesProjetService(EvaluationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Insère les livrables réalisés via SP JSON idempotente.
        /// </summary>
        public async Task AjouterAsync(LivrablesRealisesProjetDto dto)
        {
            var json = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            });

            const string sql = "BEGIN AJOUTER_LIVRABLES_REALISES_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Same as AjouterAsync (idempotent).
        /// </summary>
        public async Task MettreAJourAsync(LivrablesRealisesProjetDto dto)
        {
            await AjouterAsync(dto);
        }

        /// <summary>
        /// Supprime tous les livrables d’un projet.
        /// </summary>
        public async Task SupprimerProjetAsync(string IdIdentificationProjet)
        {
            const string sql = "BEGIN SUPPRIMER_LIVRABLES_REALISES_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Varchar2)
            {
                Value = IdIdentificationProjet
            };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Lit tous les livrables réalisés via la vue Oracle.
        /// Regroupe par projet et reconstruit le dictionnaire par année.
        /// </summary>
        public async Task<List<LivrablesRealisesProjetDto>> ObtenirTousAsync()
        {
            var result = new List<LivrablesRealisesProjetDto>();
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                   SELECT ID_IDENTIFICATION_PROJET
                         , QUANTITE_LIVREE
                      FROM O_VIEW_LIVRABLES_DU_PROJET";

                using var reader = await cmd.ExecuteReaderAsync();
                var map = new Dictionary<string, LivrablesRealisesProjetDto>();

                while (await reader.ReadAsync())
                {
                    var projetId = reader.GetString(0);
                    var exercice = reader.GetByte(1);
                    var qteLivree = reader.GetInt32(2);
                    var valeurCible = reader.GetInt32(3);

                    if (!map.TryGetValue(projetId, out var dto))
                    {
                        dto = new LivrablesRealisesProjetDto();
                        dto.InitializeValeurLivree(valeurCible);
                        map[projetId] = dto;
                    }

                    dto.QuantiteLivreeParAnnee[exercice] = qteLivree;
                }

                result = map.Values.ToList();
            }

            return result;
        }

        /// <summary>
        /// Lit un seul DTO à partir de son identifiant projet.
        /// </summary>
        public async Task<LivrablesRealisesProjetDto?> ObtenirParIdAsync(string id)
        {
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_IDENTIFICATION_PROJET
                         , QUANTITE_LIVREE
                      FROM O_VIEW_LIVRABLES_DU_PROJET
                     WHERE ID_IDENTIFICATION_PROJET = :p_id";

                var param = cmd.CreateParameter();
                param.ParameterName = "p_id";
                param.Value = id;
                cmd.Parameters.Add(param);

                using var reader = await cmd.ExecuteReaderAsync();
                LivrablesRealisesProjetDto? dto = null;

                while (await reader.ReadAsync())
                {
                    var exercice = reader.GetByte(1);
                    var qteLivree = reader.GetInt32(2);
                    var valeurCible = reader.GetInt32(3);

                    if (dto == null)
                    {
                        dto = new LivrablesRealisesProjetDto();
                        dto.InitializeValeurLivree(valeurCible);
                    }

                    dto.QuantiteLivreeParAnnee[exercice] = qteLivree;
                }

                return dto;
            }
        }
    }
}
