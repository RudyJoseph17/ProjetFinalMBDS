using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetAspectsJuridiquesPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_ASPECTS_JURIDIQUES")]
    [Precision(6)]
    public int? IdAspectsJuridiques { get; set; }

    [Column("DESCRIPTION_ASPECT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? DescriptionAspect { get; set; }
}
