using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewSectionCommunalePlat
{
    [Column("IDSECTIONCOMMUNALE")]
    [Precision(6)]
    public int Idsectioncommunale { get; set; }

    [Column("NOMSECTIONCOMMUNALE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomsectioncommunale { get; set; }

    [Column("IDCOMMUNE")]
    [Precision(6)]
    public int? Idcommune { get; set; }

    [Column("NOMCOMMUNE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomcommune { get; set; }
}
