using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetImpactsPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_IMPACTS_PROJET")]
    [Precision(3)]
    public byte? IdImpactsProjet { get; set; }

    [Column("IMPACTS_DU_PROJET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ImpactsDuProjet { get; set; }
}
