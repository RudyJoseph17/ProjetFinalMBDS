using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Web.Models
{
    public class DdpViewModel
    {
        public ProjetsBPDto Projets { get; set; } = new();
        public ActiviteBPDto ActiviteBP { get; set; } = new();
        public ActivitesAnnuellesDto ActiviteAnuelle { get; set; } = new();
        public BailleursDeFondsDto BailleursDeFonds { get; set; } = new();
        public DdpCadreLogiqueDto CadreLogique { get; set; } = new();
        public AspectsJuridiquesDto AspectsJuridiques { get; set; } = new();
        public PartiesPrenantesDto PartiesPrenantesProjets { get; set; } = new();
        public LocalisationGeographiqueProjDto LocalisationGeographique { get; set; } = new();
        public CoutAnnuelDuProjetDto CoutAnnuelProjet { get; set; } = new();
        public EffetsDuProjetDto EffetsProjets { get; set; } = new();
        public ImpactsDuProjetDto ImpactsDuProjets { get; set; } = new();
        public IndicateursDeResultatDto IndicateursResultats { get; set; } = new();
        public InformationsFinancieresBPDto InformationsFinancieresBP { get; set; } = new();
        public DefinitionLivrablesDuProjetDto DefinitionLivrables { get; set; } = new();
        public ObjectifsSpecifiquesDto ObjectifsSpecifiques { get; set; } = new();
    }
}
