using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface ILivrablesRealisesProjetService
    {
        Task AjouterAsync(LivrablesRealisesProjetDto autorisationSurProjet);
        Task MettreAJourAsync(LivrablesRealisesProjetDto autorisationSurProjet);
        Task SupprimerProjetAsync(string IdIdentificationProjet);
        Task<List<LivrablesRealisesProjetDto>> ObtenirTousAsync();
        Task<LivrablesRealisesProjetDto?> ObtenirParIdAsync(string id);
    }
}
