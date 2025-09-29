using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using Programmation.Infrastructure.Data;
using Programmation.Infrastructure.Entities;

namespace Programmation.Infrastructure.Persistence
{
    public class HypothesesEtRisquesService : IHypothesesEtRisquesService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<HypothesesEtRisquesService> _logger;

        public HypothesesEtRisquesService(
            ProgrammationDbContext db,
            ILogger<HypothesesEtRisquesService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AjouterAsync(HypothesesEtRisquesDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] AjouterAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            // 1) Génération de l'ID
            if (dto.IdHypothesesEtRisques == 0)
            {
                await using var seqCmd = new OracleCommand(
                    "SELECT SEQ_HYPOTHESES_ET_RISQUES.NEXTVAL FROM DUAL",
                    (OracleConnection)conn);
                dto.IdHypothesesEtRisques = Convert.ToInt32(await seqCmd.ExecuteScalarAsync());
                _logger.LogInformation("🔢 Nouveau IdHypothesesEtRisques = {Id}", dto.IdHypothesesEtRisques);
            }

            // 2) Préparation de la commande Oracle
            await using var cmd = new OracleCommand(
                "AJOUTER_HYPOTHESES_ET_RISQUES_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_hypo", dto.IdHypothesesEtRisques));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscaleDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscaleFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_desc", dto.DescriptionDesConditionsPouvantImpacterLeProjet ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_strategies", dto.StrategiesPourAssurerUnImpactSurLeProjet ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            // 3) Log des paramètres
            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] AJOUTER_HYPOTHESES_ET_RISQUES_JSON params: {Params}", paramList);

            // 4) Exécution
            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Ajout terminé pour Id={Id}", dto.IdHypothesesEtRisques);
        }

        public async Task MettreAJourAsync(HypothesesEtRisquesDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] MettreAJourAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "MODIFIER_HYPOTHESES_ET_RISQUES_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_hypo", dto.IdHypothesesEtRisques));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscaleDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscaleFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_desc", dto.DescriptionDesConditionsPouvantImpacterLeProjet ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_strategies", dto.StrategiesPourAssurerUnImpactSurLeProjet ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] MODIFIER_HYPOTHESES_ET_RISQUES_JSON params: {Params}", paramList);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Update terminé pour Id={Id}", dto.IdHypothesesEtRisques);
        }

        public async Task SupprimerAsync(int idHypothesesEtRisques)
        {
            _logger.LogInformation("🗑️ [SERVICE] SupprimerAsync reçu Id={Id}", idHypothesesEtRisques);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "SUPPRIMER_HYPOTHESES_ET_RISQUES_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };
            cmd.Parameters.Add(new OracleParameter("p_id_hypo", idHypothesesEtRisques));

            _logger.LogInformation("📤 [SERVICE] SUPPRIMER_HYPOTHESES_ET_RISQUES_JSON params: p_id_hypo={Id}", idHypothesesEtRisques);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Suppression terminée pour Id={Id}", idHypothesesEtRisques);
        }

        public async Task<List<HypothesesEtRisquesDto>> ObtenirTousAsync()
        {
            var entities = await _db.OViewHypothesesEtRisques
                                     .AsNoTracking()
                                     .ToListAsync();

            return entities.Select(e => new HypothesesEtRisquesDto
            {
                IdHypothesesEtRisques = e.IdHypothesesEtRisques,
                ExerciceFiscaleDebut = e.ExerciceFiscaleDebut,
                ExerciceFiscaleFin = e.ExerciceFiscaleFin,
                DescriptionDesConditionsPouvantImpacterLeProjet = e.DescriptionDesConditionsPouvantImpacterLeProjet,
                StrategiesPourAssurerUnImpactSurLeProjet = e.StrategiesPourAssurerUnImpactSurLeProjet,
                IdIdentificationProjet = e.IdIdentificationProjet
            }).ToList();
        }

        public async Task<HypothesesEtRisquesDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet)
        {
            if (string.IsNullOrWhiteSpace(idIdentificationProjet))
                return null;

            var e = await _db.OViewHypothesesEtRisques
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.IdIdentificationProjet == idIdentificationProjet);

            if (e == null) return null;

            return new HypothesesEtRisquesDto
            {
                IdHypothesesEtRisques = e.IdHypothesesEtRisques,
                ExerciceFiscaleDebut = e.ExerciceFiscaleDebut,
                ExerciceFiscaleFin = e.ExerciceFiscaleFin,
                DescriptionDesConditionsPouvantImpacterLeProjet = e.DescriptionDesConditionsPouvantImpacterLeProjet,
                StrategiesPourAssurerUnImpactSurLeProjet = e.StrategiesPourAssurerUnImpactSurLeProjet,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        public async Task<byte> GetNextIdAsync()
        {
            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var seqCmd = new OracleCommand(
                "SELECT SEQ_HYPOTHESES_ET_RISQUES.NEXTVAL FROM DUAL",
                (OracleConnection)conn);
            return Convert.ToByte(await seqCmd.ExecuteScalarAsync());
        }

        // Méthode inutilisée par l'interface
        public Task GetNextIdAsync(int IdHypothesesEtRisques) => throw new NotImplementedException();
    }
}
