using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewLivrablesDuProjet
{
    [Column("ID_LIVRABLES_PROJET")]
    [Precision(3)]
    public byte IdLivrablesProjet { get; set; }

    [Column("DEFINITION_LIVRABLES_DU_PROJET")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DefinitionLivrablesDuProjet { get; set; }

    [Column("QUANTITE_A_LIVRER")]
    [Precision(7)]
    public int? QuantiteALivrer { get; set; }

    [Column("QUANTITE_LIVREE")]
    [Precision(7)]
    public int? QuantiteLivree { get; set; }

    [Column("VALEUR_LIVREE")]
    [Precision(7)]
    public int? ValeurLivree { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
