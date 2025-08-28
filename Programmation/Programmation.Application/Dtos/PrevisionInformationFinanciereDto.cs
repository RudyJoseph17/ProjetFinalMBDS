using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Dtos
{
    public class PrevisionInformationFinanciereDto: InformationsFinancieresProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public int IdActivites { get; set; }
        public int? IdInformationsFinancieres { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? SourcesFinancement { get; set; }
        public decimal? MontantPrevu { get; set; }
    }
}
