using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using Programmation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Persistence
{
    public class QuantiteALivrerParAnneeService : IQuantiteALivrerParAnneeService
    {
        private readonly ProgrammationDbContext _dbContext;
        private readonly ILogger<QuantiteALivrerParAnneeService> _logger;

        public QuantiteALivrerParAnneeService(
            ProgrammationDbContext dbContext,
            ILogger<QuantiteALivrerParAnneeService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(QuantiteALivrerParAnneeDto quantiteALivrerParAnnee)
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
                data = quantiteALivrerParAnnee
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(QuantiteALivrerParAnneeDto quantiteALivrerParAnnee)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "update",
                data = quantiteALivrerParAnnee
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

        public async Task<List<QuantiteALivrerParAnneeDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<QuantiteALivrerParAnneeDto>()
                .FromSqlRaw("SELECT * FROM VIEW_IDENT_PROJET_LIVRABLES_PLAT")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<QuantiteALivrerParAnneeDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<QuantiteALivrerParAnneeDto>()
                .FromSqlRaw(
                    "SELECT * FROM VIEW_IDENT_PROJET_LIVRABLES_PLAT WHERE ID_LIVRABLES_PROJET = {0}",
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
