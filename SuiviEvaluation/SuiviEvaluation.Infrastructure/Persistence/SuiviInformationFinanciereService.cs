using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SuiviEvaluation.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuiviEvaluation.Infrastructure.Data;
using SuiviEvaluation.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Persistence
{
    public class SuiviInformationFinanciereService: ISuiviInformationFinanciereService
    {
        private readonly EvaluationDbContext _dbContext;
        private readonly ILogger<ISuiviInformationFinanciereService> _logger;

        public SuiviInformationFinanciereService(
            EvaluationDbContext dbContext,
            ILogger<ISuiviInformationFinanciereService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(SuiviInformationFinanciereDto suiviInformationFinanciereDto)
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
                entity = "ViewIdentProjetLivrablesPlat",
                action = "insert",
                data = suiviInformationFinanciereDto
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(SuiviInformationFinanciereDto suiviInformationFinanciereDto)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "update",
                data = suiviInformationFinanciereDto
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdInformationsFinancieres)
        {
            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "delete",
                data = new { IdInformationsFinancieres }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task<List<SuiviInformationFinanciereDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<SuiviInformationFinanciereDto>()
                .FromSqlRaw("SELECT * FROM VIEW_IDENT_PROJET_LIVRABLES_PLAT")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SuiviInformationFinanciereDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<SuiviInformationFinanciereDto>()
                .FromSqlRaw(
                    "SELECT * VIEW_IDENT_PROJET_LIVRABLES_PLAT WHERE id_aspects_juridiques = {0}",
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
