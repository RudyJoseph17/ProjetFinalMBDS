using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpEtudesPrefaisabiliteService
    {
        Task AjouterAsync(GrilleDdpEtudesPrefaisabiliteDto grilleDEtudesPrefaisabilite);
        Task MettreAJourAsync(GrilleDdpEtudesPrefaisabiliteDto grilleDEtudesPrefaisabilite);
        Task SupprimerAsync(byte IdGrilleDdpEtudesPrefaisabilite);
        Task<List<GrilleDdpEtudesPrefaisabiliteDto>> ObtenirTousAsync();
        Task<GrilleDdpEtudesPrefaisabiliteDto?> ObtenirParIdAsync(byte id);
    }
}
