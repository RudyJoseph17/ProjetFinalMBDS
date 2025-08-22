using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Entities;

[Keyless]
public partial class OViewAutorisation
{
    [Column("ID_AUTORISATION")]
    [Precision(3)]
    public byte IdAutorisation { get; set; }

    [Column("EXERCICE_FISCALE_AUTORISATION")]
    [Precision(12)]
    public long? ExerciceFiscaleAutorisation { get; set; }

    [Column("MOIS_AUTORISATION")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MoisAutorisation { get; set; }

    [Column("SOURCES_FINANCEMENT_AUTORISATI")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SourcesFinancementAutorisati { get; set; }

    [Column("ARTICLE_AUTORISE")]
    [StringLength(40)]
    [Unicode(false)]
    public string? ArticleAutorise { get; set; }

    [Column("ALINEA_AUTORISEE")]
    [StringLength(40)]
    [Unicode(false)]
    public string? AlineaAutorisee { get; set; }

    [Column("MONTANT_AUTORISATION_SUR_ALINE", TypeName = "NUMBER(12,2)")]
    public decimal? MontantAutorisationSurAline { get; set; }

    [Column("ID_ACTIVITES")]
    [Precision(6)]
    public int IdActivites { get; set; }
}
