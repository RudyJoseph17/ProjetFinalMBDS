using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentificationProjetPlat
{
    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;

    [Column("NOM_PROJET")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NomProjet { get; set; }

    [Column("TYPE_DE_PROJET")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TypeDeProjet { get; set; }

    [Column("SECTEUR_D_ACTIVITES")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SecteurDActivites { get; set; }

    [Column("SOUS_SECTEUR_D_ACTIVITES")]
    [StringLength(50)]
    [Unicode(false)]
    public string? SousSecteurDActivites { get; set; }

    [Column("MINISTERE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Ministere { get; set; }

    [Column("NOM_DIRECTEUR_DE_PROJET")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NomDirecteurDeProjet { get; set; }

    [Column("TELEPHONE_DIRECTEUR_DE_PROJET")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneDirecteurDeProjet { get; set; }

    [Column("COURRIEL_DIRECTEUR_DE_PROJET")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CourrielDirecteurDeProjet { get; set; }

    [Column("SECTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Section { get; set; }

    [Column("CODE_PIP")]
    [StringLength(13)]
    [Unicode(false)]
    public string? CodePip { get; set; }

    [Column("CODE_BAILLEUR")]
    [StringLength(15)]
    [Unicode(false)]
    public string? CodeBailleur { get; set; }

    [Column("JUSTIFICATION_PROJET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? JustificationProjet { get; set; }

    [Column("ETUDE_PREFAISABILITE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? EtudePrefaisabilite { get; set; }

    [Column("ETUDE_FAISABILITE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? EtudeFaisabilite { get; set; }

    [Column("OBJECTIF_GENERAL_PROJET")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ObjectifGeneralProjet { get; set; }

    [Column("DUREE_PROJET")]
    [StringLength(15)]
    [Unicode(false)]
    public string? DureeProjet { get; set; }

    [Column("POPULATION_VISEE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PopulationVisee { get; set; }

    [Column("COUT_TOTAL_PROJET", TypeName = "NUMBER(12,2)")]
    public decimal? CoutTotalProjet { get; set; }

    [Column("ECHELON_TERRITORIAL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? EchelonTerritorial { get; set; }

    [Column("PROGRAMME")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Programme { get; set; }

    [Column("SOUS_PROGRAMME")]
    [StringLength(100)]
    [Unicode(false)]
    public string? SousProgramme { get; set; }

    [Column("DATE_INSCRIPTION", TypeName = "DATE")]
    public DateTime? DateInscription { get; set; }

    [Column("DATE_MISE_A_JOUR", TypeName = "DATE")]
    public DateTime? DateMiseAJour { get; set; }
}
