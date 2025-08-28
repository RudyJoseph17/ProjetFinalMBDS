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
    public class QuantiteLivreParAnneeService: IQuantiteLivreParAnneeService
    {
        private readonly EvaluationDbContext _dbContext;
        private readonly ILogger<IQuantiteLivreParAnneeService> _logger;

        public QuantiteLivreParAnneeService(
            EvaluationDbContext dbContext,
            ILogger<IQuantiteLivreParAnneeService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(QuantiteLivreeParAnneeDto quantiteLivreParAnnee)
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
                data = quantiteLivreParAnnee
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(QuantiteLivreeParAnneeDto quantiteLivreParAnnee)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "update",
                data = quantiteLivreParAnnee
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdLivrablesProjet)
        {
            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "delete",
                data = new { IdLivrablesProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task<List<QuantiteLivreeParAnneeDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<QuantiteLivreeParAnneeDto>()
                .FromSqlRaw("SELECT * FROM VIEW_IDENT_PROJET_LIVRABLES_PLAT")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<QuantiteLivreeParAnneeDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<QuantiteLivreeParAnneeDto>()
                .FromSqlRaw(
                    "SELECT * VIEW_IDENT_PROJET_LIVRABLES_PLAT WHERE ID_LIVRABLES_PROJET = {0}",
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
