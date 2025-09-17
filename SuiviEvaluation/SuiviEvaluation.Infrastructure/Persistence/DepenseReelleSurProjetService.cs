// Infrastructure/Persistence/DepenseReelleSurProjetService.cs
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Infrastructure.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SuiviEvaluation.Infrastructure.Persistence
{
    public class DepenseReelleSurProjetService : IDepenseReelleSurProjetService
    {
        private readonly EvaluationDbContext _db;
        public DepenseReelleSurProjetService(EvaluationDbContext db) => _db = db;

        public async Task AjouterAsync(DepenseReelleSurProjetDto dto)
        {
            var json = JsonConvert.SerializeObject(dto,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            const string sql = "BEGIN AJOUTER_DEPENSE_REELLE_SUR_PROJET_JSON(:p_json); END;";
            var p = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };
            await _db.Database.ExecuteSqlRawAsync(sql, p);
        }

        public Task MettreAJourAsync(DepenseReelleSurProjetDto dto)
            => AjouterAsync(dto);

        public async Task SupprimerAsync(int IdActivites)
        {
            const string sql = "BEGIN SUPPRIMER_DEPENSE_REELLE_PAR_ACTIVITE(:p_id); END;";
            var p = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdActivites };
            await _db.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task SupprimerProjetAsync(string IdIdentificationProjet)
        {
            const string sql = "BEGIN SUPPRIMER_DEPENSES_REELLES_SUR_PROJET(:p_id); END;";
            var p = new OracleParameter("p_id", OracleDbType.Varchar2) { Value = IdIdentificationProjet };
            await _db.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task<List<DepenseReelleSurProjetDto>> ObtenirTousAsync()
        {
            var list = new List<DepenseReelleSurProjetDto>();
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET, ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         ARTICLE, ALINEA, MOIS_DEPENSE, MONTANT_DEPENSE
                    FROM O_VIEW_DEPENSES_REELLES_SUR_PROJET";
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DepenseReelleSurProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        IdActivites = reader.GetInt32(1),
                        ExerciceFiscalDebut = reader.IsDBNull(2) ? null : reader.GetByte(2),
                        ExerciceFiscalFin = reader.IsDBNull(3) ? null : reader.GetByte(3),
                        Article = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Alinea = reader.IsDBNull(5) ? null : reader.GetString(5),
                        MoisDepense = reader.IsDBNull(6) ? null : reader.GetString(6),
                        MontantDepense = reader.IsDBNull(7) ? null : reader.GetDecimal(7)
                    });
                }
            }
            return list;
        }

        public async Task<DepenseReelleSurProjetDto?> ObtenirParIdAsync(string id)
        {
            DepenseReelleSurProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET, ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         ARTICLE, ALINEA, MOIS_DEPENSE, MONTANT_DEPENSE
                    FROM O_VIEW_DEPENSES_REELLES_SUR_PROJET
                   WHERE ID_IDENTIFICATION_PROJET = :p_id";
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    dto = new DepenseReelleSurProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        IdActivites = reader.GetInt32(1),
                        ExerciceFiscalDebut = reader.IsDBNull(2) ? null : reader.GetByte(2),
                        ExerciceFiscalFin = reader.IsDBNull(3) ? null : reader.GetByte(3),
                        Article = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Alinea = reader.IsDBNull(5) ? null : reader.GetString(5),
                        MoisDepense = reader.IsDBNull(6) ? null : reader.GetString(6),
                        MontantDepense = reader.IsDBNull(7) ? null : reader.GetDecimal(7)
                    };
                }
            }
            return dto;
        }

        public async Task<DepenseReelleSurProjetDto?> ObtenirParIdActiviteAsync(int id)
        {
            DepenseReelleSurProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET, ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         ARTICLE, ALINEA, MOIS_DEPENSE, MONTANT_DEPENSE
                    FROM O_VIEW_DEPENSES_REELLES_SUR_PROJET
                   WHERE ID_ACTIVITES = :p_id";
                var p = cmd.CreateParameter();
                p.ParameterName = "p_id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    dto = new DepenseReelleSurProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        IdActivites = reader.GetInt32(1),
                        ExerciceFiscalDebut = reader.IsDBNull(2) ? null : reader.GetByte(2),
                        ExerciceFiscalFin = reader.IsDBNull(3) ? null : reader.GetByte(3),
                        Article = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Alinea = reader.IsDBNull(5) ? null : reader.GetString(5),
                        MoisDepense = reader.IsDBNull(6) ? null : reader.GetString(6),
                        MontantDepense = reader.IsDBNull(7) ? null : reader.GetDecimal(7)
                    };
                }
            }
            return dto;
        }
    }
}
