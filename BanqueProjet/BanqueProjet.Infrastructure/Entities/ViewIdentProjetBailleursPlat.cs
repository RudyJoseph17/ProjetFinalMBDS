using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetBailleursPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_BAILLEURS_DE_FONDS")]
    [Precision(6)]
    public int? IdBailleursDeFonds { get; set; }

    [Column("NOM_BAILLEUR")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NomBailleur { get; set; }

    [Column("TELEPHONE_REPRESENTANT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneRepresentant { get; set; }

    [Column("COURRIEL_REPRESENTANT")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CourrielRepresentant { get; set; }
}
