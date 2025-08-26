using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Infrastructure.Data;
using Shared.Domain.Interface;
using Shared.Domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Persistence
{
    public class InformationsFinancieresService: IInformationsFinancieresService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<IInformationsFinancieresService> _logger;

        public InformationsFinancieresService(
            SharedDbContext dbContext,
            ILogger<IInformationsFinancieresService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(InformationsFinancieresProjetDto informationsFinancieresProjet)
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
                data = informationsFinancieresProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(InformationsFinancieresProjetDto informationsFinancieresProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewActivitesIformationsFinanciere",
                action = "update",
                data = informationsFinancieresProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
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
            _logger.LogInformation("🗑️ JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task<List<InformationsFinancieresProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<InformationsFinancieresProjetDto>()
                .FromSqlRaw("SELECT * FROM VIEW_ACTIVITES_IFORMATIONS_FINANCIERES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InformationsFinancieresProjetDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<InformationsFinancieresProjetDto>()
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
