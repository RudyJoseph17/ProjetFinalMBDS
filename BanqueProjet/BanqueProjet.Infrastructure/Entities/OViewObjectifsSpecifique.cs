using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewObjectifsSpecifique
{
    [Column("ID_OBJECTIFS_SPECIFIQUES")]
    [Precision(6)]
    public int IdObjectifsSpecifiques { get; set; }

    [Column("OBJECTIFS_SPECIFIQUES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ObjectifsSpecifiques { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
