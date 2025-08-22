using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpCommentairesGenerauxService
    {
        Task AjouterAsync(GrilleDdpCommentairesGenerauxDto grilleDCommentairesGeneraux);
        Task MettreAJourAsync(GrilleDdpCommentairesGenerauxDto grilleDCommentairesGeneraux);
        Task SupprimerAsync(byte IdGrilleDdpCommentairesGeneraux);
        Task<List<GrilleDdpCommentairesGenerauxDto>> ObtenirTousAsync();
        Task<GrilleDdpCommentairesGenerauxDto?> ObtenirParIdAsync(byte id);
    }
}
