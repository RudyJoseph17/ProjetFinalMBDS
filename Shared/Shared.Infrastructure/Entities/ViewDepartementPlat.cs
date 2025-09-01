using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewDepartementPlat
{
    [Column("IDDEPARTEMENT")]
    [Precision(6)]
    public int Iddepartement { get; set; }

    [Column("NOMDEPARTEMENT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomdepartement { get; set; }
}
