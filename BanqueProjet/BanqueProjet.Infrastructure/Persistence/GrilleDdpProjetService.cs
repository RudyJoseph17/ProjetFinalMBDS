using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class GrilleDdpProjetService : IGrilleDdpProjetService
    {
        private readonly BanquePDbContext _db;

        public GrilleDdpProjetService(BanquePDbContext db)
        {
            _db = db;
        }

        public async Task AjouterAsync(GrilleDdpProjetDto dto)
        {
            var conn = _db.Database.GetDbConnection();

            await using (conn)
            {
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "AJOUTER_GRILLE_DDP_PROJET";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Paramètres
                cmd.Parameters.Add(new OracleParameter("p_id_grille", dto.IdGrilleDdpProjet));
                cmd.Parameters.Add(new OracleParameter("p_titre_projet", dto.TitreProjet ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_ministere", dto.Ministere ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_date_soumission", dto.DateSoumission ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_date_debut_analyse", dto.DateDebutAnalyse ?? (object)DBNull.Value));

                // Les booléens convertis en NUMBER(1)
                cmd.Parameters.Add(new OracleParameter("p_titre_projet_AOL", dto.TitreProjetAol.HasValue ? (dto.TitreProjetAol.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_projet_lien_PSDH", dto.ProjetLienPsdh.HasValue ? (dto.ProjetLienPsdh.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_historique_decrit", dto.HistoriqueDecrit.HasValue ? (dto.HistoriqueDecrit.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_justification_demontree", dto.JustificationDemontree.HasValue ? (dto.JustificationDemontree.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_projet_objectif_clair", dto.ProjetObjectifClair.HasValue ? (dto.ProjetObjectifClair.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_effets_attendus_coherents", dto.EffetsAttendusCoherents.HasValue ? (dto.EffetsAttendusCoherents.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_population_visee_decrite", dto.PopulationViseeDecrite.HasValue ? (dto.PopulationViseeDecrite.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_localisation_decrite", dto.LocalisationDecrite.HasValue ? (dto.LocalisationDecrite.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_duree_total_projet_bien_define", dto.DureeTotalProjetBienDefine.HasValue ? (dto.DureeTotalProjetBienDefine.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_cout_total_projet_bien_determine", dto.CoutTotalProjetBienDetermine.HasValue ? (dto.CoutTotalProjetBienDetermine.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_emplois_crees_identifies", dto.EmploisCreesIdentifies.HasValue ? (dto.EmploisCreesIdentifies.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_facteur_genre_pris_en_compte", dto.FacteurGenrePrisEnCompte.HasValue ? (dto.FacteurGenrePrisEnCompte.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_etudes_satisfaisantes", dto.EtudesSatisfaisantes.HasValue ? (dto.EtudesSatisfaisantes.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_activites_et_resultats_decrits", dto.ActivitesEtResultatsDecrits.HasValue ? (dto.ActivitesEtResultatsDecrits.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_duree_activite_dans_gantt", dto.DureeActiviteDansGantt.HasValue ? (dto.DureeActiviteDansGantt.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_calendrier_financier_correspond_gantt", dto.CalendrierFinancierCorrespondGantt.HasValue ? (dto.CalendrierFinancierCorrespondGantt.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_calculs_depenses_exacts", dto.CalculsDepensesExacts.HasValue ? (dto.CalculsDepensesExacts.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_depenses_prevues_permet_activites", dto.DepensesPrevuesPermetActivites.HasValue ? (dto.DepensesPrevuesPermetActivites.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_depenses_projet_incluses", dto.DepensesProjetIncluses.HasValue ? (dto.DepensesProjetIncluses.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_sources_financement_identifiees", dto.SourcesFinancementIdentifiees.HasValue ? (dto.SourcesFinancementIdentifiees.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_entites_roles_clairement_definis", dto.EntitesRolesClairementDefinis.HasValue ? (dto.EntitesRolesClairementDefinis.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_structure_orga_inclut_entites", dto.StructureOrgaInclutEntites.HasValue ? (dto.StructureOrgaInclutEntites.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_objectif_general_specifique_definis", dto.ObjectifGeneralSpecifiqueDefinis.HasValue ? (dto.ObjectifGeneralSpecifiqueDefinis.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_details_suffisants_asp_juridiques", dto.DetailsSuffisantsAspJuridiques.HasValue ? (dto.DetailsSuffisantsAspJuridiques.Value ? 1 : 0) : (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_passation_des_marches_rigoureux", dto.PassationDesMarchesRigoureux.HasValue ? (dto.PassationDesMarchesRigoureux.Value ? 1 : 0) : (object)DBNull.Value));

                // Autres champs
                cmd.Parameters.Add(new OracleParameter("p_commentaires_generaux", dto.CommentairesGeneraux ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_resultats_analyse", dto.ResultatsAnalyse ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_recommandations", dto.Recommandations ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_decision", dto.Decision ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_date_avis", dto.DateAvis ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("p_id_identification_projet", dto.IdIdentificationProjet));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task MettreAJourAsync(GrilleDdpProjetDto grilleDdpProjetDto)
        {
            // Même principe que AjouterAsync, mais avec la procédure de mise à jour si elle existe
            throw new NotImplementedException();
        }

        public async Task SupprimerAsync(byte IdAspectsJuridiques)
        {
            // Ici appeler votre procédure Oracle de suppression
            throw new NotImplementedException();
        }

        public async Task<List<GrilleDdpProjetDto>> ObtenirTousAsync()
        {
            // Ici récupérer via view ou table, mapper vers DTO
            throw new NotImplementedException();
        }

        public async Task<GrilleDdpProjetDto?> ObtenirParIdAsync(byte id)
        {
            // Ici récupérer via view ou table, mapper vers DTO
            throw new NotImplementedException();
        }

        public async Task<GrilleDdpProjetDto?> ObtenirParProjetIdAsync(string idProjet)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM O_VIEW_GRILLE_DDP_PROJET WHERE ID_IDENTIFICATION_PROJET = :idProjet";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add(new OracleParameter("idProjet", idProjet));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new GrilleDdpProjetDto
                {
                    IdGrilleDdpProjet = Convert.ToByte(reader["ID_GRILLE_DDP_PROJET"]),
                    IdIdentificationProjet = reader["ID_IDENTIFICATION_PROJET"]?.ToString(),
                    Decision = reader["DECISION"]?.ToString(),
                    DateAvis = reader["DATE_AVIS"] as DateTime?
                    // autres mappings si besoin
                };
            }

            return null;
        }

    }
}
