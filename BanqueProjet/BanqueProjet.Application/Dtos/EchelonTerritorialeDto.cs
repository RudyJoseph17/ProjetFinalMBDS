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
    public class EchelonTerritorialeDto
    {
      
        public byte IdEchelonTerritoriale { get; set; }

       
        public string? EchelonTerritoriale { get; set; }

       
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
