using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Dtos;

namespace Shared.Domain.Interface
{
    public interface ILivrablesDuProjet
    {
        Task AjouterAsync(LivrablesDuProjetDto activitesAnnuelles);
        Task MettreAJourAsync(LivrablesDuProjetDto activitesAnnuelles);
        Task SupprimerAsync(byte IdLivrablesProjet);
        Task<List<LivrablesDuProjetDto>> ObtenirTousAsync();
        Task<LivrablesDuProjetDto?> ObtenirParIdAsync(byte id);
    }
}
