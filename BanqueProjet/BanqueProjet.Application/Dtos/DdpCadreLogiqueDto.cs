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
    public class DdpCadreLogiqueDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdDdpCadreLogique { get; set; }
        public string? IntrantsResumeNarratif { get; set; }
        public string? ExtrantsResumeNarratif { get; set; }
        public string? ObjectifsSpecifiquesResumeNarratif { get; set; }
        public string? ObjectifGeneralResumeNarratif { get; set; }
        public string? IntrantsIov { get; set; }
        public string? ExtrantsIov { get; set; }
        public string? ObjectifsSpecifiquesIov { get; set; }
        public string? ObjectifGeneralIov { get; set; }
        public string? IntrantsSmov { get; set; }
        public string? ExtrantsSmov { get; set; }
        public string? ObjectifsSpecifiquesSmov { get; set; }
        public string? ObjectifGeneralSmov { get; set; }
        public string? IntrantsRisquesHypotheses { get; set; }
        public string? ExtrantsRisquesHypotheses { get; set; }
        public string? ObjectifsSpecifiquesRisquesHypotheses { get; set; }
        public string? ObjectifGeneralRisquesHypotheses { get; set; }
    }
}
