using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewSectionInstitutionPlat
{
    [Column("IDSECTION")]
    [Precision(6)]
    public int Idsection { get; set; }

    [Column("NOMSECTION")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nomsection { get; set; }

    [Column("SIGLESECTION")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Siglesection { get; set; }

    [Column("ADRESSESECTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Adressesection { get; set; }

    [Column("IDINSTITUTION")]
    [Precision(6)]
    public int Idinstitution { get; set; }

    [Column("NOMINSTITUTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nominstitution { get; set; }
}
