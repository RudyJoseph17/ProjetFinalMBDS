using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiviEvaluation.Infrastructure.Entities;

[Keyless]
public partial class OViewEvolutionTemporelleDuProjet
{
    [Column("ID_EVOLUTION_TEMPORELLE")]
    [Precision(3)]
    public byte IdEvolutionTemporelle { get; set; }

    [Column("DATE_DE_DEMMARAGE_", TypeName = "DATE")]
    public DateTime? DateDeDemmarage { get; set; }

    [Column("DATE_ACHEVEMENT_PREVUE", TypeName = "DATE")]
    public DateTime? DateAchevementPrevue { get; set; }

    [Column("TEMPS_ECOULE")]
    [Precision(3)]
    public byte? TempsEcoule { get; set; }

    [Column("TEMPS_RESTANT")]
    [Precision(3)]
    public byte? TempsRestant { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
