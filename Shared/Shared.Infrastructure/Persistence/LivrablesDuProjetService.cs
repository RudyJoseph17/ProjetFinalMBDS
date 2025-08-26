using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using Shared.Domain.Dtos;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Persistence
{
    public class LivrablesDuProjetService : ILivrablesDuProjet
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<ILivrablesDuProjet> _logger;

        public LivrablesDuProjetService(
            SharedDbContext dbContext,
            ILogger<ILivrablesDuProjet> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(LivrablesDuProjetDto livrablesDuProjet)
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
                entity = "OViewLivrablesDuProjet",
                action = "insert",
                data = livrablesDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(LivrablesDuProjetDto livrablesDuProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewLivrablesDuProjet",
                action = "update",
                data = livrablesDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(byte IdLivrablesProjet)
        {
            var payload = new
            {
                entity = "OViewLivrablesDuProjet",
                action = "delete",
                data = new { IdLivrablesProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON ", json);
        }

        public async Task<List<LivrablesDuProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<LivrablesDuProjetDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_ACTIVITES_ANNUELLES")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LivrablesDuProjetDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<LivrablesDuProjetDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_LIVRABLES_DU_PROJET WHERE ID_LIVRABLES_PROJET = {0}",
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
