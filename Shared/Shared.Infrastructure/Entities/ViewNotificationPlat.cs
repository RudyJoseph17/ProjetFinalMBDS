using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewNotificationPlat
{
    [Column("ID_NOTIFICATION")]
    [Precision(10)]
    public int IdNotification { get; set; }

    [Column("TITRE")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Titre { get; set; }

    [Column("MESSAGE")]
    [StringLength(2000)]
    [Unicode(false)]
    public string? Message { get; set; }

    [Column("ID_RECEVEUR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdReceveur { get; set; }

    [Column("EST_LU_CHAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? EstLuChar { get; set; }

    [Column("EST_LU_BOOL")]
    [StringLength(5)]
    [Unicode(false)]
    public string? EstLuBool { get; set; }

    [Column("DATE_NOTIFICATION", TypeName = "DATE")]
    public DateTime? DateNotification { get; set; }

    [Column("RESUME")]
    [Unicode(false)]
    public string? Resume { get; set; }
}
