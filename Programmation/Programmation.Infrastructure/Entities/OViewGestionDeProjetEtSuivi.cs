using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Programmation.Infrastructure.Entities;

[Keyless]
public partial class OViewGestionDeProjetEtSuivi
{
    [Column("ID_GESTION_DE_PROJET_ET_SUIVI")]
    [Precision(6)]
    public int IdGestionDeProjetEtSuivi { get; set; }

    [Column("EXERCICE_FISCALE_DEBUT")]
    [Precision(4)]
    public byte? ExerciceFiscaleDebut { get; set; }

    [Column("EXERCICE_FISCALE_FIN")]
    [Precision(4)]
    public byte? ExerciceFiscaleFin { get; set; }

    [Column("DESCRIPTION_ROLE")]
    [StringLength(500)]
    [Unicode(false)]
    public string? DescriptionRole { get; set; }

    [Column("CADRE_DE_REFERENCE_CONTRAT")]
    [StringLength(500)]
    [Unicode(false)]
    public string? CadreDeReferenceContrat { get; set; }

    [Column("DESCRIPTION_DES_METHODES")]
    [StringLength(500)]
    [Unicode(false)]
    public string? DescriptionDesMethodes { get; set; }

    [Column("IDENTIFICATION_DES_AGENTS_IMPLIQUES")]
    [StringLength(200)]
    [Unicode(false)]
    public string? IdentificationDesAgentsImpliques { get; set; }

    [Column("ID_IDENTIFICATION_PROJET")]
    [StringLength(12)]
    [Unicode(false)]
    public string IdIdentificationProjet { get; set; } = null!;
}
