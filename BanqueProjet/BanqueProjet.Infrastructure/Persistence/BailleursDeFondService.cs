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
    public class BailleursDeFondService: IBailleursDeFondService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IBailleursDeFondService> _logger;

        public BailleursDeFondService(
            BanquePDbContext dbContext,
            ILogger<IBailleursDeFondService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(BailleursDeFondsDto BailleursDeFond)
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
                entity = "OViewBailleursDeFond",
                action = "insert",
                data = BailleursDeFond
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task MettreAJourAsync(BailleursDeFondsDto BailleursDeFond)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewBailleursDeFond",
                action = "update",
                data = BailleursDeFond
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task SupprimerAsync(byte IdBailleursDeFonds)
        {
            var payload = new
            {
                entity = "OViewBailleursDeFond",
                action = "delete",
                data = new { IdBailleursDeFonds }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task<List<BailleursDeFondsDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<BailleursDeFondsDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_BAILLEURS_DE_FONDS")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BailleursDeFondsDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<BailleursDeFondsDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_BAILLEURS_DE_FONDS WHERE ID_BAILLEURS_DE_FONDS = {0}",
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
