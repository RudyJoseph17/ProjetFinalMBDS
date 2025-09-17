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
    public class DecaissementSurProjetService : IDecaissementSurProjetService
    {
        private readonly EvaluationDbContext _db;
        public DecaissementSurProjetService(EvaluationDbContext db) => _db = db;

        public async Task AjouterAsync(DecaissementSurProjetDto dto)
        {
            var json = JsonConvert.SerializeObject(dto,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            const string sql = "BEGIN AJOUTER_DECAISSEMENT_SUR_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };
            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        public Task MettreAJourAsync(DecaissementSurProjetDto dto)
            => AjouterAsync(dto);

        public async Task SupprimerAsync(int IdActivites)
        {
            const string sql = "BEGIN SUPPRIMER_DECAISSEMENT_PAR_ACTIVITE(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdActivites };
            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        public async Task SupprimerProjetAsync(string IdIdentificationProjet)
        {
            const string sql = "BEGIN SUPPRIMER_DECAISSEMENTS_SUR_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Varchar2) { Value = IdIdentificationProjet };
            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        public async Task<List<DecaissementSurProjetDto>> ObtenirTousAsync()
        {
            var list = new List<DecaissementSurProjetDto>();
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET, ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         ARTICLE, ALINEA, MOIS_DECAISSEMENT, MONTANT_DECAISSEMENT
                    FROM O_VIEW_DECAISSEMENTS_SUR_PROJET";
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DecaissementSurProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        IdActivites = reader.GetInt32(1),
                        ExerciceFiscalDebut = reader.GetByte(2),
                        ExerciceFiscalFin = reader.GetByte(3),
                        Article = reader.GetString(4),
                        Alinea = reader.GetString(5),
                        MoisDecaissement = reader.GetString(6),
                        MontantDecaissement = reader.GetDecimal(7)
                    });
                }
            }
            return list;
        }

        public async Task<DecaissementSurProjetDto?> ObtenirParIdAsync(string id)
        {
            DecaissementSurProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET, ID_ACTIVITES,
                         EXERCICE_FISCAL_DEBUT, EXERCICE_FISCAL_FIN,
                         ARTICLE, ALINEA, MOIS_DECAISSEMENT, MONTANT_DECAISSEMENT
                    FROM O_VIEW_DECAISSEMENTS_SUR_PROJET
                   WHERE ID_IDENTIFICATION_PROJET = :p_id";
                var param = cmd.CreateParameter();
                param.ParameterName = "p_id";
                param.Value = id;
                cmd.Parameters.Add(param);
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    dto = new DecaissementSurProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        IdActivites = reader.GetInt32(1),
                        ExerciceFiscalDebut = reader.GetByte(2),
                        ExerciceFiscalFin = reader.GetByte(3),
                        Article = reader.GetString(4),
                        Alinea = reader.GetString(5),
                        MoisDecaissement = reader.GetString(6),
                        MontantDecaissement = reader.GetDecimal(7)
                    };
                }
            }
            return dto;
        }

        public Task<DecaissementSurProjetDto?> ObtenirParIdActiviteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
