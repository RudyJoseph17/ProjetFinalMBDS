using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class LivrablesDuProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public byte IdLivrablesProjet { get; set; }
        public string? DefinitionLivrablesDuProjet { get; set; }
        public int? QuantiteALivrer { get; set; }
        public int? QuantiteLivree { get; set; }
        public int? ValeurLivree { get; set; }
    }
}
