using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface ILivrablesProjetService
    {
        Task AjouterAsync(LivrablesProgrameProjetDto livrablesProgrameProjetService);
        Task MettreAJourAsync(LivrablesProgrameProjetDto livrablesProgrameProjetService);
        Task SupprimerAsync(byte IdLivrablesProjet);
        Task<List<LivrablesProgrameProjetDto>> ObtenirTousAsync();
        Task<LivrablesProgrameProjetDto?> ObtenirParIdAsync(byte id);
    }
}
