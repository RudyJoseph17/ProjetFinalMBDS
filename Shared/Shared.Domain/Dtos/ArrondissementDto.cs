using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class ArrondissementDto
    {
        public int IdArrondissement { get; set; }
        public string? NomArrondissement { get; set; }
        public List<CommuneDto> ListCommunes { get; set; } = new();
    }
}

