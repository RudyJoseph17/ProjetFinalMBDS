using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetEffetsPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_EFFETS_DU_PROJET")]
    [Precision(3)]
    public byte? IdEffetsDuProjet { get; set; }

    [Column("EFFET_DU_PROJET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? EffetDuProjet { get; set; }
}
