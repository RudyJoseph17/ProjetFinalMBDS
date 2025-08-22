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
    public class CoutAnnuelDuProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public decimal IdCoutAnnuelProjet { get; set; }
        public byte? ExerciceFiscaleDebut { get; set; }
        public byte? ExerciceFiscaleFin { get; set; }
        public string? SourcesDeFinancementCoutAn { get; set; }
        public decimal? MontantAnnuel { get; set; }
    }
}
