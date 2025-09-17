using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewLocalisationGeographiqueProj
{
    [Column("ID_LOCALISATION_GEOGRAPHIQUE")]
    [Precision(3)]
    public byte IdLocalisationGeographique { get; set; }

    [Column("DEPARTEMENT")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Departement { get; set; }

    [Column("ARRONDISSEMENT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Arrondissement { get; set; }

    [Column("COMMUNE")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Commune { get; set; }

    [Column("SECTION_COMMUNALE")]
    [StringLength(40)]
    [Unicode(false)]
    public string? SectionCommunale { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
