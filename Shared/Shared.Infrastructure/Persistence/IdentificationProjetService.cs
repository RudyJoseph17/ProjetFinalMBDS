using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Persistence
{
    public class IdentificationProjetService : IIdentificationProjetService
    {
        private readonly SharedDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IIdentificationProjetService> _logger;

        public IdentificationProjetService(
            SharedDbContext dbContext,
            IMapper mapper,
            ILogger<IIdentificationProjetService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<IdentificationProjetDto>> ObtenirTousAsync()
        {
            // 1. On interroge l’entité EF Core correctement
            var vues = await _dbContext
                .ViewIdentificationProjetPlats
                .AsNoTracking()
                .ToListAsync();

            // 2. On mappe la liste d’entités vers la liste de DTOs
            return _mapper.Map<List<IdentificationProjetDto>>(vues);
        }

        public async Task<IdentificationProjetDto?> ObtenirParIdAsync(string id)
        {
            var vue = await _dbContext
                .ViewIdentificationProjetPlats
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdIdentificationProjet == id);

            return _mapper.Map<IdentificationProjetDto?>(vue);
        }

        public async Task AjouterAsync(IdentificationProjetDto identificationProjet)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentificationProjetPlat",
                action = "insert",
                data = identificationProjet
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(IdentificationProjetDto identificationProjet)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            var payload = new
            {
                entity = "ViewIdentificationProjetPlat",
                action = "update",
                data = identificationProjet
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(string IdIdentificationProjet)
        {
            var payload = new
            {
                entity = "ViewIdentificationProjetPlat",
                action = "delete",
                data = new { IdIdentificationProjet }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à AJOUTER_PROJET_ET_LISTES_JSON : {Json}", json);

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
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
