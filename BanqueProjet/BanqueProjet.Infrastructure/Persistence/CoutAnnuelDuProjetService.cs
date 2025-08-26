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
    public class CoutAnnuelDuProjetService: ICoutAnnuelDuProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<ICoutAnnuelDuProjetService> _logger;

        public CoutAnnuelDuProjetService(
            BanquePDbContext dbContext,
            ILogger<ICoutAnnuelDuProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(CoutAnnuelDuProjetDto coutAnnuelDuProjet)
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
                entity = "OViewCoutAnnuelDuProjet",
                action = "insert",
                data = coutAnnuelDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Cout_Annuel_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Cout_Annuel_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(CoutAnnuelDuProjetDto coutAnnuelDuProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewCoutAnnuelDuProjet",
                action = "update",
                data = coutAnnuelDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Cout_Annuel_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Cout_Annuel_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdCoutAnnuelProjet)
        {
            var payload = new
            {
                entity = "OViewCoutAnnuelDuProjet",
                action = "delete",
                data = new { IdCoutAnnuelProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Cout_Annuel_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Cout_Annuel_Projet_JSON", json);
        }

        public async Task<List<CoutAnnuelDuProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<CoutAnnuelDuProjetDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_COUT_ANNUEL_DU_PROJET")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CoutAnnuelDuProjetDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<CoutAnnuelDuProjetDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_COUT_ANNUEL_DU_PROJET WHERE ID_COUT_ANNUEL_PROJET = {0}",
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
