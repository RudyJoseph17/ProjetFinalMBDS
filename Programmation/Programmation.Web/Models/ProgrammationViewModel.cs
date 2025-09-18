using BanqueProjet.Application.Dtos;
using Programmation.Application.Dtos;
using System.Collections.Generic;

namespace Programmation.Web.Models
{
    public class ProgrammationViewModel
    {
        public ProgrammationProjetDto ProjetsCrees { get; set; } = new();
        public List<LivrablesProgrameProjetDto> LivrablesProgramme { get; set; } = new();
        public List<InformationsFinancieresProgrammeesProjetDto> InfosFinancieresProgrammees { get; set; } = new();
    }
}
