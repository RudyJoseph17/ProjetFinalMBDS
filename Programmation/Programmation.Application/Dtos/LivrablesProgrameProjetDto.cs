using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Dtos
{
    public class LivrablesProgrameProjetDto : LivrablesDuProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }

        [NotMapped]
        public byte? ExerciceFiscalDebut { get; set; }

        [NotMapped]
        public byte? ExerciceFiscalFin { get; set; }
        public int? QuantiteALivrer { get; set; }
    }
}
