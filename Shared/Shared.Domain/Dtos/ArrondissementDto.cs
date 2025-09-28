using Newtonsoft.Json;
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

        [JsonProperty("ListCommunes")]
        public List<CommuneDto> ListCommunes { get; set; } = new();

        [JsonProperty("ParentDepartement")]
        public int ParentDepartement { get; set; }
    }
}

