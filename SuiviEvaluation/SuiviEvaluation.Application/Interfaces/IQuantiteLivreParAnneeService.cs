using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IQuantiteLivreParAnneeService
    {
        Task AddAsync(QuantiteLivreParAnneeDto quantiteLivre);
        Task<IEnumerable<QuantiteLivreParAnneeDto>> GetAllAsync();
        //Task<IEnumerable<QuantiteLivreParAnneeDto>> GetQuantiteLivreParAneeAsync();
    }
}
