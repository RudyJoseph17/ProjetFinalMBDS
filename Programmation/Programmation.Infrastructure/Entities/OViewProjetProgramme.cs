using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Infrastructure.Entities
{
    [Keyless]
    [Table("VIEW_IDENTIFICATION_PROJET_PLAT")]
    public partial class OViewProjetProgramme
    {
        [Column("ID_IDENTIFICATION_PROJET")]
        [StringLength(12)]
        [Unicode(false)]
        public string IdIdentificationProjet { get; set; } = null!;

        [Column("NOM_PROJET")]
        [StringLength(100)]
        [Unicode(false)]
        public string? NomProjet { get; set; }
    }
}
