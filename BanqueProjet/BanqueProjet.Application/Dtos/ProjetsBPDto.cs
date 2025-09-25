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

        [JsonProperty("MINISTERE")]
        public string? Ministere { get; set; }

        [JsonProperty("SECTION")]
        public string? Section { get; set; }

        [JsonProperty("CODE_PIP")]
        public string CodePip { get; set; }

        [JsonProperty("CODE_BAILLEUR")]
        public string CodeBailleur { get; set; }

        [JsonProperty("JUSTIFICATION_PROJET")]
        public string JustificationProjet { get; set; }

        [JsonProperty("ETUDE_PREFAISABILITE")]
        public string EtudePrefaisabilite { get; set; }

        [JsonProperty("ETUDE_FAISABILITE")]
        public string EtudeFaisabilite { get; set; }

        [JsonProperty("POPULATION_VISEE")]
        public string PopulationVisee { get; set; }

        [JsonProperty("PROGRAMME")]
        public string? Programme { get; set; }

        [JsonProperty("SOUS_PROGRAMME")]
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

        [JsonProperty("SECTEUR_D_ACTIVITES")]
        public string? SecteurDActivites { get; set; }

        [JsonProperty("SOUS_SECTEUR_D_ACTIVITES")]
        public string? SousSecteurDActivites { get; set; }

        [JsonProperty("NOM_DIRECTEUR_DE_PROJET")]
        public string? NomDirecteurDeProjet { get; set; }

        [JsonProperty("TELEPHONE_DIRECTEUR_DE_PROJET")]
        public string? TelephoneDirecteurDeProjet { get; set; }

        [JsonProperty("COURRIEL_DIRECTEUR_DE_PROJET")]
        public string? CourrielDirecteurDeProjet { get; set; }

        [JsonProperty("OBJECTIF_GENERAL_PROJET")]
        public string? ObjectifGeneralProjet { get; set; }

        [JsonProperty("DUREE_PROJET")]
        public string? DureeProjet { get; set; }

        [JsonProperty("COUT_TOTAL_PROJET")]
        public decimal? CoutTotalProjet { get; set; }

        [JsonProperty("ECHELON_TERRITORIAL")]
        public EchelonTerritorial? EchelonTerritorial { get; set; }

        //[JsonProperty("MINISTERE")]
        public string? AvisProjet { get; set; } // "oui", "non", "Projet à analyser"

        //[JsonProperty("MINISTERE")]
        public byte? IdGrilleDdpProjet { get; set; } // Id de la grille si existante



        // Collections simplifiées ou DTOs correspondants
        [JsonProperty("listActvt")]
        public List<ActiviteBPDto> Activites { get; set; } = new ();

        [JsonProperty("Listaspjuridiques")]
        public List<AspectsJuridiquesDto> AspectsJuridiques { get; set; } = new();



        [JsonProperty("Listpartprenantes")]
         public List<PartiesPrenantesDto> PartiesPrenantes { get; set; } = new();

        [JsonProperty("listindresultat")]
        public List<IndicateursDeResultatDto> IndicateursDeResultats { get; set; } = new();

        [JsonProperty("listlivrprojet")]
        public List<DefinitionLivrablesDuProjetDto> LivrablesProjets { get; set; } = new();

        [JsonProperty("listeffetprojet")]
        public List<EffetsDuProjetDto> EffetsProjets { get; set; } = new();

        [JsonProperty("listObjspec")]
        public List<ObjectifsSpecifiquesDto> ObjectifsSpecifiques { get; set; } = new();

        [JsonProperty("listimpctprojet")]
        public List<ImpactsDuProjetDto> ImpactsDesProjets { get; set; } = new();

        [JsonProperty("Listbailfond")]
        public List<BailleursDeFondsDto> BailleursDeFonds { get; set; } = new();

        [JsonProperty("listActvann")]
        public List<ActivitesAnnuellesDto> ActivitesAnnuelles { get; set; } = new();

        [JsonProperty("listcoutannprojet")]
        public List<CoutAnnuelDuProjetDto> CoutAnnuelDuProjet { get; set; } = new();



        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
            {
                    // Clé = nom passé à GenererIdPour()
                 { "IdentificationProjetDto", "XXB-BBBB-XX-XBB" },
            };

    }
}
