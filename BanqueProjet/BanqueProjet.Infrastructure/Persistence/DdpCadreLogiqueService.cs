using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using BanqueProjet.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class DdpCadreLogiqueService : IDdpCadreLogiqueService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<DdpCadreLogiqueService> _logger;

        public DdpCadreLogiqueService(
            BanquePDbContext dbContext,
            ILogger<DdpCadreLogiqueService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region CRUD high-level

        public async Task AjouterAsync(DdpCadreLogiqueDto cadre)
        {
            var wrapper = new
            {
                entity = "ddp_cadre_logique",
                action = "insert",
                data = cadre
            };

            await ExecuteJsonProcedureAsync("AJOUTER_DDP_CADRE_LOGIQUE_JSON", wrapper);
        }

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdAsync(string idIdentificationProjet)
        {
            var wrapper = new { ID_IDENTIFICATION_PROJET = idIdentificationProjet };

            var jsonOut = await ExecuteJsonProcedureAsync("OBTENIR_CADRE_LOGIQUE_JSON", wrapper);

            if (string.IsNullOrWhiteSpace(jsonOut))
                return null;

            return JsonConvert.DeserializeObject<DdpCadreLogiqueDto>(jsonOut);
        }

        public async Task<List<DdpCadreLogiqueDto>> ObtenirTousAsync()
        {
            var rows = await _dbContext
                .Set<OViewDdpCadreLogique>()
                .AsNoTracking()
                .ToListAsync();

            return rows
                .Select(r => new DdpCadreLogiqueDto
                {
                    IdDdpCadreLogique = r.IdDdpCadreLogique,
                    IntrantsResumeNarratif = r.IntrantsResumeNarratif,
                    ExtrantsResumeNarratif = r.ExtrantsResumeNarratif,
                    ObjectifsSpecifiquesResumeNarratif = r.ObjectifsSpecifiquesResumeNarratif,
                    ObjectifGeneralResumeNarratif = r.ObjectifGeneralResumeNarratif,
                    IntrantsIov = r.IntrantsIov,
                    ExtrantsIov = r.ExtrantsIov,
                    ObjectifsSpecifiquesIov = r.ObjectifsSpecifiquesIov,
                    ObjectifGeneralIov = r.ObjectifGeneralIov,
                    IntrantsSmov = r.IntrantsSmov,
                    ExtrantsSmov = r.ExtrantsSmov,
                    ObjectifsSpecifiquesSmov = r.ObjectifsSpecifiquesSmov,
                    ObjectifGeneralSmov = r.ObjectifGeneralSmov,
                    IntrantsRisquesHypotheses = r.IntrantsRisquesHypotheses,
                    ExtrantsRisquesHypotheses = r.ExtrantsRisquesHypotheses,
                    ObjectifsSpecifiquesRisquesHypotheses = r.ObjectifsSpecifiquesRisquesHypotheses,
                    ObjectifGeneralRisquesHypotheses = r.ObjectifGeneralRisquesHypotheses,
                    IdIdentificationProjet = r.IdIdentificationProjet
                })
                .ToList();
        }

        public async Task MettreAJourAsync(DdpCadreLogiqueDto cadre)
        {
            var wrapper = new
            {
                entity = "ddp_cadre_logique",
                action = "update",
                data = cadre
            };

            await ExecuteJsonProcedureAsync("AJOUTER_DDP_CADRE_LOGIQUE_JSON", wrapper);
        }

        public async Task SupprimerAsync(string idIdentificationProjet, byte idDdpCadreLogique)
        {
            var wrapper = new
            {
                entity = "ddp_cadre_logique",
                action = "delete",
                data = new
                {
                    ID_IDENTIFICATION_PROJET = idIdentificationProjet,
                    ID_DDP_CADRE_LOGIQUE = idDdpCadreLogique
                }
            };

            await ExecuteJsonProcedureAsync("SUPPRIMER_DDP_CADRE_LOGIQUE_JSON", wrapper);
        }

        #endregion

        #region ExecuteJsonProcedureAsync & utilitaires

        public async Task<string?> ExecuteJsonProcedureAsync(string procedureName, object wrapper)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("procedureName required", nameof(procedureName));

            var jsonPayload = JsonConvert.SerializeObject(wrapper, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            });

            string? connStringFromDb = null;
            try
            {
                connStringFromDb = _dbContext.Database.GetDbConnection()?.ConnectionString;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Impossible de lire la connection string depuis DbContext.");
            }

            OracleConnectionStringBuilder csBuilder;

            if (!string.IsNullOrWhiteSpace(connStringFromDb))
            {
                csBuilder = new OracleConnectionStringBuilder(connStringFromDb);

                if (string.IsNullOrWhiteSpace(csBuilder.Password))
                {
                    _logger.LogWarning("La chaîne du DbContext ne contient pas de mot de passe. Tentative de récupération depuis les variables d'environnement (.env).");
                    if (TryBuildConnStringFromEnv(out var envConnString))
                    {
                        csBuilder = new OracleConnectionStringBuilder(envConnString);
                        _logger.LogInformation("Chaîne de connexion construite depuis les variables d'environnement.");
                    }
                    else
                    {
                        _logger.LogError("Aucune chaîne avec mot de passe trouvée. ConnString DbContext (masquée): {cs}",
                            MaskPassword(connStringFromDb));
                        throw new InvalidOperationException("Mot de passe manquant dans la chaîne de connexion (DbContext) et aucune variable d'environnement fournie.");
                    }
                }
            }
            else
            {
                if (!TryBuildConnStringFromEnv(out var envConnStringFallback))
                {
                    _logger.LogError("Aucune chaîne de connexion trouvée (DbContext vide et variables d'environnement manquantes).");
                    throw new InvalidOperationException("Chaine de connexion introuvable.");
                }
                csBuilder = new OracleConnectionStringBuilder(envConnStringFallback);
                _logger.LogInformation("Chaîne de connexion construite depuis les variables d'environnement (DbContext vide).");
            }

            var finalConnString = csBuilder.ToString();
            _logger.LogDebug("ExecuteJsonProcedureAsync: proc={proc}, jsonLen={len}, conn={cs}", procedureName, jsonPayload?.Length ?? 0, MaskPassword(finalConnString));

            try
            {
                await using var conn = new OracleConnection(finalConnString);
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add(new OracleParameter("p_json", OracleDbType.Clob) { Direction = ParameterDirection.Input, Value = (object)jsonPayload ?? DBNull.Value });
                cmd.Parameters.Add(new OracleParameter("p_out_json", OracleDbType.Clob) { Direction = ParameterDirection.Output });

                await cmd.ExecuteNonQueryAsync();

                var outVal = cmd.Parameters["p_out_json"].Value;
                return (outVal == DBNull.Value || outVal == null) ? null : outVal.ToString();
            }
            catch (OracleException oex) when (oex.Number == 1005)
            {
                _logger.LogError(oex, "ORA-01005 : Login denied (connString={cs})", MaskPassword(finalConnString));
                OracleConnection.ClearAllPools();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur ExecuteJsonProcedureAsync pour {proc}, json length {len}", procedureName, jsonPayload?.Length ?? 0);
                throw;
            }
        }

        private static bool TryBuildConnStringFromEnv(out string connString)
        {
            connString = string.Empty;

            // 1) Connexion complète
            var envCs = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            if (!string.IsNullOrWhiteSpace(envCs))
            {
                connString = envCs;
                return true;
            }

            var envCsOracle = Environment.GetEnvironmentVariable("ORACLE_DB_CONNECTIONSTRING");
            if (!string.IsNullOrWhiteSpace(envCsOracle))
            {
                connString = envCsOracle;
                return true;
            }

            // 2) Triplet simple
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var ds = Environment.GetEnvironmentVariable("DB_DATASOURCE");

            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(ds))
            {
                var b = new OracleConnectionStringBuilder
                {
                    UserID = user,
                    Password = pass,
                    DataSource = ds
                };
                connString = b.ToString();
                return true;
            }

            // 3) Variables ORACLE_DB_*
            var oracleUser = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
            var oraclePass = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
            var oracleHost = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
            var oraclePort = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
            var oracleService = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

            if (!string.IsNullOrWhiteSpace(oracleUser) &&
                !string.IsNullOrWhiteSpace(oraclePass) &&
                !string.IsNullOrWhiteSpace(oracleHost) &&
                !string.IsNullOrWhiteSpace(oraclePort) &&
                !string.IsNullOrWhiteSpace(oracleService))
            {
                var dataSource = $"{oracleHost}:{oraclePort}/{oracleService}";
                var b = new OracleConnectionStringBuilder
                {
                    UserID = oracleUser,
                    Password = oraclePass,
                    DataSource = dataSource
                };
                connString = b.ToString();
                return true;
            }

            return false;
        }

        private static string MaskPassword(string cs)
        {
            if (string.IsNullOrWhiteSpace(cs)) return cs ?? string.Empty;
            try
            {
                var b = new OracleConnectionStringBuilder(cs);
                if (!string.IsNullOrEmpty(b.Password)) b.Password = "*****";
                return b.ToString();
            }
            catch
            {
                return System.Text.RegularExpressions.Regex.Replace(cs, "(Password=)([^;]+)", "$1*****", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
        }

        #endregion

        #region Méthodes interface

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdAsync(byte id)
            => await ObtenirParIdAsync(id.ToString());

        public async Task SupprimerAsync(byte id)
        {
            var wrapper = new
            {
                entity = "ddp_cadre_logique",
                action = "delete",
                data = new { ID_DDP_CADRE_LOGIQUE = id }
            };

            await ExecuteJsonProcedureAsync("SUPPRIMER_DDP_CADRE_LOGIQUE_JSON", wrapper);
        }

        public Task<byte> GetNextIdAsync(byte parentId)
        {
            _logger.LogWarning("GetNextIdAsync(byte) appelé mais non implémenté réellement. parentId={id}", parentId);
            return Task.FromResult((byte)0);
        }

        Task IDdpCadreLogiqueService.GetNextIdAsync(byte IdDdpCadreLogique)
        {
            throw new NotImplementedException();
        }

        public Task<byte> GetNextIdAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
