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
    public class DefinitionLivrablesDuProjetService: IDefinitionLivrablesDuProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IDefinitionLivrablesDuProjetService> _logger;

        public DefinitionLivrablesDuProjetService(
            BanquePDbContext dbContext,
            ILogger<IDefinitionLivrablesDuProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(DefinitionLivrablesDuProjetDto indicateursDeResultats)
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
                data = indicateursDeResultats
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Livrables_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Livrables_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(DefinitionLivrablesDuProjetDto indicateursDeResultats)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetLivrablesPlat",
                action = "update",
                data = indicateursDeResultats
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

        public async Task<List<DefinitionLivrablesDuProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<DefinitionLivrablesDuProjetDto>()
                .FromSqlRaw("SELECT * FROM VIEW_IDENT_PROJET_LIVRABLES_PLAT")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DefinitionLivrablesDuProjetDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<DefinitionLivrablesDuProjetDto>()
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
