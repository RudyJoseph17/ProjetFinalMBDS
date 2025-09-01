using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
   public interface IDepartementService
    {
        Task AjouterAsync(DepartementDto departement);
        Task MettreAJourAsync(DepartementDto departement);
        Task SupprimerAsync(int IdDepartement);
        Task<List<DepartementDto>> ObtenirTousAsync();
        Task<DepartementDto?> ObtenirParIdAsync(int IdDepartement);
    }
}
