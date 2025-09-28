using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client; // Pour OracleDbType

namespace BanqueProjet.Infrastructure.Persistence
{
    public class AspectsJuridiquesService : IAspectsJuridiquesService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<AspectsJuridiquesService> _logger;

        public AspectsJuridiquesService(
            BanquePDbContext dbContext,
            ILogger<AspectsJuridiquesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        private static JsonSerializerSettings SerializerSettings => new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy() // garde la casse C#
            },
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public async Task AjouterAsync(AspectsJuridiquesDto aspectsJuridiques)
        {
            var payload = new
            {
                entity = "OViewAspectsJuridique",
                action = "insert",
                data = aspectsJuridiques
            };

            var json = JsonConvert.SerializeObject(payload, SerializerSettings);
            _logger.LogInformation("🟢 Insertion JSON -> PROCESS_ASPECTS_JURIDIQUES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_JURIDIQUES_JSON", json);
        }

        public async Task MettreAJourAsync(AspectsJuridiquesDto aspectsJuridiques)
        {
            var payload = new
            {
                entity = "OViewAspectsJuridique",
                action = "update",
                data = aspectsJuridiques
            };

            var json = JsonConvert.SerializeObject(payload, SerializerSettings);
            _logger.LogInformation("🔄 Mise à jour JSON -> PROCESS_ASPECTS_JURIDIQUES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_JURIDIQUES_JSON", json);
        }

        public async Task SupprimerAsync(byte idAspectsJuridiques)
        {
            var payload = new
            {
                entity = "OViewAspectsJuridique",
                action = "delete",
                data = new { IdAspectsJuridiques = idAspectsJuridiques }
            };

            var json = JsonConvert.SerializeObject(payload, SerializerSettings);
            _logger.LogInformation("🗑️ Suppression JSON -> PROCESS_ASPECTS_JURIDIQUES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_JURIDIQUES_JSON", json);
        }

        public async Task<IEnumerable<AspectsJuridiquesDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.OViewAspectsJuridiques
                .AsNoTracking()
                .ToListAsync();

            // Mappez-les vers vos DTOs métier
            return entities.Select(e => new AspectsJuridiquesDto
            {
                IdAspectsJuridiques = (byte)e.IdAspectsJuridiques,
                DescAspectsJuridiques = e.DescriptionAspect,
                CategorieAspect = e.CategorieAspect,
                IdIdentificationProjet = e.IdIdentificationProjet
            })
            .ToList();
        }

        public async Task<AspectsJuridiquesDto?> ObtenirParIdAsync(byte id)
        {
            var e = await _dbContext.OViewAspectsJuridiques
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdAspectsJuridiques == id);

            if (e == null) return null;

            return new AspectsJuridiquesDto
            {
                IdAspectsJuridiques = (byte)e.IdAspectsJuridiques,
                DescAspectsJuridiques = e.DescriptionAspect,
                CategorieAspect = e.CategorieAspect,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        private async Task ExecuteProcedureAsync(string procedureName, string json)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();

            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.CreateParameter();
            param.ParameterName = "p_json";
            param.Value = json;

            // 🔹 Assure compatibilité Oracle
            if (param is OracleParameter oracleParam)
            {
                oracleParam.OracleDbType = OracleDbType.Clob;
            }
            else
            {
                param.DbType = DbType.String;
            }

            cmd.Parameters.Add(param);

            try
            {
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur lors de l’exécution de {Procedure} avec JSON : {Json}", procedureName, json);
                throw;
            }
        }

        public Task SupprimerAsync(int idAspectsJuridiques)
        {
            throw new NotImplementedException();
        }
    }
}
