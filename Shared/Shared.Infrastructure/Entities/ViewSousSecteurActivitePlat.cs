using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewSousSecteurActivitePlat
{
    [Column("IDSOUSSECTEUR")]
    [Precision(6)]
    public int Idsoussecteur { get; set; }

    [Column("NOMSOUSSECTEUR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomsoussecteur { get; set; }

    [Column("IDSECTEUR")]
    [Precision(6)]
    public int Idsecteur { get; set; }

    [Column("NOMSECTEUR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomsecteur { get; set; }
}
