using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Entities;

[Keyless]
public partial class OViewQuantiteLivreParAnnee
{
    [Column("ID_QUANTITE_LIVREE_PAR_ANNEE")]
    [Precision(3)]
    public byte IdQuantiteLivreeParAnnee { get; set; }

    [Column("ANNEE_QUANTITE_LIVREE")]
    [Precision(4)]
    public byte? AnneeQuantiteLivree { get; set; }

    [Column("VALEUR_LIVREE")]
    [Precision(7)]
    public int? ValeurLivree { get; set; }

    [Column("ID_LIVRABLES_PROJET")]
    [Precision(3)]
    public byte IdLivrablesProjet { get; set; }
}
