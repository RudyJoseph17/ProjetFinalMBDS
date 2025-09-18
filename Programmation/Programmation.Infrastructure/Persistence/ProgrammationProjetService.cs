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
    /// <summary>
    /// Service de gestion de la programmation par projet.
    /// - Sérialise les DTO en JSON pour les procédures stockées d'écriture.
    /// - Lit les vues pour la lecture et mappe vers les DTOs (robuste face aux NULLs).
    /// </summary>
    public class ProgrammationProjetService : IProgrammationProjetService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<ProgrammationProjetService> _logger;

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public ProgrammationProjetService(ProgrammationDbContext db, ILogger<ProgrammationProjetService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Appelle la procédure AJOUTER_PROGRAMMATION_POUR_PROJET_JSON pour insérer/mettre à jour la programmation d'un projet.
        /// </summary>
        public async Task AjouterAsync(ProgrammationProjetDto programmationProjetDto)
        {
            if (programmationProjetDto == null) throw new ArgumentNullException(nameof(programmationProjetDto));

            var json = JsonConvert.SerializeObject(programmationProjetDto, _jsonSettings);
            const string sql = "BEGIN AJOUTER_PROGRAMMATION_POUR_PROJET_JSON(:p_json); END;";

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
                _logger.LogError(ex, "Erreur lors de l'appel de AJOUTER_PROGRAMMATION_POUR_PROJET_JSON pour le projet {ProjectId}.", programmationProjetDto.IdIdentificationProjet);
                throw;
            }
        }

        /// <summary>
        /// Idempotent pour l'instant : réutilise la même procédure d'insert/update JSON.
        /// </summary>
        public async Task MettreAJourAsync(ProgrammationProjetDto programmationProjetDto)
        {
            await AjouterAsync(programmationProjetDto);
        }

        /// <summary>
        /// Supprime une programmation (interface demande un byte ; adapte si besoin).
        /// </summary>
        public async Task SupprimerAsync(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet))
                throw new ArgumentNullException(nameof(idProjet));

            const string sql = "BEGIN SUPPRIMER_PROGRAMMATION_PROJET(:p_id); END;";

            var param = new OracleParameter("p_id", OracleDbType.Varchar2)
            {
                Value = idProjet,
                Direction = System.Data.ParameterDirection.Input
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la programmation Id={Id}.", idProjet);
                throw;
            }
        }


        /// <summary>
        /// Récupère la liste des projets (sans charger les collections enfants pour performance).
        /// </summary>
        public async Task<List<ProgrammationProjetDto>> ObtenirTousAsync()
        {
            const string sql = @"
                SELECT ID_IDENTIFICATION_PROJET, NOM_PROJET
                FROM VIEW_IDENTIFICATION_PROJET_PLAT";

            var result = new List<ProgrammationProjetDto>();
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new ProgrammationProjetDto
                    {
                        IdIdentificationProjet = SafeGetString(reader, "ID_IDENTIFICATION_PROJET") ?? string.Empty,
                        NomProjet = SafeGetString(reader, "NOM_PROJET")
                    };
                    result.Add(dto);
                }
            }

            return result;
        }

        /// <summary>
        /// Récupère la programmation d'un projet par identifiant et charge ses livrables et informations financières programmées.
        /// </summary>
        public async Task<ProgrammationProjetDto?> ObtenirParIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id doit être renseigné.", nameof(id));

            const string sql = @"
                SELECT ID_IDENTIFICATION_PROJET, NOM_PROJET
                FROM VIEW_IDENTIFICATION_PROJET_PLAT
                WHERE ID_IDENTIFICATION_PROJET = :p_id";

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (!await reader.ReadAsync()) return null;

                var projet = new ProgrammationProjetDto
                {
                    IdIdentificationProjet = SafeGetString(reader, "ID_IDENTIFICATION_PROJET") ?? string.Empty,
                    NomProjet = SafeGetString(reader, "NOM_PROJET")
                };

                // Charger collections enfants (livrables + infos financières)
                projet.LivrablesProgrammesProjets = await GetLivrablesByProjectAsync(id);
                projet.InformationsFinancieresProgrammeesProjet = await GetInformationsFinancieresByProjectAsync(id);

                return projet;
            }
        }

        // -------------------------
        // Helpers privés
        // -------------------------

        private async Task<List<LivrablesProgrameProjetDto>> GetLivrablesByProjectAsync(string idProjet)
        {
            const string sql = @"
                SELECT
                    ID_LIVRABLES_PROJET,
                    ID_IDENTIFICATION_PROJET,
                    ID_ACTIVITE,
                    EXERCICE_FISCAL_DEBUT,
                    EXERCICE_FISCAL_FIN,
                    QUANTITE_A_LIVRER
                FROM O_VIEW_LIVRABLES_PROJET
                WHERE ID_IDENTIFICATION_PROJET = :p_id
                ORDER BY ID_ACTIVITE";

            var list = new List<LivrablesProgrameProjetDto>();
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = idProjet;
                cmd.Parameters.Add(p);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(MapLivrableReaderToDto(reader));
                }
            }

            return list;
        }

        private async Task<List<InformationsFinancieresProgrammeesProjetDto>> GetInformationsFinancieresByProjectAsync(string idProjet)
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
                WHERE ID_IDENTIFICATION_PROJET = :p_id
                ORDER BY ARTICLE, ALINEA, ID_ACTIVITE";

            var list = new List<InformationsFinancieresProgrammeesProjetDto>();
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = idProjet;
                cmd.Parameters.Add(p);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(MapInfoReaderToDto(reader));
                }
            }

            return list;
        }

        private static LivrablesProgrameProjetDto MapLivrableReaderToDto(DbDataReader reader)
        {
            return new LivrablesProgrameProjetDto
            {
                IdLivrablesProjet = SafeGetByte(reader, "ID_LIVRABLES_PROJET") ?? 0,
                IdIdentificationProjet = SafeGetString(reader, "ID_IDENTIFICATION_PROJET") ?? string.Empty,
                ExerciceFiscalDebut = SafeGetByte(reader, "EXERCICE_FISCAL_DEBUT"),
                ExerciceFiscalFin = SafeGetByte(reader, "EXERCICE_FISCAL_FIN"),
                QuantiteALivrer = SafeGetInt(reader, "QUANTITE_A_LIVRER")
            };
        }

        private static InformationsFinancieresProgrammeesProjetDto MapInfoReaderToDto(DbDataReader reader)
        {
            return new InformationsFinancieresProgrammeesProjetDto
            {
                IdInformationsFinancieres = SafeGetByte(reader, "ID_INFORMATION_FINANCIERE") ?? 0,
                IdActivite = SafeGetInt(reader, "ID_ACTIVITE") ?? default,
                ExerciceFiscalDebut = SafeGetByte(reader, "EXERCICE_FISCAL_DEBUT"),
                ExerciceFiscalFin = SafeGetByte(reader, "EXERCICE_FISCAL_FIN"),
                SourcesFinancement = SafeGetString(reader, "SOURCES_FINANCEMENT"),
                Article = SafeGetString(reader, "ARTICLE"),
                Alinea = SafeGetString(reader, "ALINEA"),
                MoisPrevision = SafeGetString(reader, "MOIS_PREVISION"),
                MontantPrevu = SafeGetDecimal(reader, "MONTANT_PREVU")
            };
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
