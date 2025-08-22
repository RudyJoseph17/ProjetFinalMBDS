using BanqueProjet.Application.Dtos;
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
using BanqueProjet.Application.Interfaces;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class GestionDeProjetService: IGestionDeProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<GestionDeProjetService> _logger;

        public GestionDeProjetService(
            BanquePDbContext dbContext,
            ILogger<GestionDeProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(GestionDeProjetDto gestionDeProjet)
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
                entity = "gestion_de_projet",
                action = "insert",
                data = gestionDeProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à process_gestion_de_projet_json : {Json}", json);

            await ExecuteProcedureAsync("process_gestion_de_projet_json", json);
        }

        public async Task MettreAJourAsync(GestionDeProjetDto gestionDeProjet)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "gestion_de_projet",
                action = "update",
                data = gestionDeProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à process_gestion_de_projet_json  : {Json}", json);

            await ExecuteProcedureAsync("process_gestion_de_projet_json", json);
        }

        public async Task SupprimerAsync(byte IdGestionDeProjet)
        {
            var payload = new
            {
                entity = "gestion_de_projet",
                action = "delete",
                data = new { IdGestionDeProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à process_gestion_de_projet_json  : {Json}", json);

            await ExecuteProcedureAsync("process_gestion_de_projet_json", json);
        }

        public async Task<List<GestionDeProjetDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<GestionDeProjetDto>()
                .FromSqlRaw("SELECT * FROM O_VIEW_GESTION_DE_PROJET")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GestionDeProjetDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<GestionDeProjetDto>()
                .FromSqlRaw(
                    "SELECT * FROM O_VIEW_GESTION_DE_PROJET WHERE id_gestion_de_projet = {0}",
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
