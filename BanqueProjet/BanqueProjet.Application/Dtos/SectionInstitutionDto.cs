using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Dtos
{
    public class SectionInstitutionDto
    {
        public long IdSectionInstitution { get; set; }

        public long? IdInstitutionSectorielle { get; set; }

        public string? NomSection { get; set; }
    }
}
