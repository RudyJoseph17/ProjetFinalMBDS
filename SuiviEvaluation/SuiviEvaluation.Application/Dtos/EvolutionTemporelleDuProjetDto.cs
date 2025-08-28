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
    public class EvolutionTemporelleDuProjetDto
    {
        public byte? IdEvolutionTemporelle { get; set; }
        public string? IdIdentificationProjet { get; set; }
        public DateTime? DateDeDemmarage { get; set; }
        public DateTime? DateAchevementPrevue { get; set; }
        public byte? TempsEcoule { get; set; }
        public byte? TempsRestant { get; set; }
    }
}
