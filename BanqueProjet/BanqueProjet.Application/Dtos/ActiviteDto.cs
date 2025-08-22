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
    public class ActiviteDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdActivites { get; set; }
        public decimal? NumeroActivites { get; set; }
        public string? NomActivite { get; set; }
        public string? ResultatsAttendus { get; set; }

        public List<ActivitesAnnuellesDto> ActivitesAnnuelles { get; set; } = new();
    }
}
