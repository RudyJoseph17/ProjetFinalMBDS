using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuiviEvaluation.Application.Dtos;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IEvolutionTemporelleService
    {
        Task AjouterAsync(EvolutionTemporelleDuProjetDto evolutionProjet);
        Task MettreAJourAsync(EvolutionTemporelleDuProjetDto evolutionProjet);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<EvolutionTemporelleDuProjetDto>> ObtenirTousAsync();
        Task<EvolutionTemporelleDuProjetDto?> ObtenirParIdAsync(string id);
    }
}
