using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class AutorisationSurProjetDto: InformationsFinancieresProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? Article { get; set; }
        public string? Alinea { get; set; }
        public string? MoisAutorisation { get; set; }
        public decimal? MontantAutorisation { get; set; }
    }
}
