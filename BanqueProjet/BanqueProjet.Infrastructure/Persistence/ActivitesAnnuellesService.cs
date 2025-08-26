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
    public class ActivitesAnnuellesService: IActivitesAnnuellesService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IActivitesAnnuellesService> _logger;

        public ActivitesAnnuellesService(
            BanquePDbContext dbContext,
            ILogger<IActivitesAnnuellesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(ActivitesAnnuellesDto activitesAnnuelles)
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
                entity = "activites_annuelles",
                action = "insert",
                data = activitesAnnuelles
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_ACTIVITES_ANNUELLES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ACTIVITES_ANNUELLES_JSON", json);
        }

        public async Task MettreAJourAsync(ActivitesAnnuellesDto activitesAnnuelles)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "activites_annuelles",
                action = "update",
                data = activitesAnnuelles
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_ACTIVITES_ANNUELLES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ACTIVITES_ANNUELLES_JSON", json);
        }

        public async Task SupprimerAsync(byte IdActivitesAnnuelles)
        {
            var payload = new
            {
                entity = "activites_annuelles",
                action = "delete",
                data = new { IdActivitesAnnuelles }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_ACTIVITES_ANNUELLES_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_ACTIVITES_ANNUELLES_JSON", json);
        }

        public async Task<List<ActivitesAnnuellesDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<ActivitesAnnuellesDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_ACTIVITES_ANNUELLES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ActivitesAnnuellesDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<ActivitesAnnuellesDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_ACTIVITES_ANNUELLES WHERE id_activites_annuelles = {0}",
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
