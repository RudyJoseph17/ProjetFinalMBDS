using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IDecaissementSurProjetService
    {
        Task AjouterAsync(DecaissementSurProjetDto decaissementSurProjet);
        Task MettreAJourAsync(DecaissementSurProjetDto decaissementSurProjet);
        Task SupprimerAsync(int IdActivites);
        Task SupprimerProjetAsync(string IdIdentificationProjet);
        Task<List<DecaissementSurProjetDto>> ObtenirTousAsync();
        Task<DecaissementSurProjetDto?> ObtenirParIdAsync(string id);
        Task<DecaissementSurProjetDto?> ObtenirParIdActiviteAsync(int id);
    }
}
