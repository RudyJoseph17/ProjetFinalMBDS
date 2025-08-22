using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IDecaissementService
    {
        Task AddAsync(DecaissementDto decaissement);
        //Task<IEnumerable<object>> GetAllAsync();
        Task<IEnumerable<DecaissementDto>> GetDecaissementAsync();
    }
}
