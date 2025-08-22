using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpIdentificationProjetService
    {
        Task AjouterAsync(GrilleDdpIdentificationProjetDto grilleDIdentificationProjet);
        Task MettreAJourAsync(GrilleDdpIdentificationProjetDto grilleDIdentificationProjet);
        Task SupprimerAsync(byte IdGrilleDdpIdentificationProjet);
        Task<List<GrilleDdpIdentificationProjetDto>> ObtenirTousAsync();
        Task<GrilleDdpIdentificationProjetDto?> ObtenirParIdAsync(byte id);
    }
}
