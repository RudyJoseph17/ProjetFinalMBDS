using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class SousSecteurActiviteDto
    {
        public int IdSousSecteurActivite { get; set; }
        public string? NomSousSecteurActivite { get; set; }
        public int IdSecteurActivite { get; set; }
    }
}
