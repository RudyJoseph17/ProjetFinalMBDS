using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using Programmation.Infrastructure.Data;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Programmation.Infrastructure.Persistence
{
    public class LivrablesProjetService : ILivrablesProjetService
    {
        private readonly ProgrammationDbContext _db;

        public LivrablesProjetService(ProgrammationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Appelle la procédure AJOUTER_LIVRABLES_PROGRAMME_PROJET_JSON
        /// pour insérer les livrables d’un projet.
        /// </summary>
        public async Task AjouterAsync(LivrablesProgrameProjetDto livrable)
        {
            var json = JsonConvert.SerializeObject(livrable,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                });

            var sql = "BEGIN AJOUTER_LIVRABLES_PROGRAMME_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Idempotent : supprime/maj puis réinsère.
        /// </summary>
        public async Task MettreAJourAsync(LivrablesProgrameProjetDto livrable)
        {
            await AjouterAsync(livrable);
        }

        /// <summary>
        /// Supprime un livrable (idempotent).
        /// </summary>
        public async Task SupprimerAsync(byte IdActivites)
        {
            var sql = "BEGIN SUPPRIMER_LIVRABLES_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Byte) { Value = IdActivites };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Récupère tous les livrables via la vue O_VIEW_LIVRABLES_PROJET.
        /// </summary>
        public async Task<List<LivrablesProgrameProjetDto>> ObtenirTousAsync()
        {
            var result = new List<LivrablesProgrameProjetDto>();

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_ACTIVITE
                         /* , autres colonnes de la vue si disponibles */
                    FROM O_VIEW_LIVRABLES_PROJET";

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new LivrablesProgrameProjetDto
                    {
                        IdLivrablesProjet = reader.GetByte(0)
                        // compléter selon la vue
                    };
                    result.Add(dto);
                }
            }

            return result;
        }

        /// <summary>
        /// Récupère un livrable par identifiant.
        /// </summary>
        public async Task<LivrablesProgrameProjetDto?> ObtenirParIdAsync(byte id)
        {
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_ACTIVITE
                         /* , autres colonnes de la vue si disponibles */
                    FROM O_VIEW_LIVRABLES_PROJET
                    WHERE ID_ACTIVITE = :p_id";

                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LivrablesProgrameProjetDto
                    {
                        IdLivrablesProjet = reader.GetByte(0)
                        // compléter selon le schéma
                    };
                }
            }

            return null;
        }
    }
}
