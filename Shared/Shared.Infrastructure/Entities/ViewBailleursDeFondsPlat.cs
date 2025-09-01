using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewBailleursDeFondsPlat
{
    [Column("IDBAILLEUR")]
    [Precision(6)]
    public int IdBailleur { get; set; }

    [Column("NOMBAILLEUR")]
    [StringLength(500)]
    [Unicode(false)]
    public string? NomBailleur { get; set; }

    [Column("TYPEBAILLEUR")]
    [StringLength(500)]
    [Unicode(false)]
    public string? TypeBailleur { get; set; }

    [Column("LIBELLEBAILLEUR")]
    [Unicode(false)]
    public string? Libellebailleur { get; set; }
}
