using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Dtos
{
    public class ActiviteBPDto : ActiviteDto
    {
        public decimal? NumeroActivites { get; set; }
        public string? NomActivite { get; set; }
        public string? ResultatsAttendus { get; set; }

        public List<ActivitesAnnuellesDto> ActivitesAnnuelles { get; set; } = new();
        public List<InformationsFinancieresBPDto> InformationsFinancieresProjet { get; set; } = new();
    }
}
