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
        public List<SousSecteurActiviteDto> ListSousSecteurActivite { get; set; } = new();
    }
}
