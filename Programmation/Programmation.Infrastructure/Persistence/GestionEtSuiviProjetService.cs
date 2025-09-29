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
    public class GestionEtSuiviProjetService : IGestionEtSuiviService
    {
        private readonly ProgrammationDbContext _db;
        private readonly ILogger<GestionEtSuiviProjetService> _logger;

        public GestionEtSuiviProjetService(
            ProgrammationDbContext db,
            ILogger<GestionEtSuiviProjetService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AjouterAsync(GestionEtSuiviProjetDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] AjouterAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            // 1) Générer l’ID si nécessaire
            if (dto.IdGestionDeProjetEtSuivi == 0)
            {
                await using var seqCmd = new OracleCommand(
                    "SELECT SEQ_GESTION_SUIVI_PROJET.NEXTVAL FROM DUAL",
                    (OracleConnection)conn);
                dto.IdGestionDeProjetEtSuivi = Convert.ToInt32(await seqCmd.ExecuteScalarAsync());
                _logger.LogInformation("🔢 Nouveau IdGestionDeProjetEtSuivi = {Id}", dto.IdGestionDeProjetEtSuivi);
            }

            // 2) Préparer la commande Oracle
            await using var cmd = new OracleCommand(
                "AJOUTER_GESTION_SUIVI_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_gest", dto.IdGestionDeProjetEtSuivi));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscaleDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscaleFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_desc_role", dto.DescriptionRole ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_cadre_ref", dto.CadreDeReferenceContrat ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_methodes", dto.DescriptionDesMethodes ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_agents_impl", dto.IdentificationDesAgentsImpliques ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            // 3) Log des paramètres
            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] AJOUTER_GESTION_SUIVI_JSON params: {Params}", paramList);

            // 4) Exécution
            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Ajout terminé pour Id={Id}", dto.IdGestionDeProjetEtSuivi);
        }

        public async Task MettreAJourAsync(GestionEtSuiviProjetDto dto)
        {
            _logger.LogInformation("🔄 [SERVICE] MettreAJourAsync reçu DTO : {@Dto}", dto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "MODIFIER_GESTION_SUIVI_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new OracleParameter("p_id_gest", dto.IdGestionDeProjetEtSuivi));
            cmd.Parameters.Add(new OracleParameter("p_exo_deb", dto.ExerciceFiscaleDebut ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_exo_fin", dto.ExerciceFiscaleFin ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_desc_role", dto.DescriptionRole ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_cadre_ref", dto.CadreDeReferenceContrat ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_methodes", dto.DescriptionDesMethodes ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_agents_impl", dto.IdentificationDesAgentsImpliques ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("p_id_proj", dto.IdIdentificationProjet));

            var paramList = string.Join(", ",
                cmd.Parameters.Cast<OracleParameter>()
                              .Select(p => $"{p.ParameterName}={(p.Value ?? "NULL")}"));
            _logger.LogInformation("📤 [SERVICE] MODIFIER_GESTION_SUIVI_JSON params: {Params}", paramList);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Update terminé pour Id={Id}", dto.IdGestionDeProjetEtSuivi);
        }

        public async Task SupprimerAsync(int idGestionEtSuiviProjetDto)
        {
            _logger.LogInformation("🗑️ [SERVICE] SupprimerAsync reçu Id={Id}", idGestionEtSuiviProjetDto);

            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = new OracleCommand(
                "SUPPRIMER_GESTION_SUIVI_JSON",
                (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true,
                CommandTimeout = 120
            };
            cmd.Parameters.Add(new OracleParameter("p_id_gest", idGestionEtSuiviProjetDto));

            _logger.LogInformation("📤 [SERVICE] SUPPRIMER_GESTION_SUIVI_JSON params: p_id_gest={Id}", idGestionEtSuiviProjetDto);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("✔️ [SERVICE] Suppression terminée pour Id={Id}", idGestionEtSuiviProjetDto);
        }

        public async Task<List<GestionEtSuiviProjetDto>> ObtenirTousAsync()
        {
            var entities = await _db.OViewGestionEtSuivis
                                     .AsNoTracking()
                                     .ToListAsync();

            return entities.Select(e => new GestionEtSuiviProjetDto
            {
                IdGestionDeProjetEtSuivi = e.IdGestionDeProjetEtSuivi,
                ExerciceFiscaleDebut = e.ExerciceFiscaleDebut,
                ExerciceFiscaleFin = e.ExerciceFiscaleFin,
                DescriptionRole = e.DescriptionRole,
                CadreDeReferenceContrat = e.CadreDeReferenceContrat,
                DescriptionDesMethodes = e.DescriptionDesMethodes,
                IdentificationDesAgentsImpliques = e.IdentificationDesAgentsImpliques,
                IdIdentificationProjet = e.IdIdentificationProjet
            }).ToList();
        }

        public async Task<GestionEtSuiviProjetDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet)
        {
            if (string.IsNullOrWhiteSpace(idIdentificationProjet))
                return null;

            var e = await _db.OViewGestionEtSuivis
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.IdIdentificationProjet == idIdentificationProjet);

            if (e == null) return null;

            return new GestionEtSuiviProjetDto
            {
                IdGestionDeProjetEtSuivi = e.IdGestionDeProjetEtSuivi,
                ExerciceFiscaleDebut = e.ExerciceFiscaleDebut,
                ExerciceFiscaleFin = e.ExerciceFiscaleFin,
                DescriptionRole = e.DescriptionRole,
                CadreDeReferenceContrat = e.CadreDeReferenceContrat,
                DescriptionDesMethodes = e.DescriptionDesMethodes,
                IdentificationDesAgentsImpliques = e.IdentificationDesAgentsImpliques,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        public async Task<byte> GetNextIdAsync()
        {
            await using var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var seqCmd = new OracleCommand(
                "SELECT SEQ_GESTION_SUIVI_PROJET.NEXTVAL FROM DUAL",
                (OracleConnection)conn);
            return Convert.ToByte(await seqCmd.ExecuteScalarAsync());
        }

        // Méthode inutilisée par l'interface
        public Task GetNextIdAsync(int IdGestionEtSuiviProjetDto) =>
            throw new NotImplementedException();
    }
}
