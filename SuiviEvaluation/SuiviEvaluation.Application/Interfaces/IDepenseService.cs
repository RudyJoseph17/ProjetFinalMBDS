using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IDepenseService
    {
        Task AddAsync(DepenseDto depense);
        //Task<IEnumerable<object>> GetAllAsync();
        Task<IEnumerable<DepenseDto>> GetDepenseAsync();
    }
}
