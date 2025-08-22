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
    public class GrilleDdpAspectsLegauxDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdGrilleDdpAspectsLegauxProjet { get; set; }
        public bool? DetailsSuffisantsAspJuridiques { get; set; }
        public bool? PassationDesMarchesRigoureux { get; set; }
    }
}
