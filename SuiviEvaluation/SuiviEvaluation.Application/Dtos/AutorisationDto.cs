using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class AutorisationDto
    {
        
        public byte? IdAutorisation { get; set; }
        public byte? IdActivites { get; set; }
        public long? ExerciceFiscaleAutorisation { get; set; }
        public string? MoisAutorisation { get; set; }
        public string? SourcesFinancementAutorisati { get; set; }
        public string? ArticleAutorise { get; set; }
        public string? AlineaAutorisee { get; set; }
        public decimal? MontantAutorisationSurAline { get; set; }
    }
}
