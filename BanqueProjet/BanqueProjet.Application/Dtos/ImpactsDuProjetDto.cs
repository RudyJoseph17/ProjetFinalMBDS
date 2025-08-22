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
    public class ImpactsDuProjetDto
    {


        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdImpactsProjet { get; set; }

        
        public string? DescImpactsDuProjet { get; set; }
    }
}
