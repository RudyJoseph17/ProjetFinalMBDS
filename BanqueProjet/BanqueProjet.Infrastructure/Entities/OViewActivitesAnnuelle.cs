using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Entities;

[Keyless]
public partial class OViewActivitesAnnuelle
{
    [Column("ID_ACTIVITES_ANNUELLES")]
    [Precision(6)]
    public int IdActivitesAnnuelles { get; set; }

    [Column("DESCRIPTION_ACTIVITE")]
    [StringLength(400)]
    [Unicode(false)]
    public string? DescriptionActivite { get; set; }

    [Column("EXERCICE_FISCAL_DEBUT")]
    [Precision(4)]
    public byte? ExerciceFiscalDebut { get; set; }

    [Column("EXERCICE_FISCAL_FIN")]
    [Precision(4)]
    public byte? ExerciceFiscalFin { get; set; }

    [Column("COUT_ANNUEL", TypeName = "NUMBER(12,2)")]
    public decimal? CoutAnnuel { get; set; }

    [Column("DATE_DEBUT", TypeName = "DATE")]
    public DateTime? DateDebut { get; set; }

    [Column("DATE_FIN", TypeName = "DATE")]
    public DateTime? DateFin { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
