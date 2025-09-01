using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewAlineaNomenclatureBudgetairePlat
{
    [Column("IDALINEA")]
    [Precision(6)]
    public int Idalinea { get; set; }

    [Column("NOMALINEA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomalinea { get; set; }

    [Column("IDPARAGRAPHE")]
    [Precision(6)]
    public int? Idparagraphe { get; set; }

    [Column("NOMPARAGRAPHE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nomparagraphe { get; set; }
}
