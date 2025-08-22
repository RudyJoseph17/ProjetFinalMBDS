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
    public class PrevisionDto
    {
        public byte IdPrevision { get; set; }

        
        public string? ArticlePrevision { get; set; }

      
        public string? AlineaPrevision { get; set; }

        
        public decimal? MontantPrevisionSurAlinea { get; set; }

       
        public int IdActivites { get; set; }
    }
}
