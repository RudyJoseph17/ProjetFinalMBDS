using Programmation.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IInformationsFinancieresProgrammeesProjetService
    {
        Task AjouterAsync(InformationsFinancieresProgrammeesProjetDto dto);
        Task MettreAJourAsync(InformationsFinancieresProgrammeesProjetDto dto);
        Task SupprimerAsync(byte id);

        Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirTousAsync();
        Task<InformationsFinancieresProgrammeesProjetDto?> ObtenirParIdAsync(byte id);

        // Retourne toutes les infos financières programmées pour un projet
        Task<List<InformationsFinancieresProgrammeesProjetDto>> ObtenirParProjetAsync(string idProjet);
    }
}
