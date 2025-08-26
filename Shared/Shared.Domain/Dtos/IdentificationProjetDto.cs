using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Domain.Dtos
{
    public class IdentificationProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        public string? NomProjet { get; set; }



        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
            {
                    // Clé = nom passé à GenererIdPour()
                 { "IdentificationProjetDto", "XXB-BBBB-XX-XBB" },
            };
    }
}
