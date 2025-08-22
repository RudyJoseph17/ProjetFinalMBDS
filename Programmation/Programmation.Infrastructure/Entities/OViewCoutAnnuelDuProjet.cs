using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Entities;

[Keyless]
public partial class OViewCoutAnnuelDuProjet
{
    [Column("ID_COUT_ANNUEL_PROJET")]
    [Precision(3)]
    public byte IdCoutAnnuelProjet { get; set; }

    [Column("EXERCICE_FISCALE_PREVISION_TOT")]
    [Precision(12)]
    public long? ExerciceFiscalePrevisionTot { get; set; }

    [Column("SOURCES_DE_FINANCEMENT_COUT_AN")]
    [StringLength(30)]
    [Unicode(false)]
    public string? SourcesDeFinancementCoutAn { get; set; }

    [Column("MONTANT_ANNUEL", TypeName = "NUMBER(12,2)")]
    public decimal? MontantAnnuel { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
