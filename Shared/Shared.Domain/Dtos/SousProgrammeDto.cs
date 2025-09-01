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
    public class SousProgrammeDto
    {
        public int Idsousprogramme { get; set; }
        public string? Nomsousprogramme { get; set; }
        public int Idprogramme { get; set; }
        public string? Nomprogramme { get; set; }
    }
}
