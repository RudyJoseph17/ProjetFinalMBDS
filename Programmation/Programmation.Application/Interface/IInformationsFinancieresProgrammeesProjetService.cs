using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IInformationsFinancieresProgrammeesProjetService
    {
        Task AjouterAsync(InformationsFinancieresProgrammeesProjetDto informationsFinancieresProgrammeesProjet);
        Task MettreAJourAsync(InformationsFinancieresProgrammeesProjetDto informationsFinancieresProgrammeesProjet);
        Task SupprimerAsync(byte IdInformationsFinancieresProjet);
        Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirTousAsync();
        Task<InformationsFinancieresProgrammeesProjetDto?> ObtenirParIdAsync(byte id);
    }
}
