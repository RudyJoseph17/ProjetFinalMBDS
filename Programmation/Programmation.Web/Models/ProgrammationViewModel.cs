using BanqueProjet.Application.Dtos;
using Programmation.Application.Dtos;

namespace Programmation.Web.Models
{
    public class ProgrammationViewModel
    {
        //public ProjetsBPDto Projets { get; set; } = new();
        public PrevisionInformationFinanciereDto PrevisionsFinancieres { get; set; } = new();
        public QuantiteALivrerParAnneeDto QuantiteALivrer { get; set; } = new();
    }
}
