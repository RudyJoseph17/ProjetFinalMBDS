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
    /// Service de persistance pour les livrables programmés d'un projet.
    /// </summary>
    public class LivrablesProjetService : ILivrablesProjetService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<LivrablesProjetService> _logger;

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public LivrablesProjetService(
            ProgrammationDbContext db,
            ILogger<LivrablesProjetService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Appelle la procédure AJOUTER_LIVRABLES_PROGRAMME_PROJET_JSON pour insérer/mettre à jour un livrable.
        /// </summary>
        public async Task AjouterAsync(LivrablesProgrameProjetDto livrable)
        {
            if (livrable == null) throw new ArgumentNullException(nameof(livrable));

            var json = JsonConvert.SerializeObject(livrable, _jsonSettings);
            const string sql = "BEGIN AJOUTER_LIVRABLES_PROGRAMME_PROJET_JSON(:p_json); END;";

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
                _logger.LogError(ex, "Erreur lors de l'appel de AJOUTER_LIVRABLES_PROGRAMME_PROJET_JSON pour le projet {ProjectId}.", livrable.IdIdentificationProjet);
                throw;
            }
        }

        /// <summary>
        /// Actuellement idempotent : utilise la même procédure d'insert/update JSON.
        /// </summary>
        public async Task MettreAJourAsync(LivrablesProgrameProjetDto livrable)
        {
            await AjouterAsync(livrable);
        }

        /// <summary>
        /// Supprime un livrable via la procédure SUPPRIMER_LIVRABLES_PROJET.
        /// </summary>
        public async Task SupprimerAsync(byte IdLivrablesProjet)
        {
            const string sql = "BEGIN SUPPRIMER_LIVRABLES_PROJET(:p_id); END;";

            var param = new OracleParameter("p_id", OracleDbType.Byte)
            {
                Value = IdLivrablesProjet,
                Direction = System.Data.ParameterDirection.Input
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du livrable Id={Id}.", IdLivrablesProjet);
                throw;
            }
        }

        /// <summary>
        /// Récupère tous les livrables depuis la vue O_VIEW_LIVRABLES_PROJET.
        /// Mapping robuste vers le DTO.
        /// </summary>
        public async Task<List<LivrablesProgrameProjetDto>> ObtenirTousAsync()
        {
            const string sql = @"
                SELECT
                    ID_LIVRABLES_PROJET,
                    ID_IDENTIFICATION_PROJET,
                    ID_ACTIVITE,
                    EXERCICE_FISCAL_DEBUT,
                    EXERCICE_FISCAL_FIN,
                    QUANTITE_A_LIVRER
                    --, autres colonnes si nécessaire
                FROM O_VIEW_LIVRABLES_PROJET";

            return await ExecuteQueryAndMapAsync(sql, null);
        }

        /// <summary>
        /// Récupère un livrable par identifiant (ID_LIVRABLES_PROJET).
        /// </summary>
        public async Task<LivrablesProgrameProjetDto?> ObtenirParIdAsync(byte id)
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
                WHERE ID_LIVRABLES_PROJET = :p_id";

            var parameters = new[]
            {
                new OracleParameter("p_id", OracleDbType.Byte) { Value = id, Direction = System.Data.ParameterDirection.Input }
            };

            var list = await ExecuteQueryAndMapAsync(sql, parameters);
            return list.FirstOrDefault();
        }

        // ---------------------------
        // Helpers privés
        // ---------------------------

        /// <summary>
        /// Exécute la requête SQL et mappe chaque ligne vers le DTO.
        /// </summary>
        private async Task<List<LivrablesProgrameProjetDto>> ExecuteQueryAndMapAsync(string sql, OracleParameter[]? parameters)
        {
            var result = new List<LivrablesProgrameProjetDto>();
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

        /// <summary>
        /// Mappe un DbDataReader vers LivrablesProgrameProjetDto en gérant les DBNulls.
        /// Noms de colonnes attendus : voir la vue O_VIEW_LIVRABLES_PROJET.
        /// </summary>
        private static LivrablesProgrameProjetDto MapReaderToDto(DbDataReader reader)
        {
            var dto = new LivrablesProgrameProjetDto
            {
                // champs hérités de LivrablesDuProjetDto peuvent être mappés ici si présents dans la vue
                IdLivrablesProjet = SafeGetByte(reader, "ID_LIVRABLES_PROJET") ?? 0,
                IdIdentificationProjet = SafeGetString(reader, "ID_IDENTIFICATION_PROJET") ?? string.Empty,
                ExerciceFiscalDebut = SafeGetByte(reader, "EXERCICE_FISCAL_DEBUT"),
                ExerciceFiscalFin = SafeGetByte(reader, "EXERCICE_FISCAL_FIN"),
                QuantiteALivrer = SafeGetInt(reader, "QUANTITE_A_LIVRER")
            };

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

        /// <summary>
        /// Récupère tous les livrables pour un projet donné.
        /// </summary>
        public async Task<List<LivrablesProgrameProjetDto>> ObtenirParProjetAsync(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet))
                throw new ArgumentNullException(nameof(idProjet));

            const string sql = @"
        SELECT
            ID_LIVRABLES_PROJET,
            ID_IDENTIFICATION_PROJET,
            ID_ACTIVITE,
            EXERCICE_FISCAL_DEBUT,
            EXERCICE_FISCAL_FIN,
            QUANTITE_A_LIVRER
        FROM O_VIEW_LIVRABLES_PROJET
        WHERE ID_IDENTIFICATION_PROJET = :p_idProjet";

            var parameters = new[]
            {
        new OracleParameter("p_idProjet", OracleDbType.Varchar2)
        {
            Value = idProjet,
            Direction = System.Data.ParameterDirection.Input
        }
    };

            return await ExecuteQueryAndMapAsync(sql, parameters);
        }

    }
}
