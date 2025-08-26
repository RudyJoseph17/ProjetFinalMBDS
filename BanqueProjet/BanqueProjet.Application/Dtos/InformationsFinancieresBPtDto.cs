using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Dtos;

namespace BanqueProjet.Application.Dtos
{
    public class InformationsFinancieresBPDto: InformationsFinancieresProjetDto
    {
        public int IdActivites { get; set; }
        public int? IdInformationsFinancieres { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? SourcesFinancement { get; set; }
        public decimal? MontantPrevu { get; set; }
    }
}
