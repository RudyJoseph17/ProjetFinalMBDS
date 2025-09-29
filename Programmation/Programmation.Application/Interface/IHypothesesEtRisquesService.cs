using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IHypothesesEtRisquesService
    {
        Task AjouterAsync(HypothesesEtRisquesDto cadreLogique);
        Task MettreAJourAsync(HypothesesEtRisquesDto cadreLogique);
        Task SupprimerAsync(int IdHypothesesEtRisques);
        Task GetNextIdAsync(int IdHypothesesEtRisques);
        Task<List<HypothesesEtRisquesDto>> ObtenirTousAsync();
        Task<byte> GetNextIdAsync();
        // remplace ou complète l’existant
        Task<HypothesesEtRisquesDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet);
    }
}
