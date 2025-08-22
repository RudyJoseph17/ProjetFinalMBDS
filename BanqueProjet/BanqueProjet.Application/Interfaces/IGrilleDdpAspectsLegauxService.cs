using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpAspectsLegauxService
    {
        Task AjouterAsync(GrilleDdpAspectsLegauxDto grilleDApectsLegaux);
        Task MettreAJourAsync(GrilleDdpAspectsLegauxDto grilleDApectsLegaux);
        Task SupprimerAsync(byte IdGrilleDdpAspectsLegauxProjet);
        Task<List<GrilleDdpAspectsLegauxDto>> ObtenirTousAsync();
        Task<GrilleDdpAspectsLegauxDto?> ObtenirParIdAsync(byte id);
    }
}
