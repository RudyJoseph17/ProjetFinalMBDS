using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Dtos
{
    public class PhaseDuProjetDto
    {
       
        public byte IdPhaseDuProjet { get; set; }

        
        public long? CodeInterne { get; set; }

        
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
