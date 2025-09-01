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
    public class AttributionsInstitutionDto
    {
        public int Idattribution { get; set; }
        public string? Idinstitution { get; set; }
        public string? DescriptionAttribution { get; set; }
      
    }
}
