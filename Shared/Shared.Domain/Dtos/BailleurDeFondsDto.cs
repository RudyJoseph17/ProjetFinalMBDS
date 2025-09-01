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
    public class BailleurDeFondsDto
    {
        public int IdBailleur { get; set; }
        public string? NomBailleur { get; set; }
        public string? TypeBailleur { get; set; }
    }
}
