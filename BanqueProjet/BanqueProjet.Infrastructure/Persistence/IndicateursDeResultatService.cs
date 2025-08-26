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
    public class IndicateursDeResultatService: IIndicateursDeResultatService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IIndicateursDeResultatService> _logger;

        public IndicateursDeResultatService(
            BanquePDbContext dbContext,
            ILogger<IIndicateursDeResultatService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(IndicateursDeResultatDto indicateursDeResultats)
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
                entity = "OViewIndicateursDeResultat",
                action = "insert",
                data = indicateursDeResultats
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Indicateurs_Resultats_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Indicateurs_Resultats_JSON", json);
        }

        public async Task MettreAJourAsync(IndicateursDeResultatDto indicateursDeResultats)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewIndicateursDeResultat",
                action = "update",
                data = indicateursDeResultats
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Indicateurs_Resultats_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Indicateurs_Resultats_JSON", json);
        }

        public async Task SupprimerAsync(byte IdIndicateursDeResultats)
        {
            var payload = new
            {
                entity = "OViewIndicateursDeResultat",
                action = "delete",
                data = new { IdIndicateursDeResultats }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Indicateurs_Resultats_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Indicateurs_Resultats_JSON", json);
        }

        public async Task<List<IndicateursDeResultatDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<IndicateursDeResultatDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_INDICATEURS_DE_RESULTATS")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IndicateursDeResultatDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<IndicateursDeResultatDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_INDICATEURS_DE_RESULTATS WHERE ID_INDICATEURS_DE_RESULTATS = {0}",
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
