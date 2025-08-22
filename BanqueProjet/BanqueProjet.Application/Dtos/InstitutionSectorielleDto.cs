using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Dtos
{
    public class InstitutionSectorielleDto
    {
    public string IdInstitutionSectorielle { get; set; } = null!;

    public string? NomInstitutionSectorielle { get; set; }

    public string? MissionInstitutionSectorielle { get; set; }

    public string? AttributionsInstitutionSectorielle { get; set; }
    }
}
