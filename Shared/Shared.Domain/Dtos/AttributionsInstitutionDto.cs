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
    public class AttributionsInstitutionDto
    {
        [JsonProperty("Idattribution")]
        public int? Idattribution { get; set; }

        [JsonProperty("IdInstitutionSectorielle")]
        public int? Idinstitution { get; set; }

        [JsonProperty("DescriptionAttribution")]
        public string? DescriptionAttribution { get; set; }
      
    }
}
