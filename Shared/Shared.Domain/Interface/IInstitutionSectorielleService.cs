using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IInstitutionSectorielleService
    {
        Task AjouterAsync(InstitutionSectorielleDto institutionSectorielle);
        Task MettreAJourAsync(InstitutionSectorielleDto institutionSectorielle);
        Task SupprimerAsync(int Idinstitution);
        Task<List<InstitutionSectorielleDto>> ObtenirTousAsync();
        Task<InstitutionSectorielleDto?> ObtenirParIdAsync(int id);
    }
}
