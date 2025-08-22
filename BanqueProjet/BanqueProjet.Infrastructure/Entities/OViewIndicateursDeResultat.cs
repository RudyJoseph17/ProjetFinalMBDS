using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewIndicateursDeResultat
{
    [Column("ID_INDICATEURS_DE_RESULTATS")]
    [Precision(3)]
    public byte IdIndicateursDeResultats { get; set; }

    [Column("DEFINITION_INDICATEURS_DE_RESU")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DefinitionIndicateursDeResu { get; set; }

    [Column("QUANTITE_ASSOCIE_A_INDICATEUR")]
    [Precision(7)]
    public int? QuantiteAssocieAIndicateur { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
