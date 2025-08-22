using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Entities;

[Keyless]
public partial class OViewQuantiteALivrerParAnnee
{
    [Column("ID_QUANTITE_A_LIVRER")]
    [Precision(3)]
    public byte IdQuantiteALivrer { get; set; }

    [Column("ANNEE_QUANTITE_A_LIVRER")]
    [Precision(7)]
    public int? AnneeQuantiteALivrer { get; set; }

    [Column("QUANTITE_A_LIVRER")]
    [Precision(7)]
    public int? QuantiteALivrer { get; set; }

    [Column("ID_LIVRABLES_PROJET")]
    [Precision(3)]
    public byte IdLivrablesProjet { get; set; }
}
