using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewSousProgrammePlat
{
    [Column("IDSOUSPROGRAMME")]
    [Precision(6)]
    public int Idsousprogramme { get; set; }

    [Column("NOMSOUSPROGRAMME")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomsousprogramme { get; set; }

    [Column("IDPROGRAMME")]
    [Precision(6)]
    public int Idprogramme { get; set; }

    [Column("NOMPROGRAMME")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomprogramme { get; set; }
}
