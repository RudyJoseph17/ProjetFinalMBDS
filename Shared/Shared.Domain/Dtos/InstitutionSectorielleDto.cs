using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class InstitutionSectorielleDto
    {
        public int Idinstitution { get; set; }
        public string? Nominstitution { get; set; }
        public string? Sigleinstitution { get; set; }
        public string? Missioninstitution { get; set; }

        public List<AttributionsInstitutionDto> ListAttributions { get; set; } = new();

        public List<SectionInstitutionDto> ListSections { get; set; } = new();
    }
}
