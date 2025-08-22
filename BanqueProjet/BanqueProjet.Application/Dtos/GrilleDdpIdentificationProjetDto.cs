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
    public class GrilleDdpIdentificationProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdGrilleDdpIdentificationProjet { get; set; }
        public string? TitreProjet { get; set; }
        public string? Ministere { get; set; }
        public DateTime? DateSoumission { get; set; }
        public DateTime? DateDebutAnalyse { get; set; }
    }
}
