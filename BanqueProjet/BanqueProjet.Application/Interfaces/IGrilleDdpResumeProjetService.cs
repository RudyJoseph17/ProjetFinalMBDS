using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpResumeProjetService
    {
        Task AjouterAsync(GrilleDdpResumeProjetDto grilleDResumeProjet);
        Task MettreAJourAsync(GrilleDdpResumeProjetDto grilleDResumeProjet);
        Task SupprimerAsync(byte IdGrilleDdpResumeProjet);
        Task<List<GrilleDdpResumeProjetDto>> ObtenirTousAsync();
        Task<GrilleDdpResumeProjetDto?> ObtenirParIdAsync(byte id);
    }
}
