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
    public class GrilleDdpCalendrierFinancierProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdGrilleDdpCalendrierFinancierProjet { get; set; }
        public bool? CalendrierFinancierCorrespondGantt { get; set; }
        public bool? CalculsDepensesExacts { get; set; }
        public bool? DepensesPrevuesPermetActivites { get; set; }
        public bool? DepensesProjetIncluses { get; set; }
        public bool? SourcesFinancementIdentifiees { get; set; }
    }
}
