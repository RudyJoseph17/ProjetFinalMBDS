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
    public class DecaissementDto
    {
        public byte? IdDecaissement { get; set; }
        public byte? IdActivites { get; set; }
        public long? ExerciceFiscaleDeDecaisseme { get; set; }
        public string? MoisDeDecaissement { get; set; }
        public string? SourcesDeFinancementDecais { get; set; }
        public string? ArticleDecaisse { get; set; }
        public string? AlineaDecaissee { get; set; }
        public decimal? MontantDecaissementSurAline { get; set; }
    }
}
