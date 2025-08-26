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
    public class ObjectifsSpecifiquesService: IObjectifsSpecifiquesService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IObjectifsSpecifiquesService> _logger;

        public ObjectifsSpecifiquesService(
            BanquePDbContext dbContext,
            ILogger<IObjectifsSpecifiquesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(ObjectifsSpecifiquesDto objectifsSpecifiques)
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
                entity = "ViewIdentProjetObjectifsSpecifiquesPlat",
                action = "insert",
                data = objectifsSpecifiques
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task MettreAJourAsync(ObjectifsSpecifiquesDto objectifsSpecifiques)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetObjectifsSpecifiquesPlat",
                action = "update",
                data = objectifsSpecifiques
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task SupprimerAsync(byte IdObjectifsSpecifiques)
        {
            var payload = new
            {
                entity = "ViewIdentProjetObjectifsSpecifiquesPlat",
                action = "delete",
                data = new { IdObjectifsSpecifiques }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task<List<ObjectifsSpecifiquesDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<ObjectifsSpecifiquesDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_OBJECTIFS_SPECIFIQUES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ObjectifsSpecifiquesDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<ObjectifsSpecifiquesDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_OBJECTIFS_SPECIFIQUES WHERE ID_Objectifs_Specifiques = {0}",
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
    }
}
