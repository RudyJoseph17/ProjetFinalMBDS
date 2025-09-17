using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IAutorisationSurProjetService
    {
        Task AjouterAsync(AutorisationSurProjetDto autorisationSurProjet);
        Task MettreAJourAsync(AutorisationSurProjetDto autorisationSurProjet);
        Task SupprimerAsync(int IdActivites);
        Task SupprimerProjetAsync(string IdIdentificationProjet);
        Task<List<AutorisationSurProjetDto>> ObtenirTousAsync();
        Task<AutorisationSurProjetDto?> ObtenirParIdAsync(string id);
        Task<AutorisationSurProjetDto?> ObtenirParIdActiviteAsync(int id);

    }
}
