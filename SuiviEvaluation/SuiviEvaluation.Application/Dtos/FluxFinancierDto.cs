using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class FluxFinancierDto
    {
        public byte? IdActivites { get; set; }
        public decimal TotalAutorisation { get; set; }
        public decimal TotalDecaissement { get; set; }
        public decimal TotalDepense { get; set; }
    }
}
