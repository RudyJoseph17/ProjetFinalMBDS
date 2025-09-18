using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Dtos
{
    public class ProgrammationProjetDto: IdentificationProjetDto
    {
        [JsonProperty("ID_IDENTIFICATION_PROJET")]
        public string IdIdentificationProjet { get; set; }

        [JsonProperty("NOM_PROJET")]
        public string? NomProjet { get; set; }

        [NotMapped]
        public DateOnly DateProgrammation { get; set; }
        public List<LivrablesProgrameProjetDto> LivrablesProgrammesProjets { get; set; } = new();
        public List<InformationsFinancieresProgrammeesProjetDto> InformationsFinancieresProgrammeesProjet { get; set; } = new();
    }
}
