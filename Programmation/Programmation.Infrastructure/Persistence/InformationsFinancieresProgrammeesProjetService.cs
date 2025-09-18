using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using Programmation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Programmation.Infrastructure.Persistence
{
    public class InformationsFinancieresProgrammeesProjetService : IInformationsFinancieresProgrammeesProjetService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<InformationsFinancieresProgrammeesProjetService> _logger;

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public InformationsFinancieresProgrammeesProjetService(
            ProgrammationDbContext db,
            ILogger<InformationsFinancieresProgrammeesProjetService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AjouterAsync(InformationsFinancieresProgrammeesProjetDto info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            var json = JsonConvert.SerializeObject(info, _jsonSettings);
            const string sql = "BEGIN AJOUTER_INFOS_FINANCIERES_PROGRAMMEES_PROJET_JSON(:p_json); END;";

            var param = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json,
                Direction = System.Data.ParameterDirection.Input
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'appel de {Proc}.", "AJOUTER_INFOS_FINANCIERES_PROGRAMMEES_PROJET_JSON");
                throw;
            }
        }

        public async Task MettreAJourAsync(InformationsFinancieresProgrammeesProjetDto info)
        {
            // Idempotent pour l'instant : réutilise la même procédure d'insert/update JSON.
            await AjouterAsync(info);
        }

        public async Task SupprimerAsync(byte IdInformationsFinancieresProjet)
        {
            const string sql = "BEGIN SUPPRIMER_INFOS_FINANCIERES_PROGRAMMEES_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Byte)
            {
                Value = IdInformationsFinancieresProjet,
                Direction = System.Data.ParameterDirection.Input
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de Id={Id}.", IdInformationsFinancieresProjet);
                throw;
            }
        }

        public async Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirTousAsync()
        {
            const string sql = @"
                SELECT
                    ID_INFORMATION_FINANCIERE,
                    ID_IDENTIFICATION_PROJET,
                    ID_ACTIVITE,
                    EXERCICE_FISCAL_DEBUT,
                    EXERCICE_FISCAL_FIN,
                    SOURCES_FINANCEMENT,
                    ARTICLE,
                    ALINEA,
                    MOIS_PREVISION,
                    MONTANT_PREVU
                FROM O_VIEW_INFOS_FINANCIERES_PROGRAMMEES_PROJET";

            return await ExecuteQueryAndMapAsync(sql, null);
        }

        public async Task<InformationsFinancieresProgrammeesProjetDto?> ObtenirParIdAsync(byte id)
        {
            const string sql = @"
                SELECT
                    ID_INFORMATION_FINANCIERE,
                    ID_IDENTIFICATION_PROJET,
                    ID_ACTIVITE,
                    EXERCICE_FISCAL_DEBUT,
                    EXERCICE_FISCAL_FIN,
                    SOURCES_FINANCEMENT,
                    ARTICLE,
                    ALINEA,
                    MOIS_PREVISION,
                    MONTANT_PREVU
                FROM O_VIEW_INFOS_FINANCIERES_PROGRAMMEES_PROJET
                WHERE ID_INFORMATION_FINANCIERE = :p_id";

            var parameters = new[]
            {
                new OracleParameter("p_id", OracleDbType.Byte) { Value = id, Direction = System.Data.ParameterDirection.Input }
            };

            var list = await ExecuteQueryAndMapAsync(sql, parameters);
            return list.FirstOrDefault();
        }

        // -------------------- NOUVEAUTÉS --------------------

        public async Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirParProjetAsync(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet))
                throw new ArgumentException("idProjet doit être renseigné.", nameof(idProjet));

            const string sql = @"
                SELECT
                    ID_INFORMATION_FINANCIERE,
                    ID_IDENTIFICATION_PROJET,
                    ID_ACTIVITE,
                    EXERCICE_FISCAL_DEBUT,
                    EXERCICE_FISCAL_FIN,
                    SOURCES_FINANCEMENT,
                    ARTICLE,
                    ALINEA,
                    MOIS_PREVISION,
                    MONTANT_PREVU
                FROM O_VIEW_INFOS_FINANCIERES_PROGRAMMEES_PROJET
                WHERE ID_IDENTIFICATION_PROJET = :p_id
                ORDER BY ARTICLE, ALINEA, ID_ACTIVITE";

            var parameters = new[]
            {
                new OracleParameter("p_id", OracleDbType.Varchar2) { Value = idProjet, Direction = System.Data.ParameterDirection.Input }
            };

            return await ExecuteQueryAndMapAsync(sql, parameters);
        }

        public async Task<AggregatedInformationsFinancieresProjetDto> ObtenirAggregationParProjetAsync(string idProjet)
        {
            var items = await ObtenirParProjetAsync(idProjet);
            var agg = AggregatedInformationsFinancieresProjetDto.From(items, idProjet);
            return agg;
        }

        // -------------------- Helpers privés --------------------

        /// <summary>
        /// Execute la requête SQL fournie (avec paramètres optionnels) et mappe les résultats vers le DTO.
        /// Centralise la logique de connexion / lecture / mapping.
        /// </summary>
        private async Task<List<InformationsFinancieresProgrammeesProjetDto>> ExecuteQueryAndMapAsync(string sql, OracleParameter[]? parameters)
        {
            var result = new List<InformationsFinancieresProgrammeesProjetDto>();

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                if (parameters != null)
                {
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);
                }

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = MapReaderToDto(reader);
                    result.Add(dto);
                }
            }

            return result;
        }

        private static InformationsFinancieresProgrammeesProjetDto MapReaderToDto(DbDataReader reader)
        {
            var dto = new InformationsFinancieresProgrammeesProjetDto();

            dto.IdInformationsFinancieres = SafeGetByte(reader, "ID_INFORMATION_FINANCIERE") ?? 0;
            dto.IdActivite = SafeGetInt(reader, "ID_ACTIVITE") ?? default;
            dto.ExerciceFiscalDebut = SafeGetByte(reader, "EXERCICE_FISCAL_DEBUT");
            dto.ExerciceFiscalFin = SafeGetByte(reader, "EXERCICE_FISCAL_FIN");
            dto.SourcesFinancement = SafeGetString(reader, "SOURCES_FINANCEMENT");
            dto.Article = SafeGetString(reader, "ARTICLE");
            dto.Alinea = SafeGetString(reader, "ALINEA");
            dto.MoisPrevision = SafeGetString(reader, "MOIS_PREVISION");
            dto.MontantPrevu = SafeGetDecimal(reader, "MONTANT_PREVU");

            return dto;
        }

        private static string? SafeGetString(DbDataReader r, string columnName)
        {
            try
            {
                var ord = r.GetOrdinal(columnName);
                return r.IsDBNull(ord) ? null : r.GetString(ord);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        private static byte? SafeGetByte(DbDataReader r, string columnName)
        {
            try
            {
                var ord = r.GetOrdinal(columnName);
                if (r.IsDBNull(ord)) return null;
                var val = r.GetValue(ord);
                return Convert.ToByte(val);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int? SafeGetInt(DbDataReader r, string columnName)
        {
            try
            {
                var ord = r.GetOrdinal(columnName);
                if (r.IsDBNull(ord)) return null;
                var val = r.GetValue(ord);
                return Convert.ToInt32(val);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static decimal? SafeGetDecimal(DbDataReader r, string columnName)
        {
            try
            {
                var ord = r.GetOrdinal(columnName);
                if (r.IsDBNull(ord)) return null;
                var val = r.GetValue(ord);
                return Convert.ToDecimal(val);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
