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
    public class LocalisationGeographiqueProjDto
    {
       
        public byte IdLocalisationGeographique { get; set; }

      
        public string? Departement { get; set; }

       
        public string? Arrondissement { get; set; }

      
        public string? Commune { get; set; }

      
        public string? SectionCommunale { get; set; }

        public string IdIdentificationProjet { get; set; } = null!;
    }
}
