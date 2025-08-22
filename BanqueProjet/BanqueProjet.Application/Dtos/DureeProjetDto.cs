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
    public class DureeProjetDto
    {
     
        public byte IdDureeProjet { get; set; }

       
        public byte? DureeTotaleProjet { get; set; }

       
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
