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
        Task AjouterAsync(BailleursDeFondDto BailleursDeFond);
        Task MettreAJourAsync(BailleursDeFondDto BailleursDeFond);
        Task SupprimerAsync(string IdBailleursDeFonds);
        Task<List<BailleursDeFondDto>> ObtenirTousAsync();
    }
}
