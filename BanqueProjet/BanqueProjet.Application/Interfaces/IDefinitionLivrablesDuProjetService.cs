using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IDefinitionLivrablesDuProjetService
    {
        Task AjouterAsync(DefinitionLivrablesDuProjetDto indicateursDeResultats);
        Task MettreAJourAsync(DefinitionLivrablesDuProjetDto indicateursDeResultats);
        Task SupprimerAsync(byte IdIndicateursDeResultats);
        Task<List<DefinitionLivrablesDuProjetDto>> ObtenirTousAsync();
    }
}
