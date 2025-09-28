using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class CommuneDto
    {
        public int IdCommune { get; set; }
        public string? NomCommune { get; set; }

        [JsonProperty("ListSections")]
        public List<SectionCommunaleDto> ListSections { get; set; } = new();

        // pour passer l’ID de l’arrondissement parent
        [JsonProperty("ParentArr")]
        public int ParentArr { get; set; }
    }
}

