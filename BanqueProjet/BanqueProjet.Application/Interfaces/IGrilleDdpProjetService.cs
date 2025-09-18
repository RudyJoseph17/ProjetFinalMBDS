using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpProjetService
    {
        Task AjouterAsync(GrilleDdpProjetDto grilleDdpProjetDto);
        Task MettreAJourAsync(GrilleDdpProjetDto grilleDdpProjetDto);
        Task SupprimerAsync(byte IdAspectsJuridiques);
        Task<List<GrilleDdpProjetDto>> ObtenirTousAsync();
        Task<GrilleDdpProjetDto?> ObtenirParIdAsync(byte id);
        Task<GrilleDdpProjetDto?> ObtenirParProjetIdAsync(string IdIdentificationProjet);

    }
}
