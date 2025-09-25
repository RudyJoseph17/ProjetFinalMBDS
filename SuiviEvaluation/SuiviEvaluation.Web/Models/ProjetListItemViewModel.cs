namespace SuiviEvaluation.Web.Models
{
    public class ProjetListItemViewModel
    {
        public string IdIdentificationProjet { get; set; } = string.Empty;
        public string NomProjet { get; set; } = string.Empty;

        // Ici, c'est maintenant la somme des montants
        public decimal TotalAutorisation { get; set; } = 0;
    }
}
