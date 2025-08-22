using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SuiviEvaluation.Application.Interfaces
{
    public interface IFluxFinancierService
    {
        Task AddAsync(FluxFinancierDto flux);
        Task<IEnumerable<FluxFinancierDto>> GetFluxFinancierAsync();
    }
}
