using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewInstitutionSectoriellePlat
{
    [Column("IDINSTITUTION")]
    [Precision(6)]
    public int Idinstitution { get; set; }

    [Column("NOMINSTITUTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nominstitution { get; set; }

    [Column("SIGLEINSTITUTION")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Sigleinstitution { get; set; }

    [Column("MISSIONINSTITUTION")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Missioninstitution { get; set; }
}
