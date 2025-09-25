using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.Helpers;
using Shared.Domain.Interface;
using Shared.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class DdpCadreLogiqueService : IDdpCadreLogiqueService
    {
        private readonly ILogger<DdpCadreLogiqueService> _logger;
        private readonly BanquePDbContext _dbContext;

        public DdpCadreLogiqueService(ILogger<DdpCadreLogiqueService> logger,
            BanquePDbContext dbContext)
        {
            _logger = logger;

            _dbContext = dbContext;
        }

        private string GetConnectionString()
        {
            DotNetEnv.Env.Load();

            var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER")?.Trim();
            var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD")?.Trim();
            var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST")?.Trim();
            var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT")?.Trim();
            var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE")?.Trim();

            if (string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(port) ||
                string.IsNullOrWhiteSpace(service))
            {
                throw new InvalidOperationException("❌ Variables d'environnement Oracle manquantes ou invalides dans le fichier .env");
            }

            var conn = $"User Id={user};Password={password};Data Source={host}:{port}/{service};Pooling=true;";

            _logger.LogInformation("🔧 Chaîne de connexion générée (password masqué) : {Conn}", MaskPassword(conn));

            return conn;
        }

        private static string MaskPassword(string connString)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                connString,
                @"Password=.*?(;|$)",
                "Password=******$1",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        private async Task ExecuteJsonProcedureAsync(string procedureName, object dto)
        {
            var jsonPayload = JsonConvert.SerializeObject(dto, JsonSettings.CamelCase);

            if (dto is DdpCadreLogiqueDto cadre)
            {
                if (string.IsNullOrWhiteSpace(cadre.IdIdentificationProjet))
                    _logger.LogError("❌ IdIdentificationProjet est NULL ou vide avant l'appel de {Proc}.", procedureName);
                else
                    _logger.LogInformation("✅ IdIdentificationProjet fourni = {Id}", cadre.IdIdentificationProjet);
            }

            // Conversion en UPPER_SNAKE_CASE
            JToken TransformKeysToOracle(JToken token)
            {
                if (token is JObject o)
                {
                    var newObj = new JObject();
                    foreach (var prop in o.Properties())
                    {
                        string key = prop.Name;
                        var sb = new System.Text.StringBuilder();
                        for (int i = 0; i < key.Length; i++)
                        {
                            char ch = key[i];
                            if (char.IsUpper(ch) && i > 0)
                                sb.Append('_').Append(ch);
                            else
                                sb.Append(char.ToUpperInvariant(ch));
                        }
                        var newKey = sb.ToString();
                        newObj[newKey] = TransformKeysToOracle(prop.Value);
                    }
                    return newObj;
                }
                else if (token is JArray a)
                {
                    var newArr = new JArray();
                    foreach (var item in a) newArr.Add(TransformKeysToOracle(item));
                    return newArr;
                }
                else
                {
                    return token;
                }
            }

            var parsed = JToken.Parse(jsonPayload);
            var transformed = TransformKeysToOracle(parsed);
            var jsonForOracle = transformed.ToString(Formatting.None);

            _logger.LogInformation("📤 Appel de {Proc} avec JSON final = {Json}", procedureName, jsonForOracle);

            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new OracleCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.BindByName = true;
            command.Parameters.Add(new OracleParameter("p_json", OracleDbType.Clob, ParameterDirection.Input)
            {
                Value = jsonForOracle
            });

            await command.ExecuteNonQueryAsync();
        }

        // 🔹 Méthodes CRUD
        public async Task AjouterAsync(DdpCadreLogiqueDto cadreLogique)
            => await ExecuteJsonProcedureAsync("AJOUTER_DDP_CADRE_LOGIQUE_JSON", cadreLogique);

        public async Task MettreAJourAsync(DdpCadreLogiqueDto cadreLogique)
            => await ExecuteJsonProcedureAsync("MAJ_DDP_CADRE_LOGIQUE_JSON", cadreLogique);

        public async Task SupprimerAsync(byte idDdpCadreLogique)
            => await ExecuteJsonProcedureAsync("SUPPRIMER_DDP_CADRE_LOGIQUE_JSON", new { IdDdpCadreLogique = idDdpCadreLogique });

        public async Task<List<DdpCadreLogiqueDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.OViewDdpCadreLogiques
        .AsNoTracking()
        .ToListAsync();

            // Mappez-les vers vos DTOs métier
            return entities.Select(e => new DdpCadreLogiqueDto
    {
                IdDdpCadreLogique = (byte)e.IdDdpCadreLogique,
                IntrantsResumeNarratif = e.IntrantsResumeNarratif,
                ExtrantsResumeNarratif = e.ExtrantsResumeNarratif,
                ObjectifsSpecifiquesResumeNarratif = e.ObjectifsSpecifiquesResumeNarratif,
                IntrantsIov = e.IntrantsIov,
                ExtrantsIov = e.ExtrantsIov,
                ObjectifsSpecifiquesIov = e.ObjectifsSpecifiquesIov,
                ObjectifGeneralIov = e.ObjectifGeneralIov,
                IntrantsSmov = e.IntrantsSmov,
                ExtrantsSmov = e.ExtrantsSmov,
                ObjectifsSpecifiquesSmov = e.ObjectifsSpecifiquesSmov,
                ObjectifGeneralSmov = e.ObjectifGeneralSmov,
                IntrantsRisquesHypotheses = e.IntrantsRisquesHypotheses,
                ExtrantsRisquesHypotheses = e.ExtrantsRisquesHypotheses,
                ObjectifsSpecifiquesRisquesHypotheses = e.ObjectifsSpecifiquesRisquesHypotheses,
                ObjectifGeneralRisquesHypotheses = e.ObjectifsSpecifiquesRisquesHypotheses,
                IdIdentificationProjet = e.IdIdentificationProjet
    })
    .ToList();
        }

        public async Task<byte> GetNextIdAsync()
        {
            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT NVL(MAX(ID_DDP_CADRE_LOGIQUE), 0) + 1 FROM O_VIEW_DDP_CADRE_LOGIQUE";

            await using var command = new OracleCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();

            return Convert.ToByte(result);
        }

        public Task GetNextIdAsync(byte IdDdpCadreLogique)
        {
            throw new NotImplementedException();
        }

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet)
        {
            if (string.IsNullOrWhiteSpace(idIdentificationProjet))
                return null;

            // On interroge la vue Oracle via EF Core, filtrée sur la colonne IdIdentificationProjet
            var ent = await _dbContext.OViewDdpCadreLogiques
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdIdentificationProjet == idIdentificationProjet);

            if (ent is null)
                return null;

            // On mappe chaque propriété de la vue vers le DTO
            return new DdpCadreLogiqueDto
            {
                IdDdpCadreLogique = (byte)ent.IdDdpCadreLogique,
                IdIdentificationProjet = ent.IdIdentificationProjet,
                IntrantsResumeNarratif = ent.IntrantsResumeNarratif,
                ExtrantsResumeNarratif = ent.ExtrantsResumeNarratif,
                ObjectifsSpecifiquesResumeNarratif = ent.ObjectifsSpecifiquesResumeNarratif,
                ObjectifGeneralResumeNarratif = ent.ObjectifGeneralResumeNarratif,
                IntrantsIov = ent.IntrantsIov,
                ExtrantsIov = ent.ExtrantsIov,
                ObjectifsSpecifiquesIov = ent.ObjectifsSpecifiquesIov,
                ObjectifGeneralIov = ent.ObjectifGeneralIov,
                IntrantsSmov = ent.IntrantsSmov,
                ExtrantsSmov = ent.ExtrantsSmov,
                ObjectifsSpecifiquesSmov = ent.ObjectifsSpecifiquesSmov,
                ObjectifGeneralSmov = ent.ObjectifGeneralSmov,
                IntrantsRisquesHypotheses = ent.IntrantsRisquesHypotheses,
                ExtrantsRisquesHypotheses = ent.ExtrantsRisquesHypotheses,
                ObjectifsSpecifiquesRisquesHypotheses = ent.ObjectifsSpecifiquesRisquesHypotheses,
                ObjectifGeneralRisquesHypotheses = ent.ObjectifGeneralRisquesHypotheses,
                // … ajoutez ici tout autre champ retourné par votre vue …
            };
        }
    }
}
