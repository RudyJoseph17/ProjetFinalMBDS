using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IPrevisionService
    {
        Task AjouterAsync(PrevisionDto prevision);
        Task MettreAJourAsync(PrevisionDto prevision);
        Task SupprimerAsync(byte IdPrevision);
        Task<List<PrevisionDto>> ObtenirTousAsync();
    }
}
