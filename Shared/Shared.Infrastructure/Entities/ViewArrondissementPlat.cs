using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewArrondissementPlat
{
    [Column("IDARRONDISSEMENT")]
    [Precision(6)]
    public int Idarrondissement { get; set; }

    [Column("NOMARRONDISSEMENT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomarrondissement { get; set; }

    [Column("IDDEPARTEMENT")]
    [Precision(6)]
    public int? Iddepartement { get; set; }

    [Column("NOMDEPARTEMENT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomdepartement { get; set; }
}
