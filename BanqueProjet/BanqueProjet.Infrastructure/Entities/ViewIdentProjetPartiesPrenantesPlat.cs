using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetPartiesPrenantesPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_PARTIES_PRENANTES")]
    [Precision(6)]
    public int? IdPartiesPrenantes { get; set; }

    [Column("NOM_FIRME")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NomFirme { get; set; }

    [Column("TELEPHONE_FIRME")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneFirme { get; set; }

    [Column("COURRIEL_FIRME")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CourrielFirme { get; set; }
}
