using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IInstitutionSectorielleService
    {
        Task AddAsync(InstitutionSectorielleDto institutionSectorielle);
        Task<IEnumerable<InstitutionSectorielleDto>> GetInstitutionSectorielleAsync();
    }
}
