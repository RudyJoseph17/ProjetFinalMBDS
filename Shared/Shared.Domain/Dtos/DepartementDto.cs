using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class DepartementDto
    {
        public int IdDepartement { get; set; }
        public string? NomDepartement { get; set; }
        public List<ArrondissementDto> ListArrondissements { get; set; } = new();
    }
}
