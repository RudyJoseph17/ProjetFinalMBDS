using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class DdpCadreLogiqueService : IDdpCadreLogiqueService
    {
        private readonly ILogger<DdpCadreLogiqueService> _logger;

        public DdpCadreLogiqueService(ILogger<DdpCadreLogiqueService> logger)
        {
            _logger = logger;
        }

        private string GetConnectionString()
        {
            // Charger .env explicitement
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

            // 🔍 Vérif explicite si dto contient un IdIdentificationProjet
            if (dto is DdpCadreLogiqueDto cadre)
            {
                if (string.IsNullOrWhiteSpace(cadre.IdIdentificationProjet))
                {
                    _logger.LogError("❌ IdIdentificationProjet est NULL ou vide avant l'appel de {Proc}.", procedureName);
                }
                else
                {
                    _logger.LogInformation("✅ IdIdentificationProjet fourni = {Id}", cadre.IdIdentificationProjet);
                }
            }

            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new OracleCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("p_json", OracleDbType.Clob).Value = jsonPayload;

            _logger.LogInformation("📤 Appel de {Proc} avec JSON = {Json}", procedureName, jsonPayload);

            await command.ExecuteNonQueryAsync();
        }


        public async Task AjouterAsync(DdpCadreLogiqueDto cadreLogique)
            => await ExecuteJsonProcedureAsync("AJOUTER_DDP_CADRE_LOGIQUE_JSON", cadreLogique);

        public async Task MettreAJourAsync(DdpCadreLogiqueDto cadreLogique)
            => await ExecuteJsonProcedureAsync("MAJ_DDP_CADRE_LOGIQUE_JSON", cadreLogique);

        public async Task SupprimerAsync(byte idDdpCadreLogique)
            => await ExecuteJsonProcedureAsync("SUPPRIMER_DDP_CADRE_LOGIQUE_JSON", new { IdDdpCadreLogique = idDdpCadreLogique });

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdAsync(byte id)
        {
            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT * FROM O_VIEW_DDP_CADRE_LOGIQUE WHERE IdDdpCadreLogique = :id";

            await using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("id", id));

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DdpCadreLogiqueDto
                {
                    IdDdpCadreLogique = reader.GetByte(reader.GetOrdinal("IdDdpCadreLogique")),
                    IntrantsResumeNarratif = reader.GetString(reader.GetOrdinal("IntrantsResumeNarratif")),
                    ExtrantsResumeNarratif = reader.GetString(reader.GetOrdinal("IntrantsResumeNarratif"))
                };
            }
            return null;
        }

        public async Task<List<DdpCadreLogiqueDto>> ObtenirTousAsync()
        {
            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT * FROM O_VIEW_DDP_CADRE_LOGIQUE";

            await using var command = new OracleCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            var result = new List<DdpCadreLogiqueDto>();
            while (await reader.ReadAsync())
            {
                result.Add(new DdpCadreLogiqueDto
                {
                    IdDdpCadreLogique = reader.GetByte(reader.GetOrdinal("IdDdpCadreLogique")),
                    IntrantsResumeNarratif = reader.GetString(reader.GetOrdinal("IntrantsResumeNarratif ")),
                    ExtrantsResumeNarratif = reader.GetString(reader.GetOrdinal("ExtrantsResumeNarratif"))
                });
            }
            return result;
        }

        public async Task<byte> GetNextIdAsync()
        {
            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT NVL(MAX(IdDdpCadreLogique), 0) + 1 FROM O_VIEW_DDP_CADRE_LOGIQUE";

            await using var command = new OracleCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();

            return Convert.ToByte(result);
        }

        // Supprime l'autre signature inutile
        public Task GetNextIdAsync(byte idDdpCadreLogique)
            => throw new NotImplementedException();

        public async Task<DdpCadreLogiqueDto?> ObtenirParIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var connectionString = GetConnectionString();
            await using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT * FROM O_VIEW_DDP_CADRE_LOGIQUE WHERE ID_IDENTIFICATION_PROJET = :id";

            await using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("id", id));

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DdpCadreLogiqueDto
                {
                    IdIdentificationProjet = reader.GetString(reader.GetOrdinal("ID_IDENTIFICATION_PROJET")),
                    IntrantsResumeNarratif = reader.GetString(reader.GetOrdinal("IntrantsResumeNarratif")),
                    ExtrantsResumeNarratif = reader.GetString(reader.GetOrdinal("ExtrantsResumeNarratif"))
                };
            }

            return null;
        }

    }
}
