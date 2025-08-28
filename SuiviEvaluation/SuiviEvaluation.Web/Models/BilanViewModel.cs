using SuiviEvaluation.Application.Dtos;

namespace SuiviEvaluation.Web.Models
{
    public class BilanViewModel
    {
        public SuiviInformationFinanciereDto BilanFinancier { get; set; } = new();
        public QuantiteLivreeParAnneeDto QuantiteLivreeParAnnee { get; set; } = new();
    }
}
