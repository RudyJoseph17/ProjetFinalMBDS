using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpAviService
    {
        Task AjouterAsync(GrilleDdpAviDto grilleDAvis);
        Task MettreAJourAsync(GrilleDdpAviDto grilleDdpAvis);
        Task SupprimerAsync(byte IdGrilleDdpAvis);
        Task<List<GrilleDdpAviDto>> ObtenirTousAsync();
        Task<GrilleDdpAviDto?> ObtenirParIdAsync(byte id);

    }
}
