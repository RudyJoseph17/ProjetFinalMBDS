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
    [Table("O_VIEW_LIVRABLES_DU_PROJET")]
    public partial class OViewLivrablesProgrammes
    {
        [Column("ID_LIVRABLES_PROJET")]
        [Precision(6)]
        public int IdLivrablesProjet { get; set; }

        [Column("DEFINITION_LIVRABLES_DU_PROJET")]
        [StringLength(100)]
        [Unicode(false)]
        public string? DefinitionLivrablesDuProjet { get; set; }

        [Column("QUANTITE_A_LIVRER")]
        [Precision(7)]
        public int? QuantiteALivrer { get; set; }

        [Column("ID_IDENTIFICATION_PROJET")]
        [StringLength(12)]
        [Unicode(false)]
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
