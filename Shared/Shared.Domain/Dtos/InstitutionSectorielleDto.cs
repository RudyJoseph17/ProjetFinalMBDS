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
    public class InstitutionSectorielleDto
    {
        [JsonProperty("IdInstitutionSectorielle")]
        public int Idinstitution { get; set; }
        
        [JsonProperty("Nominstitution")]
        public string? Nominstitution { get; set; }

        [JsonProperty("Sigleinstitution")]
        public string? Sigleinstitution { get; set; }

        [JsonProperty("Missioninstitution")]
        public string? Missioninstitution { get; set; }

        [JsonProperty("AttributionsInstitution")]
        public List<AttributionsInstitutionDto> ListAttributions { get; set; } = new();

        [JsonProperty("SectionsInstitution")]
        public List<SectionInstitutionDto> ListSections { get; set; } = new();
    }
}
