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
    public class InformationsFinancieresProgrammeesProjetService : IInformationsFinancieresProgrammeesProjetService
    {
        private readonly ProgrammationDbContext _db;

        public InformationsFinancieresProgrammeesProjetService(ProgrammationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Appelle la procédure AJOUTER_INFOS_FINANCIERES_PROGRAMMEES_PROJET_JSON
        /// pour insérer les informations financières programmées.
        /// </summary>
        public async Task AjouterAsync(InformationsFinancieresProgrammeesProjetDto info)
        {
            var json = JsonConvert.SerializeObject(info,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                });

            var sql = "BEGIN AJOUTER_INFOS_FINANCIERES_PROGRAMMEES_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Idempotent : même logique qu’AjouterAsync.
        /// </summary>
        public async Task MettreAJourAsync(InformationsFinancieresProgrammeesProjetDto info)
        {
            await AjouterAsync(info);
        }

        /// <summary>
        /// Supprime une info. financière programmée (idempotent).
        /// </summary>
        public async Task SupprimerAsync(byte IdInformationsFinancieresProjet)
        {
            var sql = "BEGIN SUPPRIMER_INFOS_FINANCIERES_PROGRAMMEES_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Byte) { Value = IdInformationsFinancieresProjet };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Lit toutes les infos financières programmées via la vue correspondante.
        /// </summary>
        public async Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirTousAsync()
        {
            var result = new List<InformationsFinancieresProgrammeesProjetDto>();

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_INFORMATION_FINANCIERE
                         /* , autres colonnes de la vue si disponibles */
                    FROM O_VIEW_INFOS_FINANCIERES_PROGRAMMEES_PROJET";

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new InformationsFinancieresProgrammeesProjetDto
                    {
                        IdInformationsFinancieres = reader.GetByte(0)
                        // compléter selon la vue
                    };
                    result.Add(dto);
                }
            }

            return result;
        }

        /// <summary>
        /// Lit une info. financière programmée par identifiant.
        /// </summary>
        public async Task<InformationsFinancieresProgrammeesProjetDto?> ObtenirParIdAsync(byte id)
        {
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_INFORMATION_FINANCIERE
                         /* , autres colonnes de la vue si disponibles */
                    FROM O_VIEW_INFOS_FINANCIERES_PROGRAMMEES_PROJET
                    WHERE ID_INFORMATION_FINANCIERE = :p_id";

                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new InformationsFinancieresProgrammeesProjetDto
                    {
                        IdInformationsFinancieres = reader.GetByte(0)
                        // compléter selon le schéma
                    };
                }
            }

            return null;
        }
    }
}
