using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Persistence
{
    public class ActiviteService: IActiviteService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<IActiviteService> _logger;

        public ActiviteService(
            SharedDbContext dbContext,
            ILogger<IActiviteService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(ActiviteDto activite)
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
                entity = "ViewIdentProjetActivitesPlat",
                action = "insert",
                data = activite
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(ActiviteDto activite)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetActivitesPlat",
                action = "update",
                data = activite
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(byte IdActivites)
        {
            var payload = new
            {
                entity = "ViewIdentProjetActivitesPlat",
                action = "delete",
                data = new { IdActivites }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task<List<ActiviteDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<ActiviteDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_ACTIVITES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ActiviteDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<ActiviteDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_ACTIVITES WHERE ID_ACTIVITES = {0}",
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
