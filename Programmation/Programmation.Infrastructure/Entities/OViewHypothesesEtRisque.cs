using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Entities;

[Keyless]
public partial class OViewHypothesesEtRisque
{
    [Column("ID_HYPOTHESES_ET_RISQUES")]
    [Precision(6)]
    public int IdHypothesesEtRisques { get; set; }

    [Column("EXERCICE_FISCALE_DEBUT")]
    [Precision(4)]
    public byte? ExerciceFiscaleDebut { get; set; }

    [Column("EXERCICE_FISCALE_FIN")]
    [Precision(4)]
    public byte? ExerciceFiscaleFin { get; set; }

    [Column("DESCRIPTION_DES_CONDITIONS_POUVANT_IMPACTER_LE_PROJET")]
    [StringLength(200)]
    [Unicode(false)]
    public string? DescriptionDesConditionsPouvantImpacterLeProjet { get; set; }

    [Column("STRATEGIES_POUR_ASSURER_UN_IMPACT_SUR_LE_PROJET")]
    [StringLength(200)]
    [Unicode(false)]
    public string? StrategiesPourAssurerUnImpactSurLeProjet { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
