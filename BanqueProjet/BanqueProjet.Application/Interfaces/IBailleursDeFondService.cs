using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IBailleursDeFondService
    {
        Task AjouterAsync(BailleursDeFondsDto BailleursDeFond);
        Task MettreAJourAsync(BailleursDeFondsDto BailleursDeFond);
        Task SupprimerAsync(byte IdBailleursDeFonds);
        Task<List<BailleursDeFondsDto>> ObtenirTousAsync();
    }
}
