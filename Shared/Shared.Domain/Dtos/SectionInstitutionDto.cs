using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared.Domain.Dtos
{
    public class SectionInstitutionDto
    {
        [JsonProperty("IdSection")]
        public int? IdSection { get; set; }
        
        [JsonProperty("IdInstitutionSectorielle")]
        public int? IdInstitution { get; set; }

        [JsonProperty("NomSection")]
        public string? NomSection { get; set; }

        [JsonProperty("SigleSection")]
        public string? SigleSection { get; set; }

        [JsonProperty("AdresseSection")]
        public string? AdresseSection { get; set; }
 
    }
}
