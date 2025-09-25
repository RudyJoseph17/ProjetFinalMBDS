using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class SuiviActivite: ActiviteDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public int IdActivites { get; set; }
        public string? NomActivite { get; set; }

        public List<AutorisationSurProjetDto> Autorisations { get; set; } = new();
        public List<DecaissementSurProjetDto> Decaissements { get; set; } = new();
        public List<DepenseReelleSurProjetDto> DepensesReelles { get; set; } = new();
    }
}
