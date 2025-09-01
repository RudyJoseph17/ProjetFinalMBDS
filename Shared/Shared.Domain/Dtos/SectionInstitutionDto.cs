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
    public class SectionInstitutionDto
    {
        public int IdSection { get; set; }
        public int IdInstitution { get; set; }
        public string? NomSection { get; set; }
        public string? SigleSection { get; set; }
        public string? AdresseSection { get; set; }
 
    }
}
