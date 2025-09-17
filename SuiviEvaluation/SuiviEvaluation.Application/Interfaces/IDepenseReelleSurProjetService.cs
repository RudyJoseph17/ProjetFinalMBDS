using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IDepenseReelleSurProjetService
    {
        Task AjouterAsync(DepenseReelleSurProjetDto autorisationSurProjet);
        Task MettreAJourAsync(DepenseReelleSurProjetDto autorisationSurProjet);
        Task SupprimerAsync(int IdActivites);
        Task SupprimerProjetAsync(string IdIdentificationProjet);
        Task<List<DepenseReelleSurProjetDto>> ObtenirTousAsync();
        Task<DepenseReelleSurProjetDto?> ObtenirParIdAsync(string id);
        Task<DepenseReelleSurProjetDto?> ObtenirParIdActiviteAsync(int id);
    }
}
