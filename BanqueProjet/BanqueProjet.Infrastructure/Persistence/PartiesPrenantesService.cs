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
    public class PartiesPrenantesService: IPartiesPrenantesService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IPartiesPrenantesService> _logger;

        public PartiesPrenantesService(
            BanquePDbContext dbContext,
            ILogger<IPartiesPrenantesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(PartiesPrenantesDto partiesPrenantes)
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
                entity = "OViewPartiesPrenante",
                action = "insert",
                data = partiesPrenantes
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task MettreAJourAsync(PartiesPrenantesDto partiesPrenantes)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewPartiesPrenante",
                action = "update",
                data = partiesPrenantes
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task SupprimerAsync(byte IdPartiesPrenantes)
        {
            var payload = new
            {
                entity = "OViewPartiesPrenante",
                action = "delete",
                data = new { IdPartiesPrenantes }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Parties_Prenantes_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Parties_Prenantes_JSON", json);
        }

        public async Task<List<PartiesPrenantesDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<PartiesPrenantesDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_PARTIES_PRENANTES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PartiesPrenantesDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<PartiesPrenantesDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_PARTIES_PRENANTES WHERE ID_PARTIES_PRENANTES = {0}",
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
