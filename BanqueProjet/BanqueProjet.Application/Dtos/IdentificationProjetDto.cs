using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BanqueProjet.Application.Dtos
{
    public class IdentificationProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public string? NomProjet { get; set; }
        public string? Ministere { get; set; }
        public string? Section { get; set; }
        public string CodePip { get; set; }
        public string CodeBailleur { get; set; }
        public string JustificationProjet { get; set; }
        public string EtudePrefaisabilite { get; set; }
        public string EtudeFaisabilite { get; set; }
        public string PopulationVisee { get; set; }
        public string? Programme { get; set; }
        public string? SousProgramme { get; set; }
        public DateTime? DateInscription { get; set; }
        public DateTime? DateMiseAJour { get; set; }
        public string? TypeDeProjet { get; set; }
        public string? SecteurDActivites { get; set; }
        public string? SousSecteurDActivites { get; set; }
        public string? NomDirecteurDeProjet { get; set; }
        public string? TelephoneDirecteurDeProjet { get; set; }
        public string? CourrielDirecteurDeProjet { get; set; }
        public string? ObjectifGeneralProjet { get; set; }
        public string? DureeProjet { get; set; }
        public decimal? CoutTotalProjet { get; set; }
        public string? EchelonTerritorial { get; set; }


        // Collections simplifiées ou DTOs correspondants
        public List<ActiviteDto> Activites { get; set; } = new ();
        public List<AspectsJuridiquesDto> AspectsJuridiques { get; set; } = new();
        public List<PartiesPrenantesDto> PartiesPrenantes { get; set; } = new();
        public List<IndicateursDeResultatDto> IndicateursDeResultats { get; set; } = new();
        public List<LivrablesDuProjetDto> LivrablesProjets { get; set; } = new();
        public List<EffetsDuProjetDto> EffetsProjets { get; set; } = new();
        public List<ObjectifsSpecifiquesDto> ObjectifsSpecifiques { get; set; } = new();
        public List<ImpactsDuProjetDto> ImpactsDesProjets { get; set; } = new();
        public List<BailleursDeFondDto> BailleursDeFonds { get; set; } = new();
        public List<ActivitesAnnuellesDto> ActivitesAnuelles { get; set; } = new();
        public List<CoutAnnuelDuProjetDto> CoutAnnuelsProjets { get; set; } = new();



        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
            {
                    // Clé = nom passé à GenererIdPour()
                 { "IdentificationProjetDto", "XXB-BBBB-XX-XBB" },
            };

    }
}
