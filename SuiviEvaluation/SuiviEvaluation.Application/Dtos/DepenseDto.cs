using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class DepenseDto
    {
        public byte? IdDepenses { get; set; }
        public byte? IdActivites { get; set; }
        public byte? ExerciceFiscaleDepense { get; set; }
        public string? MoisDeDepense { get; set; }
        public string? SourceDeFinancementDepense { get; set; }
        public string? ArticleDepense { get; set; }
        public string? AlineaDepense { get; set; }
        public decimal? MontantDepenseSurAlinea { get; set; }
    }
}
