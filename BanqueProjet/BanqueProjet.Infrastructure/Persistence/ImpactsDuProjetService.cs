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
    public class ImpactsDuProjetService: IImpactsDuProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IImpactsDuProjetService> _logger;

        public ImpactsDuProjetService(
            BanquePDbContext dbContext,
            ILogger<IImpactsDuProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(ImpactsDuProjetDto impactsProjet)
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
                entity = "OViewImpactsDuProjet",
                action = "insert",
                data = impactsProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Impacts_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Impacts_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(ImpactsDuProjetDto impactsProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewImpactsDuProjet",
                action = "update",
                data = impactsProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Impacts_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Impacts_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdImpactsProjet)
        {
            var payload = new
            {
                entity = "aspects_juridiques",
                action = "delete",
                data = new { IdImpactsProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Impacts_Projet_JSON: {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Impacts_Projet_JSON", json);
        }

        public async Task<List<ImpactsDuProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<ImpactsDuProjetDto>()
                .FromSqlRaw("SELECT * FROM VIEW_IDENT_PROJET_IMPACTS_PLAT")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AspectsJuridiquesDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<AspectsJuridiquesDto>()
                .FromSqlRaw(
                    "SELECT * FROM VIEW_IDENT_PROJET_IMPACTS_PLAT WHERE ID_IDENTIFICATION_PROJET = {0}",
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
