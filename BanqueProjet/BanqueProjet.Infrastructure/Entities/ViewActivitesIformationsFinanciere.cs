using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewActivitesIformationsFinanciere
{
    [Column("ID_ACTIVITES")]
    [Precision(6)]
    public int IdActivites { get; set; }

    [Column("ID_INFORMATIONS_FINANCIERES")]
    [Precision(6)]
    public int? IdInformationsFinancieres { get; set; }

    [Column("EXERCICE_FISCAL_DEBUT")]
    [Precision(4)]
    public byte? ExerciceFiscalDebut { get; set; }

    [Column("EXERCICE_FISCAL_FIN")]
    [Precision(4)]
    public byte? ExerciceFiscalFin { get; set; }

    [Column("SOURCES_FINANCEMENT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? SourcesFinancement { get; set; }

    [Column("MONTANT_PREVU", TypeName = "NUMBER(12,2)")]
    public decimal? MontantPrevu { get; set; }

    [Column("MONTANT_AUTORISATION", TypeName = "NUMBER(12,2)")]
    public decimal? MontantAutorisation { get; set; }

    [Column("MONTANT_DECAISSEMENT", TypeName = "NUMBER(12,2)")]
    public decimal? MontantDecaissement { get; set; }

    [Column("MONTANT_DEPENSE", TypeName = "NUMBER(12,2)")]
    public decimal? MontantDepense { get; set; }
}
