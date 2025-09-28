using BanqueProjet.Application.Dtos;

namespace Programmation.Web.Models
{
    public class ListeProgrammationViewModel
    {
        public string IdIdentificationProjet { get; set; } = string.Empty;
        public string NomProjet { get; set; } = string.Empty;

        // Ici, c'est maintenant la somme des montants
        public decimal TotalProgrammation { get; set; } = 0;

        public decimal CoutTotal { get; set; } = 0;
    }
}
