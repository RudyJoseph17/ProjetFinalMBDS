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
    public class LocalisationGeographiqueProjService: ILocalisationGeographiqueProjService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<LocalisationGeographiqueProjService> _logger;

        public LocalisationGeographiqueProjService(
            BanquePDbContext dbContext,
            ILogger<LocalisationGeographiqueProjService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(LocalisationGeographiqueProjDto localisationGeographique)
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
                entity = "localisation_geographique_projet",
                action = "insert",
                data = localisationGeographique
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_LOCALISATION_GEO_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_LOCALISATION_GEO_JSON", json);
        }

        public async Task MettreAJourAsync(LocalisationGeographiqueProjDto localisationGeographique)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "localisation_geographique_projet",
                action = "update",
                data = localisationGeographique
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_LOCALISATION_GEO_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_LOCALISATION_GEO_JSON", json);
        }

        public async Task SupprimerAsync(byte IdLocalisationGeographique)
        {
            var payload = new
            {
                entity = "localisation_geographique_projet",
                action = "delete",
                data = new { IdLocalisationGeographique }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_LOCALISATION_GEO_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_LOCALISATION_GEO_JSON", json);
        }

        public async Task<List<LocalisationGeographiqueProjDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<LocalisationGeographiqueProjDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_LOCALISATION_GEOGRAPHIQUE_PROJ")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LocalisationGeographiqueProjDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<LocalisationGeographiqueProjDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_LOCALISATION_GEOGRAPHIQUE_PROJ WHERE id_localisation_geographique_projet = {0}",
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
