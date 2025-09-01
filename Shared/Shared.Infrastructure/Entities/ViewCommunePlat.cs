using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewCommunePlat
{
    [Column("IDCOMMUNE")]
    [Precision(6)]
    public int Idcommune { get; set; }

    [Column("NOMCOMMUNE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomcommune { get; set; }

    [Column("IDARRONDISSEMENT")]
    [Precision(6)]
    public int? Idarrondissement { get; set; }

    [Column("NOMARRONDISSEMENT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomarrondissement { get; set; }
}
