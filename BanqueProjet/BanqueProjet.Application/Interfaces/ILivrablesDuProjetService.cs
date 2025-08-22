using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface ILivrablesDuProjetService
    {
        Task AjouterAsync(LivrablesDuProjetDto indicateursDeResultats);
        Task MettreAJourAsync(LivrablesDuProjetDto indicateursDeResultats);
        Task SupprimerAsync(byte IdIndicateursDeResultats);
        Task<List<LivrablesDuProjetDto>> ObtenirTousAsync();
    }
}
