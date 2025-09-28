using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class OViewInformationsFinancieresT
{
    [Column("ID_INFORMATIONS_FINANCIERES")]
    [Precision(6)]
    public int IdInformationsFinancieres { get; set; }

    [Column("EXERCICE_FISCAL_DEBUT")]
    [Precision(4)]
    public byte? ExerciceFiscalDebut { get; set; }

    [Column("EXERCICE_FISCAL_FIN")]
    [Precision(4)]
    public byte? ExerciceFiscalFin { get; set; }

    [Column("SOURCES_FINANCEMENT")]
    [StringLength(50)]
    [Unicode(false)]
    public string? SourcesFinancement { get; set; }

    [Column("ARTICLE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Article { get; set; }

    [Column("ALINEA")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Alinea { get; set; }

    [Column("MONTANT_PREVU", TypeName = "NUMBER(12,2)")]
    public decimal? MontantPrevu { get; set; }

    [Column("MOIS_PREVISION")]
    [StringLength(12)]
    [Unicode(false)]
    public string? MoisPrevision { get; set; }

    [Column("MONTANT_AUTORISATION", TypeName = "NUMBER(12,2)")]
    public decimal? MontantAutorisation { get; set; }

    [Column("MOIS_AUTORISATION")]
    [StringLength(12)]
    [Unicode(false)]
    public string? MoisAutorisation { get; set; }

    [Column("MONTANT_DECAISSEMENT", TypeName = "NUMBER(12,2)")]
    public decimal? MontantDecaissement { get; set; }

    [Column("MOIS_DECAISSEMENT")]
    [StringLength(12)]
    [Unicode(false)]
    public string? MoisDecaissement { get; set; }

    [Column("MONTANT_DEPENSE", TypeName = "NUMBER(12,2)")]
    public decimal? MontantDepense { get; set; }

    [Column("MOIS_DEPENSE")]
    [StringLength(12)]
    [Unicode(false)]
    public string? MoisDepense { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
