using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewDdpCadreLogique
{
    [Column("ID_DDP_CADRE_LOGIQUE")]
    [Precision(3)]
    public byte IdDdpCadreLogique { get; set; }

    [Column("INTRANTS_RESUME_NARRATIF")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IntrantsResumeNarratif { get; set; }

    [Column("EXTRANTS_RESUME_NARRATIF")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ExtrantsResumeNarratif { get; set; }

    [Column("OBJECTIFS_SPECIFIQUES_RESUME_NARRATIF")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifsSpecifiquesResumeNarratif { get; set; }

    [Column("OBJECTIF_GENERAL_RESUME_NARRATIF")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifGeneralResumeNarratif { get; set; }

    [Column("INTRANTS_IOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IntrantsIov { get; set; }

    [Column("EXTRANTS_IOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ExtrantsIov { get; set; }

    [Column("OBJECTIFS_SPECIFIQUES_IOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifsSpecifiquesIov { get; set; }

    [Column("OBJECTIF_GENERAL_IOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifGeneralIov { get; set; }

    [Column("INTRANTS_SMOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IntrantsSmov { get; set; }

    [Column("EXTRANTS_SMOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ExtrantsSmov { get; set; }

    [Column("OBJECTIFS_SPECIFIQUES_SMOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifsSpecifiquesSmov { get; set; }

    [Column("OBJECTIF_GENERAL_SMOV")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifGeneralSmov { get; set; }

    [Column("INTRANTS_RISQUES_HYPOTHESES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IntrantsRisquesHypotheses { get; set; }

    [Column("EXTRANTS_RISQUES_HYPOTHESES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ExtrantsRisquesHypotheses { get; set; }

    [Column("OBJECTIFS_SPECIFIQUES_RISQUES_HYPOTHESES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifsSpecifiquesRisquesHypotheses { get; set; }

    [Column("OBJECTIF_GENERAL_RISQUES_HYPOTHESES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifGeneralRisquesHypotheses { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
