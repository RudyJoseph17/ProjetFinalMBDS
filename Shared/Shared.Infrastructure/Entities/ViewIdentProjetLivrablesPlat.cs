using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetLivrablesPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_LIVRABLES_PROJET")]
    [Precision(3)]
    public byte? IdLivrablesProjet { get; set; }

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
}
