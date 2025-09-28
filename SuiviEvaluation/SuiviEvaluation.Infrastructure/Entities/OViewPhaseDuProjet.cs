using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Entities;

[Keyless]
public partial class OViewPhaseDuProjet
{
    [Column("ID_PHASE_DU_PROJET")]
    [Precision(3)]
    public byte IdPhaseDuProjet { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
