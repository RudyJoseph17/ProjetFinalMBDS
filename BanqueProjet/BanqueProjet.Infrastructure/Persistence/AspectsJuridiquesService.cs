using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class AspectsJuridiquesService : IAspectsJuridiquesService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IAspectsJuridiquesService> _logger;

        public AspectsJuridiquesService(
            BanquePDbContext dbContext,
            ILogger<IAspectsJuridiquesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(AspectsJuridiquesDto aspectsJuridiques)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy() // respecte la casse C#
                },
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var payload = new
            {
                entity = "aspects_juridiques",
                action = "insert",
                data = aspectsJuridiques
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_ASPECTS_Juridiques_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_Juridiques_JSON", json);
        }

        public async Task MettreAJourAsync(AspectsJuridiquesDto aspectsJuridiques)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "aspects_juridiques",
                action = "update",
                data = aspectsJuridiques
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_ASPECTS_Juridiques_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_Juridiques_JSON", json);
        }

        public async Task SupprimerAsync(byte IdAspectsJuridiques)
        {
            var payload = new
            {
                entity = "aspects_juridiques",
                action = "delete",
                data = new { IdAspectsJuridiques }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_ASPECTS_Juridiques_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ASPECTS_Juridiques_JSON", json);
        }

        public async Task<List<AspectsJuridiquesDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<AspectsJuridiquesDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_ASPECTS_JURIDIQUES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AspectsJuridiquesDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<AspectsJuridiquesDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_ASPECTS_JURIDIQUES WHERE id_aspects_juridiques = {0}",
                    id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        private async Task ExecuteProcedureAsync(string procedureName, string json)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();

            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.CreateParameter();
            param.ParameterName = "p_json";
            param.DbType = DbType.String;
            param.Value = json;
            cmd.Parameters.Add(param);

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        //public Task<List<AspectsJuridiquesDto>> ObtenirParIdAsync(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
