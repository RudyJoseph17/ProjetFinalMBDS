using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class FluxFinancierDto
    {
        public string IdIdentificationProjet { get; set; }
        public DateOnly ExerciceFiscalDebut { get; set; }
        public DateOnly ExerciceFiscalFin { get; set; }
        public decimal Totalprévu { get; set; }
        public decimal TotalAutorisation { get; set; }
        public decimal TotalDecaissement { get; set; }
        public decimal TotalDepense { get; set; }
        public decimal TauxDeMobilisation  { get; set; }
        public decimal TauxDExecution { get; set; }
    }
}
