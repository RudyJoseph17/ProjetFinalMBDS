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
    public class GrilleDdpResumeProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdGrilleDdpResumeProjet { get; set; }
        public bool? TitreProjetAol { get; set; }
        public bool? ProjetLienPsdh { get; set; }
        public bool? HistoriqueDecrit { get; set; }
        public bool? JustificationDemontree { get; set; }
        public bool? ProjetObjectifClair { get; set; }
        public bool? EffetsAttendusCoherents { get; set; }
        public bool? PopulationViseeDecrite { get; set; }
        public bool? LocalisationDecrite { get; set; }
        public bool? DureeTotalProjetBienDefine { get; set; }
        public bool? CoutTotalProjetBienDetermine { get; set; }
        public bool? EmploisCreesIdentifies { get; set; }
        public bool? FacteurGenrePrisEnCompte { get; set; }
    }
}
