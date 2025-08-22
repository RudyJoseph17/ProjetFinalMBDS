using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Domain.Helpers;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class DdpCadreLogiqueService : IDdpCadreLogiqueService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<DdpCadreLogiqueService> _logger;

        public DdpCadreLogiqueService(BanquePDbContext dbContext, ILogger<DdpCadreLogiqueService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(DdpCadreLogiqueDto cadre)
        {
            // Même pattern JSON que pour IdentificationProjet
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
                entity = "ddp_cadre_logique",
                action = "insert",
                data = cadre
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à process_ddp_cadre_logique_json : {Json}", json);

            await ExecuteProcedureAsync("process_ddp_cadre_logique_json", json);
        }

        public async Task MettreAJourAsync(DdpCadreLogiqueDto cadre)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ddp_cadre_logique",
                action = "update",
                data = cadre
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à process_ddp_cadre_logique_json : {Json}", json);

            await ExecuteProcedureAsync("process_ddp_cadre_logique_json", json);
        }

        public async Task SupprimerAsync(byte id)
        {
            var payload = new
            {
                entity = "ddp_cadre_logique",
                action = "delete",
                data = new { id }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à process_ddp_cadre_logique_json : {Json}", json);

            await ExecuteProcedureAsync("process_ddp_cadre_logique_json", json);
        }

        public async Task<List<DdpCadreLogiqueDto>> ObtenirTousAsync()
        {
            return await _dbContext.Set<DdpCadreLogiqueDto>()
                                   .FromSqlRaw("SELECT * FROM o_view_DDP_CADRE_LOGIQUE")
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdAsync(byte id)
        {
            return await _dbContext.Set<DdpCadreLogiqueDto>()
                                   .FromSqlRaw("SELECT * FROM o_view_DDP_CADRE_LOGIQUE WHERE ID_DDP_CADRE_LOGIQUE = {0}", id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();
        }

        private async Task ExecuteProcedureAsync(string procedureName, string json)
        {
            // On récupère une nouvelle connexion pour chaque appel
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
            // la fermeture de la connexion se fait automatiquement via 'await using'
        }

        Task IDdpCadreLogiqueService.GetNextIdAsync(byte IdDdpCadreLogique)
        {
            throw new NotImplementedException();
        }

        Task<byte> IDdpCadreLogiqueService.GetNextIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
