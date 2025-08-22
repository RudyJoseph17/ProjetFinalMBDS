using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IImpactsDuProjetService
    {
        Task AjouterAsync(ImpactsDuProjetDto impactsProjet);
        Task MettreAJourAsync(ImpactsDuProjetDto impactsProjet);
        Task SupprimerAsync(string IdImpactsProjet);
        Task<List<ImpactsDuProjetDto>> ObtenirTousAsync();
    }
}
