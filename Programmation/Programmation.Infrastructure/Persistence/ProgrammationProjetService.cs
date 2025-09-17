using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using Programmation.Infrastructure.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Programmation.Infrastructure.Persistence
{
    public class ProgrammationProjetService : IProgrammationProjetService
    {
        private readonly ProgrammationDbContext _db;

        public ProgrammationProjetService(ProgrammationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Appelle la procédure AJOUTER_PROGRAMMATION_POUR_PROJET_JSON
        /// pour insérer les données de programmation liées à un projet existant.
        /// </summary>
        public async Task AjouterAsync(ProgrammationProjetDto programmationProjetDto)
        {
            var json = JsonConvert.SerializeObject(programmationProjetDto,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                });

            var sql = "BEGIN AJOUTER_PROGRAMMATION_POUR_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Même logique qu’AjouterAsync : la procédure est idempotente (supprime/maj puis réinsère).
        /// </summary>
        public async Task MettreAJourAsync(ProgrammationProjetDto programmationProjetDto)
        {
            await AjouterAsync(programmationProjetDto);
        }

        /// <summary>
        /// Supprimer une activité (idempotente).
        /// Suppose que tu as une proc SUPPRIMER_ACTIVITE(id).
        /// </summary>
        public async Task SupprimerAsync(byte IdActivites)
        {
            var sql = "BEGIN SUPPRIMER_ACTIVITE(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Byte) { Value = IdActivites };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        /// <summary>
        /// Obtenir toutes les programmations via la vue Oracle O_VIEW_PROGRAMMATION_PROJET.
        /// </summary>
        public async Task<List<ProgrammationProjetDto>> ObtenirTousAsync()
        {
            var result = new List<ProgrammationProjetDto>();

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_IDENTIFICATION_PROJET
                           /* , autres colonnes de la vue si disponibles */
                    FROM VIEW_IDENTIFICATION_PROJET_PLAT";

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new ProgrammationProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0)
                        // compléter ici si ta vue expose d’autres colonnes
                    };
                    result.Add(dto);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtenir une programmation par IdIdentificationProjet via la vue Oracle.
        /// </summary>
        public async Task<ProgrammationProjetDto?> ObtenirParIdAsync(string id)
        {
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ID_IDENTIFICATION_PROJET
                           /* , autres colonnes de la vue si disponibles */
                    FROM VIEW_IDENTIFICATION_PROJET_PLAT
                    WHERE ID_IDENTIFICATION_PROJET = :p_id";

                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ProgrammationProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0)
                        // compléter ici avec d’autres colonnes si besoin
                    };
                }
            }

            return null;
        }
    }
}
