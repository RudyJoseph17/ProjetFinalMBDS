using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Web.Models
{
    public class DdpViewModel
    {
        public int Step { get; set; }
        public ProjetsBPDto Projets { get; set; } = new();
        public List<ActiviteBPDto> ActiviteBP { get; set; } = new();
        public List<ActivitesAnnuellesDto> ActivitesAnnuelles { get; set; } = new();
        public List<BailleursDeFondsDto> BailleursDeFonds { get; set; } = new();
        public DdpCadreLogiqueDto CadreLogique { get; set; } = new();
        public List<AspectsJuridiquesDto> AspectsJuridiques { get; set; } = new();
        public List<PartiesPrenantesDto> PartiesPrenantesProjets { get; set; } = new();
        public LocalisationGeographiqueProjDto LocalisationGeographique { get; set; } = new();
        public List<CoutAnnuelDuProjetDto> CoutAnnuelDuProjet { get; set; } = new();
        public List<EffetsDuProjetDto> EffetsProjets { get; set; } = new();
        public List<ImpactsDuProjetDto> ImpactsDuProjets { get; set; } = new();
        public List<IndicateursDeResultatDto> IndicateursResultats { get; set; } = new();
        public List<InformationsFinancieresBPDto> InformationsFinancieresBP { get; set; } = new();
        public List<DefinitionLivrablesDuProjetDto> DefinitionLivrables { get; set; } = new();
        public List<ObjectifsSpecifiquesDto> ObjectifsSpecifiques { get; set; } = new();
    }
}
