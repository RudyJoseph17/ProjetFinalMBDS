// Infrastructure/Persistence/EvolutionTemporelleService.cs
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Infrastructure.Data;

namespace SuiviEvaluation.Infrastructure.Persistence
{
    public class EvolutionTemporelleService : IEvolutionTemporelleService
    {
        private readonly EvaluationDbContext _db;

        public EvolutionTemporelleService(EvaluationDbContext db)
        {
            _db = db;
        }

        public async Task AjouterAsync(EvolutionTemporelleDuProjetDto dto)
        {
            var json = JsonConvert.SerializeObject(dto,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                });

            const string sql = "BEGIN AJOUTER_EVOLUTION_TEMPORELLE_PROJET_JSON(:p_json); END;";
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        public Task MettreAJourAsync(EvolutionTemporelleDuProjetDto dto)
            => AjouterAsync(dto);

        public async Task SupprimerAsync(string IdIdentificationProjet)
        {
            const string sql = "BEGIN SUPPRIMER_EVOLUTION_TEMPORELLE_PAR_PROJET(:p_id); END;";
            var param = new OracleParameter("p_id", OracleDbType.Varchar2)
            {
                Value = IdIdentificationProjet
            };

            await _db.Database.ExecuteSqlRawAsync(sql, param);
        }

        public async Task<List<EvolutionTemporelleDuProjetDto>> ObtenirTousAsync()
        {
            var list = new List<EvolutionTemporelleDuProjetDto>();
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET,
                         DATE_DEMARRAGE,
                         DUREE_PROJET
                    FROM O_VIEW_EVOLUTION_TEMPORELLE_PROJET";

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new EvolutionTemporelleDuProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        DateDeDemmarage = reader.GetDateTime(1),
                        DureeProjet = reader.GetInt32(2)
                    });
                }
            }

            return list;
        }

        public async Task<EvolutionTemporelleDuProjetDto?> ObtenirParIdAsync(string id)
        {
            EvolutionTemporelleDuProjetDto? dto = null;
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                  SELECT ID_IDENTIFICATION_PROJET,
                         DATE_DEMARRAGE,
                         DUREE_PROJET
                    FROM O_VIEW_EVOLUTION_TEMPORELLE_PROJET
                   WHERE ID_IDENTIFICATION_PROJET = :p_id";

                var param = cmd.CreateParameter();
                param.ParameterName = "p_id";
                param.Value = id;
                cmd.Parameters.Add(param);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    dto = new EvolutionTemporelleDuProjetDto
                    {
                        IdIdentificationProjet = reader.GetString(0),
                        DateDeDemmarage = reader.GetDateTime(1),
                        DureeProjet = reader.GetInt32(2)
                    };
                }
            }

            return dto;
        }
    }
}
