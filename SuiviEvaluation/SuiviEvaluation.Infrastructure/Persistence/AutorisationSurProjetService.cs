using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SuiviEvaluation.Infrastructure.Persistence
{
    public class AutorisationSurProjetService : IAutorisationSurProjetService
    {
        private readonly EvaluationDbContext _db;
        public AutorisationSurProjetService(EvaluationDbContext db) => _db = db;

        // Ajout/MAJ : appelle la proc et lit OUT params
        public async Task AjouterAsync(AutorisationSurProjetDto dto)
        {
            var json = JsonConvert.SerializeObject(dto,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AJOUTER_AUTORISATION_SUR_PROJET_JSON";

                var pJson = new OracleParameter("p_json", OracleDbType.Clob)
                {
                    Direction = ParameterDirection.Input,
                    Value = json
                };
                var pCode = new OracleParameter("p_result_code", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                var pMsg = new OracleParameter("p_result_msg", OracleDbType.Varchar2, 4000)
                {
                    Direction = ParameterDirection.Output
                };
                var pId = new OracleParameter("p_id_out", OracleDbType.Varchar2, 100)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(pJson);
                cmd.Parameters.Add(pCode);
                cmd.Parameters.Add(pMsg);
                cmd.Parameters.Add(pId);

                await cmd.ExecuteNonQueryAsync();

                // lire les OUT
                var code = pCode.Value == DBNull.Value ? -1 : Convert.ToInt32(pCode.Value);
                var message = pMsg.Value == DBNull.Value ? string.Empty : pMsg.Value.ToString();
                var idOut = pId.Value == DBNull.Value ? null : pId.Value.ToString();

                if (code != 0)
                {
                    // log erreur + rethrow controllée
                    throw new InvalidOperationException($"Proc AJOUTER_AUTORISATION_SUR_PROJET_JSON failed: {code} - {message}");
                }

                // si tu veux retourner ou utiliser l'id généré, tu peux le logger ou le stocker
                // ex: _logger.LogInformation("Created info financiere id {Id}", idOut);
            }
            catch (OracleException orex)
            {
                // optional: map Oracle error numbers to friendly messages
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public Task MettreAJourAsync(AutorisationSurProjetDto dto)
            => AjouterAsync(dto);

        public async Task SupprimerAsync(int IdActivites)
        {
            const string sql = "BEGIN SUPPRIMER_AUTORISATION_PAR_ACTIVITE(:p_id); END;";
            var p = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdActivites };
            await _db.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task SupprimerProjetAsync(string IdIdentificationProjet)
        {
            const string sql = "BEGIN SUPPRIMER_AUTORISATIONS_SUR_PROJET(:p_id); END;";
            var p = new OracleParameter("p_id", OracleDbType.Varchar2) { Value = IdIdentificationProjet };
            await _db.Database.ExecuteSqlRawAsync(sql, p);
        }

        // ---------- READ methods: robustes et safe ----------
        public async Task<List<AutorisationSurProjetDto>> ObtenirTousAsync()
        {
            var list = new List<AutorisationSurProjetDto>();
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT  ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         MONTANT_AUTORISATION
                    FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES";

                using var reader = await cmd.ExecuteReaderAsync();

                // obtenir ordinals de façon sûre
                int ordIdProj = -1, ordIdAct = -1, ordDeb = -1, ordFin = -1,
                    ordArticle = -1, ordAlinea = -1, ordMois = -1, ordMontant = -1;

                try { ordIdAct = reader.GetOrdinal("ID_ACTIVITES"); } catch { }
                try { ordDeb = reader.GetOrdinal("EXERCICE_FISCAL_DEBUT"); } catch { }
                try { ordFin = reader.GetOrdinal("EXERCICE_FISCAL_FIN"); } catch { }
                //try { ordArticle = reader.GetOrdinal("ARTICLE"); } catch { }
                //try { ordAlinea = reader.GetOrdinal("ALINEA"); } catch { }
                //try { ordMois = reader.GetOrdinal("MOIS_AUTORISATION"); } catch { }
                try { ordMontant = reader.GetOrdinal("MONTANT_AUTORISATION"); } catch { }

                while (await reader.ReadAsync())
                {
                    var dto = new AutorisationSurProjetDto();

                    if (ordIdAct >= 0 && !reader.IsDBNull(ordIdAct))
                        dto.IdActivites = reader.GetInt32(ordIdAct);

                    if (ordDeb >= 0 && !reader.IsDBNull(ordDeb))
                        dto.ExerciceFiscalDebut = reader.GetByte(ordDeb);

                    if (ordFin >= 0 && !reader.IsDBNull(ordFin))
                        dto.ExerciceFiscalFin = reader.GetByte(ordFin);

                    //if (ordArticle >= 0 && !reader.IsDBNull(ordArticle))
                    //    dto.Article = reader.GetString(ordArticle);

                    //if (ordAlinea >= 0 && !reader.IsDBNull(ordAlinea))
                    //    dto.Alinea = reader.GetString(ordAlinea);

                    //if (ordMois >= 0 && !reader.IsDBNull(ordMois))
                    //    dto.MoisAutorisation = reader.GetString(ordMois);

                    if (ordMontant >= 0 && !reader.IsDBNull(ordMontant))
                        dto.MontantAutorisation = reader.GetDecimal(ordMontant);

                    list.Add(dto);
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return list;
        }

        public async Task<AutorisationSurProjetDto?> ObtenirParIdAsync(string id)
        {
            AutorisationSurProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         MONTANT_AUTORISATION
                    FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES
                   WHERE ID_ACTIVITES = :p_id";
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    // ordinals
                    int ordIdProj = -1, ordIdAct = -1, ordDeb = -1, ordFin = -1,
                        ordArticle = -1, ordAlinea = -1, ordMois = -1, ordMontant = -1;


                    try { ordIdAct = reader.GetOrdinal("ID_ACTIVITES"); } catch { }
                    try { ordDeb = reader.GetOrdinal("EXERCICE_FISCAL_DEBUT"); } catch { }
                    try { ordFin = reader.GetOrdinal("EXERCICE_FISCAL_FIN"); } catch { }
                    //try { ordArticle = reader.GetOrdinal("ARTICLE"); } catch { }
                    //try { ordAlinea = reader.GetOrdinal("ALINEA"); } catch { }
                    //try { ordMois = reader.GetOrdinal("MOIS_AUTORISATION"); } catch { }
                    try { ordMontant = reader.GetOrdinal("MONTANT_AUTORISATION"); } catch { }

                    dto = new AutorisationSurProjetDto();


                    if (ordIdAct >= 0 && !reader.IsDBNull(ordIdAct))
                        dto.IdActivites = reader.GetInt32(ordIdAct);

                    if (ordDeb >= 0 && !reader.IsDBNull(ordDeb))
                        dto.ExerciceFiscalDebut = reader.GetByte(ordDeb);

                    if (ordFin >= 0 && !reader.IsDBNull(ordFin))
                        dto.ExerciceFiscalFin = reader.GetByte(ordFin);

                    //if (ordArticle >= 0 && !reader.IsDBNull(ordArticle))
                    //    dto.Article = reader.GetString(ordArticle);

                    //if (ordAlinea >= 0 && !reader.IsDBNull(ordAlinea))
                    //    dto.Alinea = reader.GetString(ordAlinea);

                    //if (ordMois >= 0 && !reader.IsDBNull(ordMois))
                    //    dto.MoisAutorisation = reader.GetString(ordMois);

                    if (ordMontant >= 0 && !reader.IsDBNull(ordMontant))
                        dto.MontantAutorisation = reader.GetDecimal(ordMontant);
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return dto;
        }

        public async Task<AutorisationSurProjetDto?> ObtenirParIdActiviteAsync(int id)
        {
            AutorisationSurProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         MONTANT_AUTORISATION
                    FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES
                   WHERE ID_ACTIVITES = :p_id";
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    int ordIdProj = -1, ordIdAct = -1, ordDeb = -1, ordFin = -1,
                        ordArticle = -1, ordAlinea = -1, ordMois = -1, ordMontant = -1;


                    try { ordIdAct = reader.GetOrdinal("ID_ACTIVITES"); } catch { }
                    try { ordDeb = reader.GetOrdinal("EXERCICE_FISCAL_DEBUT"); } catch { }
                    try { ordFin = reader.GetOrdinal("EXERCICE_FISCAL_FIN"); } catch { }
                    //try { ordArticle = reader.GetOrdinal("ARTICLE"); } catch { }
                    //try { ordAlinea = reader.GetOrdinal("ALINEA"); } catch { }
                    //try { ordMois = reader.GetOrdinal("MOIS_AUTORISATION"); } catch { }
                    try { ordMontant = reader.GetOrdinal("MONTANT_AUTORISATION"); } catch { }

                    dto = new AutorisationSurProjetDto();


                    if (ordIdAct >= 0 && !reader.IsDBNull(ordIdAct))
                        dto.IdActivites = reader.GetInt32(ordIdAct);

                    if (ordDeb >= 0 && !reader.IsDBNull(ordDeb))
                        dto.ExerciceFiscalDebut = reader.GetByte(ordDeb);

                    if (ordFin >= 0 && !reader.IsDBNull(ordFin))
                        dto.ExerciceFiscalFin = reader.GetByte(ordFin);

                    //if (ordArticle >= 0 && !reader.IsDBNull(ordArticle))
                    //    dto.Article = reader.GetString(ordArticle);

                    //if (ordAlinea >= 0 && !reader.IsDBNull(ordAlinea))
                    //    dto.Alinea = reader.GetString(ordAlinea);

                    //if (ordMois >= 0 && !reader.IsDBNull(ordMois))
                    //    dto.MoisAutorisation = reader.GetString(ordMois);

                    if (ordMontant >= 0 && !reader.IsDBNull(ordMontant))
                        dto.MontantAutorisation = reader.GetDecimal(ordMontant);
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return dto;
        }
    }
}
