using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IBailleurDeFondsService
    {
        Task AjouterAsync(BailleurDeFondsDto bailleur);
        Task MettreAJourAsync(BailleurDeFondsDto bailleur);
        Task SupprimerAsync(int Idbailleur);
        Task<List<BailleurDeFondsDto>> ObtenirTousAsync();
        Task<BailleurDeFondsDto?> ObtenirParIdAsync(int id);
    }
}
