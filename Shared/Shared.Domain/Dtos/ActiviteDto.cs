using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class ActiviteDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public int IdActivites { get; set; }
        public byte? NumeroActivites { get; set; }
        public string? NomActivite { get; set; }
        public string? ResultatsAttendus { get; set; }

        public List<InformationsFinancieresProjetDto> InformationsFinancieres { get; set; } = new();
    }
}
