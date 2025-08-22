using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuiviEvaluation.Application.Dtos;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IEvolutionTemporelleService
    {
        Task AddAsync(EvolutionTemporelleDuProjetDto evolution);
        Task<IEnumerable<EvolutionTemporelleDuProjetDto>> GetEvolutionCouranteAsync();
    }
}
