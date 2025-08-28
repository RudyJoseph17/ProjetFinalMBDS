using SuiviEvaluation.Application.Dtos;


namespace SuiviEvaluation.Web.Models
{
    public class Bilan
    {
       
        public List<EvolutionTemporelleDuProjetDto> Evolution { get; set; }
       
        public List<FluxFinancierDto> FluxFinanciers { get; set; }

        public List<QuantiteLivreeParAnneeDto> QuantitesLivrees { get; set; }
    }
}
