using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Entities
{
    [Keyless]
    [Table("O_VIEW_LIVRABLES_DU_PROJET")]
    public class OViewLivrablesProjet
    {
        [Column("ID_IDENTIFICATION_PROJET")] 
        public string IdIdentificationProjet { get; set; }

        [Column("ID_LIVRABLES_PROJET")] 
        public int IdLivrablesProjet { get; set; }

        [Column("DEFINITION_LIVRABLES_DU_PROJET")] 
        public string DefinitionLivrables { get; set; }
        
        [Column("VALEUR_LIVREE")] 
        public int ValeurLivree { get; set; }
    }

}
