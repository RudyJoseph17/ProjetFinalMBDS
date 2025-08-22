using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class QuantiteLivreParAnneeDto
    {
        public byte? IdQuantiteLivreeParAnnee { get; set; }
        public byte? AnneeQuantiteLivree { get; set; }
        public int? ValeurLivree { get; set; }
    }
}
