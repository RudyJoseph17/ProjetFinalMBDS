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
    public class GrilleDdpStrategieGestionProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdGrilleDdpStrategieGestionProjet { get; set; }
        public bool? EntitesRolesClairementDefinis { get; set; }
        public bool? StructureOrgaInclutEntites { get; set; }
        public bool? ObjectifGeneralSpecifiqueDefinis { get; set; }
    }
}
