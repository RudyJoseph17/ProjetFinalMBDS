using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewParagrapheNomenclatureBudgetairePlat
{
    [Column("IDPARAGRAPHE")]
    [Precision(6)]
    public int Idparagraphe { get; set; }

    [Column("NOMPARAGRAPHE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomparagraphe { get; set; }

    [Column("IDARTICLE")]
    [Precision(6)]
    public int? Idarticle { get; set; }

    [Column("NOMARTICLE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomarticle { get; set; }
}
