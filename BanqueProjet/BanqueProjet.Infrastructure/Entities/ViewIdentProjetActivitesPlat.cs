using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class ViewIdentProjetActivitesPlat
{
    [Column("ID_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdProjet { get; set; } = null!;

    [Column("ID_ACTIVITES")]
    [Precision(6)]
    public int? IdActivites { get; set; }

    [Column("NUMERO_ACTIVITES")]
    [Precision(3)]
    public byte? NumeroActivites { get; set; }

    [Column("NOM_ACTIVITE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NomActivite { get; set; }

    [Column("RESULTATS_ATTENDUS")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ResultatsAttendus { get; set; }
}
