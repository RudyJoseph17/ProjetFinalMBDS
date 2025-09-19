using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Domain.Dtos;
using BanqueProjet.Application.Enums;

namespace BanqueProjet.Application.Dtos
{
    public class ProjetsBPDto : IdentificationProjetDto
    {
        [JsonProperty("ID_IDENTIFICATION_PROJET")]
        public string IdIdentificationProjet { get; set; }

        [JsonProperty("NOM_PROJET")]
        public string? NomProjet { get; set; }
        public string? Ministere { get; set; }
        public string? Section { get; set; }

        [JsonProperty("CODE_PIP")]
        public string CodePip { get; set; }
        public string CodeBailleur { get; set; }
        public string JustificationProjet { get; set; }
        public string EtudePrefaisabilite { get; set; }
        public string EtudeFaisabilite { get; set; }
        public string PopulationVisee { get; set; }
        public string? Programme { get; set; }
        public string? SousProgramme { get; set; }

        [JsonProperty("DATE_INSCRIPTION")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateInscription { get; set; }

        [JsonProperty("DATE_MISE_A_JOUR")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateMiseAJour { get; set; }
        public TypeProjet? TypeDeProjet { get; set; }
        public string? SecteurDActivites { get; set; }
        public string? SousSecteurDActivites { get; set; }
        public string? NomDirecteurDeProjet { get; set; }
        public string? TelephoneDirecteurDeProjet { get; set; }
        public string? CourrielDirecteurDeProjet { get; set; }
        public string? ObjectifGeneralProjet { get; set; }
        public string? DureeProjet { get; set; }
        public decimal? CoutTotalProjet { get; set; }
        public EchelonTerritorial? EchelonTerritorial { get; set; }
        public string? AvisProjet { get; set; } // "oui", "non", "Projet à analyser"
        public byte? IdGrilleDdpProjet { get; set; } // Id de la grille si existante



        // Collections simplifiées ou DTOs correspondants
        public List<ActiviteBPDto> Activites { get; set; } = new ();
        public List<AspectsJuridiquesDto> AspectsJuridiques { get; set; } = new();



        [JsonProperty("Listpartprenantes")]
         public List<PartiesPrenantesDto> PartiesPrenantes { get; set; } = new();
        public List<IndicateursDeResultatDto> IndicateursDeResultats { get; set; } = new();
        public List<DefinitionLivrablesDuProjetDto> LivrablesProjets { get; set; } = new();
        public List<EffetsDuProjetDto> EffetsProjets { get; set; } = new();
        public List<ObjectifsSpecifiquesDto> ObjectifsSpecifiques { get; set; } = new();
        public List<ImpactsDuProjetDto> ImpactsDesProjets { get; set; } = new();
        public List<BailleursDeFondsDto> BailleursDeFonds { get; set; } = new();
        public List<ActivitesAnnuellesDto> ActivitesAnnuelles { get; set; } = new();
        public List<CoutAnnuelDuProjetDto> CoutAnnuelDuProjet { get; set; } = new();



        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
            {
                    // Clé = nom passé à GenererIdPour()
                 { "IdentificationProjetDto", "XXB-BBBB-XX-XBB" },
            };

    }
}
