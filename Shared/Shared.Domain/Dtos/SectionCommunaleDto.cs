using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class SectionCommunaleDto
    {
        public int IdSectionCommunale { get; set; }
        public string? NomSectionCommunale { get; set; }

        // pour passer l’ID de la commune parent
        [JsonProperty("ParentCommune")]
        public int ParentCommune { get; set; }
    }
}
