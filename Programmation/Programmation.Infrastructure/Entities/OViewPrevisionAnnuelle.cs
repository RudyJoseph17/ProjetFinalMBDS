using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Entities;

[Keyless]
public partial class OViewPrevisionAnnuelle
{
    [Column("ID_PREVISIONANNUELLE")]
    [Precision(3)]
    public byte IdPrevisionannuelle { get; set; }

    [Column("EXERCICE_FISCALE_PREVISION")]
    [Precision(12)]
    public long? ExerciceFiscalePrevision { get; set; }

    [Column("SOURCES_FINANCEMENT_ANNUEL_ACT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? SourcesFinancementAnnuelAct { get; set; }

    [Column("ARTICLE_PREVISION_ANNUELLE")]
    [StringLength(30)]
    [Unicode(false)]
    public string? ArticlePrevisionAnnuelle { get; set; }

    [Column("ALINEA_PREVISION_ANNUELLE")]
    [StringLength(30)]
    [Unicode(false)]
    public string? AlineaPrevisionAnnuelle { get; set; }

    [Column("MONTANT_ANNUEL_PREVU_SUR_ALINE", TypeName = "NUMBER(12,2)")]
    public decimal? MontantAnnuelPrevuSurAline { get; set; }

    [Column("ID_ACTIVITES")]
    [Precision(6)]
    public int IdActivites { get; set; }
}
