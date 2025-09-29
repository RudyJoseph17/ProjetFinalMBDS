using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Programmation.Application.Dtos
{
    public class HypothesesEtRisquesDto
    {
        public int IdHypothesesEtRisques { get; set; }
        public byte? ExerciceFiscaleDebut { get; set; }
        public byte? ExerciceFiscaleFin { get; set; }
        public string? DescriptionDesConditionsPouvantImpacterLeProjet { get; set; }
        public string? StrategiesPourAssurerUnImpactSurLeProjet { get; set; }

        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
