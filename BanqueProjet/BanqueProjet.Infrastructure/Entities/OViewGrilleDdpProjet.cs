using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewGrilleDdpProjet
{
    [Column("ID_GRILLE_DDP_PROJET_PROJET")]
    [Precision(3)]
    public byte IdGrilleDdpProjetProjet { get; set; }

    [Column("TITRE_PROJET")]
    [StringLength(200)]
    [Unicode(false)]
    public string? TitreProjet { get; set; }

    [Column("MINISTERE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Ministere { get; set; }

    [Column("DATE_SOUMISSION", TypeName = "DATE")]
    public DateTime? DateSoumission { get; set; }

    [Column("DATE_DEBUT_ANALYSE", TypeName = "DATE")]
    public DateTime? DateDebutAnalyse { get; set; }

    [Column("TITRE_PROJET_AOL", TypeName = "NUMBER(1)")]
    public bool? TitreProjetAol { get; set; }

    [Column("PROJET_LIEN_PSDH", TypeName = "NUMBER(1)")]
    public bool? ProjetLienPsdh { get; set; }

    [Column("HISTORIQUE_DECRIT", TypeName = "NUMBER(1)")]
    public bool? HistoriqueDecrit { get; set; }

    [Column("JUSTIFICATION_DEMONTREE", TypeName = "NUMBER(1)")]
    public bool? JustificationDemontree { get; set; }

    [Column("PROJET_OBJECTIF_CLAIR", TypeName = "NUMBER(1)")]
    public bool? ProjetObjectifClair { get; set; }

    [Column("EFFETS_ATTENDUS_COHERENTS", TypeName = "NUMBER(1)")]
    public bool? EffetsAttendusCoherents { get; set; }

    [Column("POPULATION_VISEE_DECRITE", TypeName = "NUMBER(1)")]
    public bool? PopulationViseeDecrite { get; set; }

    [Column("LOCALISATION_DECRITE", TypeName = "NUMBER(1)")]
    public bool? LocalisationDecrite { get; set; }

    [Column("DUREE_TOTAL_PROJET_BIEN_DEFINE", TypeName = "NUMBER(1)")]
    public bool? DureeTotalProjetBienDefine { get; set; }

    [Column("COUT_TOTAL_PROJET_BIEN_DETERMINE", TypeName = "NUMBER(1)")]
    public bool? CoutTotalProjetBienDetermine { get; set; }

    [Column("EMPLOIS_CREES_IDENTIFIES", TypeName = "NUMBER(1)")]
    public bool? EmploisCreesIdentifies { get; set; }

    [Column("FACTEUR_GENRE_PRIS_EN_COMPTE", TypeName = "NUMBER(1)")]
    public bool? FacteurGenrePrisEnCompte { get; set; }

    [Column("ETUDES_SATISFAISANTES", TypeName = "NUMBER(1)")]
    public bool? EtudesSatisfaisantes { get; set; }

    [Column("ACTIVITES_ET_RESULTATS_DECRITS", TypeName = "NUMBER(1)")]
    public bool? ActivitesEtResultatsDecrits { get; set; }

    [Column("DUREE_ACTIVITE_DANS_GANTT", TypeName = "NUMBER(1)")]
    public bool? DureeActiviteDansGantt { get; set; }

    [Column("CALENDRIER_FINANCIER_CORRESPOND_GANTT", TypeName = "NUMBER(1)")]
    public bool? CalendrierFinancierCorrespondGantt { get; set; }

    [Column("CALCULS_DEPENSES_EXACTS", TypeName = "NUMBER(1)")]
    public bool? CalculsDepensesExacts { get; set; }

    [Column("DEPENSES_PREVUES_PERMET_ACTIVITES", TypeName = "NUMBER(1)")]
    public bool? DepensesPrevuesPermetActivites { get; set; }

    [Column("DEPENSES_PROJET_INCLUSES", TypeName = "NUMBER(1)")]
    public bool? DepensesProjetIncluses { get; set; }

    [Column("SOURCES_FINANCEMENT_IDENTIFIEES", TypeName = "NUMBER(1)")]
    public bool? SourcesFinancementIdentifiees { get; set; }

    [Column("ENTITES_ROLES_CLAIREMENT_DEFINIS", TypeName = "NUMBER(1)")]
    public bool? EntitesRolesClairementDefinis { get; set; }

    [Column("STRUCTURE_ORGA_INCLUT_ENTITES", TypeName = "NUMBER(1)")]
    public bool? StructureOrgaInclutEntites { get; set; }

    [Column("OBJECTIF_GENERAL_SPECIFIQUE_DEFINIS", TypeName = "NUMBER(1)")]
    public bool? ObjectifGeneralSpecifiqueDefinis { get; set; }

    [Column("DETAILS_SUFFISANTS_ASP_JURIDIQUES", TypeName = "NUMBER(1)")]
    public bool? DetailsSuffisantsAspJuridiques { get; set; }

    [Column("PASSATION_DES_MARCHES_RIGOUREUX", TypeName = "NUMBER(1)")]
    public bool? PassationDesMarchesRigoureux { get; set; }

    [Column("COMMENTAIRES_GENERAUX")]
    [StringLength(500)]
    [Unicode(false)]
    public string? CommentairesGeneraux { get; set; }

    [Column("RESULTATS_ANALYSE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ResultatsAnalyse { get; set; }

    [Column("RECOMMANDATIONS")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Recommandations { get; set; }

    [Column("DECISION")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Decision { get; set; }

    [Column("DATE_AVIS", TypeName = "DATE")]
    public DateTime? DateAvis { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
