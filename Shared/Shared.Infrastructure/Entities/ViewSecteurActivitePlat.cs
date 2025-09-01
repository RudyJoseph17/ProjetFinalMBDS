using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewSecteurActivitePlat
{
    [Column("IDSECTEUR")]
    [Precision(6)]
    public int Idsecteur { get; set; }

    [Column("NOMSECTEUR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomsecteur { get; set; }
}
