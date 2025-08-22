using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Application.Interfaces
{
    public interface ISectionInstitutionService
    {
        Task AddAsync(SectionInstitutionDto sectionInstitution);
        Task<IEnumerable<SectionInstitutionDto>> GetSectionInstitutionAsync();

    }
}
