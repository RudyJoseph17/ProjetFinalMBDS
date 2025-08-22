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
    public class PartiesPrenantesDto
    {


        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdPartiesPrenantes { get; set; }
        public string? NomFirme { get; set; }
        public decimal? TelephoneFirme { get; set; }
        public string? CourrielFirme { get; set; }
        public string? RoleFirme { get; set; }

    }
}
