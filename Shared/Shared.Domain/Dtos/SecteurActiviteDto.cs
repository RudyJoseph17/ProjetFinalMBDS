using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class SecteurActiviteDto
    {
        public int IdSecteurActivite { get; set; }
        public string? NomSecteurActivite { get; set; }

        // default initialiser + constructor pour garantir non-null
        [JsonProperty("listSousSecteurActivite")]
        public List<SousSecteurActiviteDto> ListSousSecteurActivite { get; set; }

        public SecteurActiviteDto()
        {
            ListSousSecteurActivite = new List<SousSecteurActiviteDto>();
        }
    }
}
