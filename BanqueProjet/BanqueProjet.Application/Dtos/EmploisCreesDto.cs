using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BanqueProjet.Application.Dtos
{
    public class EmploisCreesDto
    {
       
        public byte IdEmploisCrees { get; set; }

       
        public string? CodeInterne { get; set; }

       
        public byte? EmploiHommePendant { get; set; }

       
        public byte? EmploiFemmePendant { get; set; }

      
        public byte? EmploiHommeApres { get; set; }

      
        public byte? EmploiFemmeApres { get; set; }


        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
    }
}
