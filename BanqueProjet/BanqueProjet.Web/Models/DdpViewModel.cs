using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Web.Models
{
    public class DdpViewModel
    {
        public IdentificationProjetDto Projets { get; set; } = new();
        public DdpCadreLogiqueDto CadreLogique { get; set; } = new();
        public AspectsJuridiquesDto AspectsJuridiques { get; set; } = new();
        public DureeProjetDto DureeProjet { get; set; } = new();
        public EchelonTerritorialeDto Echelon { get; set; } = new();
        public EmploisCreesDto EmploisCrees { get; set; } = new();
        public GestionDeProjetDto GestionProjet { get; set; } = new();
        public LocalisationGeographiqueProjDto LocalisationGeographique { get; set; } = new();
        public PhaseDuProjetDto PhaseProjet { get; set; } = new();
        public PrevisionDto Prevision { get; set; } = new();
        public SuiviEtControleDto SuiviControle { get; set; } = new();
    }
}
