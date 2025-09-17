using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Dtos
{
    public class LivrablesProgrameProjetDto : LivrablesDuProjetDto
    {
        public int? QuantiteALivrer { get; set; }
    }
}
