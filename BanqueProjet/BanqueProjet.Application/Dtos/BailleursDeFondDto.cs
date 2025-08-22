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
    public class BailleursDeFondDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdBailleursDeFonds { get; set; }
        public string? NomBailleur { get; set; }
        public string? NomRepresentant { get; set; }
        public decimal? TelephoneRepresentant { get; set; }
        public string? CourrielRepresentant { get; set; }
    }
}
