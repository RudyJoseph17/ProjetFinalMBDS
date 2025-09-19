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
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();

        // Générer un ID unique pour la grille si nécessaire
        if (dto.IdGrilleDdpProjet == 0)
        {
            await using var seqCmd = conn.CreateCommand();
            seqCmd.CommandText = "SELECT SEQ_GRILLE_DDP.NEXTVAL FROM DUAL";
            dto.IdGrilleDdpProjet = Convert.ToByte(await seqCmd.ExecuteScalarAsync());
        }

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "AJOUTER_GRILLE_DDP_PROJET";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        // Paramètres
        cmd.Parameters.Add(new OracleParameter("p_id_grille", dto.IdGrilleDdpProjet));
        cmd.Parameters.Add(new OracleParameter("p_titre_projet", dto.TitreProjet ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_ministere", dto.Ministere ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_date_soumission", dto.DateSoumission ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_date_debut_analyse", dto.DateDebutAnalyse ?? (object)DBNull.Value));

        // Booléens convertis en NUMBER(1)...
        cmd.Parameters.Add(new OracleParameter("p_titre_projet_AOL", dto.TitreProjetAol.HasValue ? (dto.TitreProjetAol.Value ? 1 : 0) : (object)DBNull.Value));
        // ... (reste des booléens)
        cmd.Parameters.Add(new OracleParameter("p_passation_des_marches_rigoureux", dto.PassationDesMarchesRigoureux.HasValue ? (dto.PassationDesMarchesRigoureux.Value ? 1 : 0) : (object)DBNull.Value));

        // Autres champs
        cmd.Parameters.Add(new OracleParameter("p_commentaires_generaux", dto.CommentairesGeneraux ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_resultats_analyse", dto.ResultatsAnalyse ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_recommandations", dto.Recommandations ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_decision", dto.Decision ?? (object)DBNull.Value));
        cmd.Parameters.Add(new OracleParameter("p_date_avis", dto.DateAvis ?? (object)DBNull.Value));

        // Lien avec le projet existant
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
            if (string.IsNullOrEmpty(idProjet))
                return null;

            var conn = _db.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        SELECT *
        FROM O_VIEW_GRILLE_DDP_PROJET
        WHERE ID_IDENTIFICATION_PROJET = :idProjet
    ";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add(new OracleParameter("idProjet", idProjet));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                GrilleDdpProjetDto dto = new();

                object GetValueSafe(string columnName)
                {
                    try
                    {
                        int idx = reader.GetOrdinal(columnName);
                        return reader.IsDBNull(idx) ? null : reader.GetValue(idx);
                    }
                    catch
                    {
                        return null;
                    }
                }

                // Clés et dates
                dto.IdGrilleDdpProjet = GetValueSafe("ID_GRILLE_DDP_PROJET_PROJET") != null
                    ? Convert.ToByte(GetValueSafe("ID_GRILLE_DDP_PROJET_PROJET"))
                    : (byte)0;
                dto.IdIdentificationProjet = GetValueSafe("ID_IDENTIFICATION_PROJET")?.ToString();
                dto.Decision = GetValueSafe("DECISION")?.ToString();
                dto.DateAvis = GetValueSafe("DATE_AVIS") as DateTime?;

                // Infos générales
                dto.TitreProjet = GetValueSafe("TITRE_PROJET")?.ToString();
                dto.Ministere = GetValueSafe("MINISTERE")?.ToString();
                dto.DateSoumission = GetValueSafe("DATE_SOUMISSION") as DateTime?;
                dto.DateDebutAnalyse = GetValueSafe("DATE_DEBUT_ANALYSE") as DateTime?;

                // Booléens (NUMBER(1) dans Oracle) → convertis en bool?
                dto.TitreProjetAol = GetValueSafe("TITRE_PROJET_AOL") != null ? Convert.ToBoolean(GetValueSafe("TITRE_PROJET_AOL")) : null;
                dto.ProjetLienPsdh = GetValueSafe("PROJET_LIEN_PSDH") != null ? Convert.ToBoolean(GetValueSafe("PROJET_LIEN_PSDH")) : null;
                dto.HistoriqueDecrit = GetValueSafe("HISTORIQUE_DECRIT") != null ? Convert.ToBoolean(GetValueSafe("HISTORIQUE_DECRIT")) : null;
                dto.JustificationDemontree = GetValueSafe("JUSTIFICATION_DEMONTREE") != null ? Convert.ToBoolean(GetValueSafe("JUSTIFICATION_DEMONTREE")) : null;
                dto.ProjetObjectifClair = GetValueSafe("PROJET_OBJECTIF_CLAIR") != null ? Convert.ToBoolean(GetValueSafe("PROJET_OBJECTIF_CLAIR")) : null;
                dto.EffetsAttendusCoherents = GetValueSafe("EFFETS_ATTENDUS_COHERENTS") != null ? Convert.ToBoolean(GetValueSafe("EFFETS_ATTENDUS_COHERENTS")) : null;
                dto.PopulationViseeDecrite = GetValueSafe("POPULATION_VISEE_DECRITE") != null ? Convert.ToBoolean(GetValueSafe("POPULATION_VISEE_DECRITE")) : null;
                dto.LocalisationDecrite = GetValueSafe("LOCALISATION_DECRITE") != null ? Convert.ToBoolean(GetValueSafe("LOCALISATION_DECRITE")) : null;
                dto.DureeTotalProjetBienDefine = GetValueSafe("DUREE_TOTAL_PROJET_BIEN_DEFINE") != null ? Convert.ToBoolean(GetValueSafe("DUREE_TOTAL_PROJET_BIEN_DEFINE")) : null;
                dto.CoutTotalProjetBienDetermine = GetValueSafe("COUT_TOTAL_PROJET_BIEN_DETERMINE") != null ? Convert.ToBoolean(GetValueSafe("COUT_TOTAL_PROJET_BIEN_DETERMINE")) : null;
                dto.EmploisCreesIdentifies = GetValueSafe("EMPLOIS_CREES_IDENTIFIES") != null ? Convert.ToBoolean(GetValueSafe("EMPLOIS_CREES_IDENTIFIES")) : null;
                dto.FacteurGenrePrisEnCompte = GetValueSafe("FACTEUR_GENRE_PRIS_EN_COMPTE") != null ? Convert.ToBoolean(GetValueSafe("FACTEUR_GENRE_PRIS_EN_COMPTE")) : null;
                dto.EtudesSatisfaisantes = GetValueSafe("ETUDES_SATISFAISANTES") != null ? Convert.ToBoolean(GetValueSafe("ETUDES_SATISFAISANTES")) : null;
                dto.ActivitesEtResultatsDecrits = GetValueSafe("ACTIVITES_ET_RESULTATS_DECRITS") != null ? Convert.ToBoolean(GetValueSafe("ACTIVITES_ET_RESULTATS_DECRITS")) : null;
                dto.DureeActiviteDansGantt = GetValueSafe("DUREE_ACTIVITE_DANS_GANTT") != null ? Convert.ToBoolean(GetValueSafe("DUREE_ACTIVITE_DANS_GANTT")) : null;
                dto.CalendrierFinancierCorrespondGantt = GetValueSafe("CALENDRIER_FINANCIER_CORRESPOND_GANTT") != null ? Convert.ToBoolean(GetValueSafe("CALENDRIER_FINANCIER_CORRESPOND_GANTT")) : null;
                dto.CalculsDepensesExacts = GetValueSafe("CALCULS_DEPENSES_EXACTS") != null ? Convert.ToBoolean(GetValueSafe("CALCULS_DEPENSES_EXACTS")) : null;
                dto.DepensesPrevuesPermetActivites = GetValueSafe("DEPENSES_PREVUES_PERMET_ACTIVITES") != null ? Convert.ToBoolean(GetValueSafe("DEPENSES_PREVUES_PERMET_ACTIVITES")) : null;
                dto.DepensesProjetIncluses = GetValueSafe("DEPENSES_PROJET_INCLUSES") != null ? Convert.ToBoolean(GetValueSafe("DEPENSES_PROJET_INCLUSES")) : null;
                dto.SourcesFinancementIdentifiees = GetValueSafe("SOURCES_FINANCEMENT_IDENTIFIEES") != null ? Convert.ToBoolean(GetValueSafe("SOURCES_FINANCEMENT_IDENTIFIEES")) : null;
                dto.EntitesRolesClairementDefinis = GetValueSafe("ENTITES_ROLES_CLAIREMENT_DEFINIS") != null ? Convert.ToBoolean(GetValueSafe("ENTITES_ROLES_CLAIREMENT_DEFINIS")) : null;
                dto.StructureOrgaInclutEntites = GetValueSafe("STRUCTURE_ORGA_INCLUT_ENTITES") != null ? Convert.ToBoolean(GetValueSafe("STRUCTURE_ORGA_INCLUT_ENTITES")) : null;
                dto.ObjectifGeneralSpecifiqueDefinis = GetValueSafe("OBJECTIF_GENERAL_SPECIFIQUE_DEFINIS") != null ? Convert.ToBoolean(GetValueSafe("OBJECTIF_GENERAL_SPECIFIQUE_DEFINIS")) : null;
                dto.DetailsSuffisantsAspJuridiques = GetValueSafe("DETAILS_SUFFISANTS_ASP_JURIDIQUES") != null ? Convert.ToBoolean(GetValueSafe("DETAILS_SUFFISANTS_ASP_JURIDIQUES")) : null;
                dto.PassationDesMarchesRigoureux = GetValueSafe("PASSATION_DES_MARCHES_RIGOUREUX") != null ? Convert.ToBoolean(GetValueSafe("PASSATION_DES_MARCHES_RIGOUREUX")) : null;

                // Textes
                dto.CommentairesGeneraux = GetValueSafe("COMMENTAIRES_GENERAUX")?.ToString();
                dto.ResultatsAnalyse = GetValueSafe("RESULTATS_ANALYSE")?.ToString();
                dto.Recommandations = GetValueSafe("RECOMMANDATIONS")?.ToString();

                return dto;
            }

            return null;
        }



        //public Task<List<GrilleDdpProjetDto>> ObtenirParIdAsync(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
