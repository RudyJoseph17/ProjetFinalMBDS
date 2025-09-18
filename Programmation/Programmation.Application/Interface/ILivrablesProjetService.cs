using Programmation.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface ILivrablesProjetService
    {
        Task AjouterAsync(LivrablesProgrameProjetDto dto);
        Task MettreAJourAsync(LivrablesProgrameProjetDto dto);
        Task SupprimerAsync(byte idLivrablesProjet);

        Task<List<LivrablesProgrameProjetDto>> ObtenirTousAsync();
        Task<LivrablesProgrameProjetDto?> ObtenirParIdAsync(byte id);

        // Retourne tous les livrables associés à un projet
        Task<List<LivrablesProgrameProjetDto>> ObtenirParProjetAsync(string idProjet);
    }
}
