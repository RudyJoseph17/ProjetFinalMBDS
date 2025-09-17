using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
   public interface ISuiviProjetService
    {
        Task AjouterAsync(SuiviProjetDto suiviProjetDto);
        Task MettreAJourAsync(SuiviProjetDto suiviProjetDto);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<SuiviProjetDto>> ObtenirTousAsync();
        Task<SuiviProjetDto?> ObtenirParIdAsync(string id);
    }
}
