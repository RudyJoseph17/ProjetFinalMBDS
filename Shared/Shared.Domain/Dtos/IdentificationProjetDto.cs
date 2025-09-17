using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Domain.Dtos
{
    public class IdentificationProjetDto
    {
        [JsonProperty("ID_IDENTIFICATION_PROJET")]
        public string IdIdentificationProjet { get; set; }

        [JsonProperty("NOM_PROJET")]
        public string? NomProjet { get; set; }
        public string? TypeDeProjet { get; set; }
        public string? SecteurDActivites { get; set; }
        public string? SousSecteurDActivites { get; set; }
        public string? Ministere { get; set; }
        public string? NomDirecteurDeProjet { get; set; }
        public string? TelephoneDirecteurDeProjet { get; set; }
        public string? CourrielDirecteurDeProjet { get; set; }
        public string? Section { get; set; }
        public string? CodePip { get; set; }
        public string? CodeBailleur { get; set; }
        public string? JustificationProjet { get; set; }
        public string? EtudePrefaisabilite { get; set; }
        public string? EtudeFaisabilite { get; set; }
        public string? ObjectifGeneralProjet { get; set; }
        public string? DureeProjet { get; set; }
        public string? PopulationVisee { get; set; }
        public decimal? CoutTotalProjet { get; set; }
        public string? EchelonTerritorial { get; set; }
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

                // Collections simplifiées ou DTOs correspondants
        public List<ActiviteDto> Activites { get; set; } = new ();
        public List<LivrablesDuProjetDto> LivrablesProjets { get; set; } = new();
        public List<InformationsFinancieresProjetDto> InformationsFinancieresDuProjet { get; set; } = new();

        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
            {
                    // Clé = nom passé à GenererIdPour()
                 { "IdentificationProjetDto", "XXB-BBBB-XX-XBB" },
            };
    }
}
