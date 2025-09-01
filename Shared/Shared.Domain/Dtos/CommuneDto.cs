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
        public List<SectionCommunaleDto> ListSections { get; set; } = new();
    }
}

