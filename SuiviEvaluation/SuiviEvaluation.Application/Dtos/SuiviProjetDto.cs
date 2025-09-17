using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class SuiviProjetDto : IdentificationProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public List<LivrablesRealisesProjetDto> LivrablesRealisesProjets { get; set; } = new();

        public List<AutorisationSurProjetDto> AutorisationsSurProjets { get; set; } = new();

        public List<DecaissementSurProjetDto> DecaissementsSurProjets { get; set; } = new();

        public List<DepenseReelleSurProjetDto> DepensesReellesSurProjets { get; set; } = new();

    }
}
