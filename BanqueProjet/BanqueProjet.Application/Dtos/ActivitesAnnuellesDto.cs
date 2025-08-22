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
    public class ActivitesAnnuellesDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public decimal IdActivitesAnnuelles { get; set; }
        public string? DescriptionActivite { get; set; }
        public decimal? CoutAnnuel { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

    }
}
