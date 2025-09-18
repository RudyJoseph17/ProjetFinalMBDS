using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BanqueProjet.Application.Converters;

namespace BanqueProjet.Application.Dtos
{
    public class GrilleDdpProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = null!;
        public byte IdGrilleDdpProjet { get; set; } 
        public string? TitreProjet { get; set; }
        public string? Ministere { get; set; }
        public DateTime? DateSoumission { get; set; }
        public DateTime? DateDebutAnalyse { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? TitreProjetAol { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? ProjetLienPsdh { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? HistoriqueDecrit { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? JustificationDemontree { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? ProjetObjectifClair { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? EffetsAttendusCoherents { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? PopulationViseeDecrite { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? LocalisationDecrite { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? DureeTotalProjetBienDefine { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? CoutTotalProjetBienDetermine { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? EmploisCreesIdentifies { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? FacteurGenrePrisEnCompte { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? EtudesSatisfaisantes { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? ActivitesEtResultatsDecrits { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? DureeActiviteDansGantt { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? CalendrierFinancierCorrespondGantt { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? CalculsDepensesExacts { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? DepensesPrevuesPermetActivites { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? DepensesProjetIncluses { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? SourcesFinancementIdentifiees { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? EntitesRolesClairementDefinis { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? StructureOrgaInclutEntites { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? ObjectifGeneralSpecifiqueDefinis { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? DetailsSuffisantsAspJuridiques { get; set; }

        [JsonConverter(typeof(OuiNonBoolConverter))]
        public bool? PassationDesMarchesRigoureux { get; set; }
        public string? CommentairesGeneraux { get; set; }
        public string? ResultatsAnalyse { get; set; }
        public string? Recommandations { get; set; }
        public string? Decision { get; set; }
        public DateTime? DateAvis { get; set; }
      
    }
}
