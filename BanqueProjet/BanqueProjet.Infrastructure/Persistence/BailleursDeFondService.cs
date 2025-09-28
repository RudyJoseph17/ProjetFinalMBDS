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
    public class BailleursDeFondService: IBailleursDeFondService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IBailleursDeFondService> _logger;

        public BailleursDeFondService(
            BanquePDbContext dbContext,
            ILogger<IBailleursDeFondService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(BailleursDeFondsDto BailleursDeFond)
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
                entity = "ViewIdentProjetBailleursPlat",
                action = "insert",
                data = BailleursDeFond
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task MettreAJourAsync(BailleursDeFondsDto BailleursDeFond)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var payload = new
            {
                entity = "ViewIdentProjetBailleursPlat",
                action = "update",
                data = BailleursDeFond
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
            _logger.LogInformation("🔄 JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task SupprimerAsync(byte IdBailleursDeFonds)
        {
            var payload = new
            {
                entity = "ViewIdentProjetBailleursPlat",
                action = "delete",
                data = new { IdBailleursDeFonds }
            };

            var json = JsonConvert.SerializeObject(payload);
            _logger.LogInformation("🗑️ JSON envoyé à PROCESS_Bailleurs_De_Fonds_JSON : {Json}", json);

            await ExecuteProcedureAsync("PROCESS_Bailleurs_De_Fonds_JSON", json);
        }

        public async Task<List<BailleursDeFondsDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.ViewProjetBailleursPlats
    .AsNoTracking()
    .ToListAsync();

            // Mappez-les vers vos DTOs métier
            return entities.Select(e => new BailleursDeFondsDto
            {
                IdBailleursDeFonds = (byte)e.IdBailleursDeFonds,
                NomBailleur = e.NomBailleur,
                TelephoneRepresentant = e.TelephoneRepresentant,
                CourrielRepresentant = e.CourrielRepresentant,
                IdIdentificationProjet = e.IdIdentificationProjet
            })
            .ToList();

        }

        public async Task<BailleursDeFondsDto?> ObtenirParIdAsync(byte id)
        {
            var e = await _dbContext.ViewProjetBailleursPlats
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.IdBailleursDeFonds == id);

            if (e == null) return null;

            return new BailleursDeFondsDto
            {
                IdBailleursDeFonds = (byte)e.IdBailleursDeFonds,
                NomBailleur = e.NomBailleur,
                TelephoneRepresentant = e.TelephoneRepresentant,
                CourrielRepresentant = e.CourrielRepresentant,
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

        public Task SupprimerAsync(int idBailleursDeFonds)
        {
            throw new NotImplementedException();
        }
    }
}
