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
    public class PrevisionActiviteAnnuelleService : IPrevisionActiviteAnnuelleService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<PrevisionActiviteAnnuelleService> _logger;

        public PrevisionActiviteAnnuelleService(
            ProgrammationDbContext db,
            ILogger<PrevisionActiviteAnnuelleService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AjouterAsync(PrevisionActiviteAnnuelleDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] AjouterAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            // 1) Générer l’ID si nécessaire
            if (dto.IdActivitesAnnuelles == 0)
            {
                await using var seqCmd = new OracleCommand(
                    "SELECT SEQ_PREVISION_ACTIVITE_ANN.NEXTVAL FROM DUAL",
                    (OracleConnection)conn);
                dto.IdActivitesAnnuelles = Convert.ToInt32(await seqCmd.ExecuteScalarAsync());
                _logger.LogInformation("🔢 Nouveau IdActivitesAnnuelles = {Id}", dto.IdActivitesAnnuelles);
            }

            // 2) Préparer la commande Oracle
            await using var cmd = new OracleCommand(
                "AJOUTER_PREVISION_ACTIVITE_ANNUELLE_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_act", dto.IdActivitesAnnuelles));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscalDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscalFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_date_debut", dto.DateDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_date_fin", dto.DateFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_cout_annuel", dto.CoutAnnuelProgramme ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            // 3) Log des paramètres
            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] AJOUTER_PREVISION_ACTIVITE_ANNUELLE_JSON params: {Params}", paramList);

            // 4) Exécution
            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Ajout terminé pour Id={Id}", dto.IdActivitesAnnuelles);
        }

        public async Task MettreAJourAsync(PrevisionActiviteAnnuelleDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] MettreAJourAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "MODIFIER_PREVISION_ACTIVITE_ANNUELLE_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_act", dto.IdActivitesAnnuelles));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscalDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscalFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_date_debut", dto.DateDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_date_fin", dto.DateFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_cout_annuel", dto.CoutAnnuelProgramme ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] MODIFIER_PREVISION_ACTIVITE_ANNUELLE_JSON params: {Params}", paramList);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Update terminé pour Id={Id}", dto.IdActivitesAnnuelles);
        }

        public async Task SupprimerAsync(int IdActivitesAnnuelles)
        {
            _logger.LogInformation("🗑️ [SERVICE] SupprimerAsync reçu Id={Id}", IdActivitesAnnuelles);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "SUPPRIMER_PREVISION_ACTIVITE_ANNUELLE_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_act", IdActivitesAnnuelles));

            _logger.LogInformation("📤 [SERVICE] SUPPRIMER_PREVISION_ACTIVITE_ANNUELLE_JSON params: p_id_act={Id}", IdActivitesAnnuelles);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Suppression terminée pour Id={Id}", IdActivitesAnnuelles);
        }

        public async Task<List<PrevisionActiviteAnnuelleDto>> ObtenirTousAsync()
        {
            var entities = await _db.OViewPrevisionActivitesAnnuelles
                                     .AsNoTracking()
                                     .ToListAsync();

            return entities.Select(e => new PrevisionActiviteAnnuelleDto
            {
                IdActivitesAnnuelles = e.IdActivitesAnnuelles,
                ExerciceFiscalDebut = e.ExerciceFiscalDebut,
                ExerciceFiscalFin = e.ExerciceFiscalFin,
                DateDebut = e.DateDebut,
                DateFin = e.DateFin,
                CoutAnnuelProgramme = e.CoutAnnuelProgramme,
                IdIdentificationProjet = e.IdIdentificationProjet
            }).ToList();
        }

        public async Task<PrevisionActiviteAnnuelleDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet)
        {
            if (string.IsNullOrWhiteSpace(idIdentificationProjet))
                return null;

            var e = await _db.OViewPrevisionActivitesAnnuelles
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.IdIdentificationProjet == idIdentificationProjet);

            if (e == null) return null;

            return new PrevisionActiviteAnnuelleDto
            {
                IdActivitesAnnuelles = e.IdActivitesAnnuelles,
                ExerciceFiscalDebut = e.ExerciceFiscalDebut,
                ExerciceFiscalFin = e.ExerciceFiscalFin,
                DateDebut = e.DateDebut,
                DateFin = e.DateFin,
                CoutAnnuelProgramme = e.CoutAnnuelProgramme,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        public async Task<byte> GetNextIdAsync()
        {
            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var seqCmd = new OracleCommand(
                "SELECT SEQ_PREVISION_ACTIVITE_ANN.NEXTVAL FROM DUAL",
                (OracleConnection)conn);
            return Convert.ToByte(await seqCmd.ExecuteScalarAsync());
        }

        // non utilisé
        public Task GetNextIdAsync(int IdActivitesAnnuelles) =>
            throw new NotImplementedException();
    }
}
