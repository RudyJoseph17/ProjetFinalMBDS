using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewAttributionsInstitutionPlat
{
    [Column("IDATTRIBUTION")]
    [Precision(6)]
    public int Idattribution { get; set; }

    [Column("DESCRIPTIONATTRIBUTION")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Descriptionattribution { get; set; }

    [Column("IDINSTITUTION")]
    [Precision(6)]
    public int Idinstitution { get; set; }

    [Column("NOMINSTITUTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nominstitution { get; set; }
}
