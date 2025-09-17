using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Dtos
{
    public class ProgrammationProjetDto: IdentificationProjetDto
    {
        public List<LivrablesProgrameProjetDto> LivrablesProgrammesProjets { get; set; } = new();
        public List<InformationsFinancieresProgrammeesProjetDto> InformationsFinancieresProgrammeesProjet { get; set; } = new();
    }
}
