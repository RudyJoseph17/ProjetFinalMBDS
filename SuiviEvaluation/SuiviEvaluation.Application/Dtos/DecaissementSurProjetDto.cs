using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class DecaissementSurProjetDto : InformationsFinancieresProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? Article { get; set; }
        public string? Alinea { get; set; }
        public string? MoisDecaissement { get; set; }
        public decimal? MontantDecaissement { get; set; }
    }
}
