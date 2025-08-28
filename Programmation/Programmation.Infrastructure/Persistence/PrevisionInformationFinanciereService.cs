using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Programmation.Infrastructure.Data;
using Programmation.Application.Interface;
using Programmation.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Persistence
{
    // IMPORTANT : implémentation de l'interface
    public class PrevisionInformationFinanciereService : IPrevisionInformationFinanciereService
    {
        private readonly ProgrammationDbContext _dbContext;
        private readonly ILogger<PrevisionInformationFinanciereService> _logger;

        public PrevisionInformationFinanciereService(
            ProgrammationDbContext dbContext,
            ILogger<PrevisionInformationFinanciereService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(PrevisionInformationFinanciereDto previsionInformationFinanciere)
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
                entity = "ViewActivitesIformationsFinanciere",
                action = "insert",
                data = previsionInformationFinanciere
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(PrevisionInformationFinanciereDto previsionInformationFinanciere)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewActivitesIformationsFinanciere",
                action = "update",
                data = previsionInformationFinanciere
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdInformationsFinancieres)
        {
            var payload = new
            {
                entity = "ViewActivitesIformationsFinanciere",
                action = "delete",
                data = new { IdInformationsFinancieres }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task<List<PrevisionInformationFinanciereDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<PrevisionInformationFinanciereDto>()
                .FromSqlRaw("SELECT * FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PrevisionInformationFinanciereDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<PrevisionInformationFinanciereDto>()
                .FromSqlRaw(
                    "SELECT * FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES WHERE ID_INFORMATIONS_FINANCIERES = {0}",
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
