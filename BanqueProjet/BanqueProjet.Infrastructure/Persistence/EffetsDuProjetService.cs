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
    public class EffetsDuProjetService: IEffetsDuProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IEffetsDuProjetService> _logger;

        public EffetsDuProjetService(
            BanquePDbContext dbContext,
            ILogger<IEffetsDuProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(EffetsDuProjetDto effetsDuProjet)
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
                entity = "OViewEffetsDuProjet",
                action = "insert",
                data = effetsDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Effets_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Effets_Projet_JSON", json);
        }

        public async Task MettreAJourAsync(EffetsDuProjetDto effetsDuProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "OViewEffetsDuProjet",
                action = "update",
                data = effetsDuProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Effets_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Effets_Projet_JSON", json);
        }

        public async Task SupprimerAsync(byte IdEffetsDuProjet)
        {
            var payload = new
            {
                entity = "OViewEffetsDuProjet",
                action = "delete",
                data = new { IdEffetsDuProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Effets_Projet_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Effets_Projet_JSON", json);
        }

        public async Task<List<EffetsDuProjetDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.OViewEffetsDuProjet
                .AsNoTracking()
                .ToListAsync();

            // Mappez-les vers vos DTOs métier
            return entities.Select(e => new EffetsDuProjetDto
            {
                IdEffetsDuProjet = (byte)e.IdEffetsDuProjet,
                EffetDuProjet = e.EffetDuProjet,
                IdIdentificationProjet = e.IdIdentificationProjet
            })
            .ToList();
        }

        public async Task<EffetsDuProjetDto?> ObtenirParIdAsync(byte id)
        {
            var e = await _dbContext.OViewEffetsDuProjet
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.IdEffetsDuProjet == id);

            if (e == null) return null;

            return new EffetsDuProjetDto
            {
                IdEffetsDuProjet = (byte)e.IdEffetsDuProjet,
                EffetDuProjet = e.EffetDuProjet,
                IdIdentificationProjet = e.IdIdentificationProjet
            };

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
