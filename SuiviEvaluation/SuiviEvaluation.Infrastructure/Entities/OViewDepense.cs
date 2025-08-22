using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Entities;

[Keyless]
public partial class OViewDepense
{
    [Column("ID_DEPENSES")]
    [Precision(3)]
    public byte IdDepenses { get; set; }

    [Column("EXERCICE_FISCALE_DEPENSE")]
    [Precision(4)]
    public byte? ExerciceFiscaleDepense { get; set; }

    [Column("MOIS_DE_DEPENSE")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MoisDeDepense { get; set; }

    [Column("SOURCE_DE_FINANCEMENT_DEPENSE")]
    [StringLength(30)]
    [Unicode(false)]
    public string? SourceDeFinancementDepense { get; set; }

    [Column("ARTICLE_DEPENSE")]
    [StringLength(40)]
    [Unicode(false)]
    public string? ArticleDepense { get; set; }

    [Column("ALINEA_DEPENSE")]
    [StringLength(40)]
    [Unicode(false)]
    public string? AlineaDepense { get; set; }

    [Column("MONTANT_DEPENSE_SUR_ALINEA", TypeName = "NUMBER(12,2)")]
    public decimal? MontantDepenseSurAlinea { get; set; }

    [Column("ID_ACTIVITES")]
    [Precision(6)]
    public int IdActivites { get; set; }
}
