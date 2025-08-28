using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class SuiviInformationFinanciereDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public int IdActivites { get; set; }
        public int? IdInformationsFinancieres { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? SourcesFinancement { get; set; }
        public decimal? MontantAutorisation { get; set; }
        public decimal? MontantDecaissement { get; set; }
        public decimal? MontantDepense { get; set; }
    }
}
