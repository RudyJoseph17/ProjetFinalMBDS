using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpStrategieGestionProjetService
    {
        Task AjouterAsync(GrilleDdpStrategieGestionProjetDto grilleDStrategieGestionProje);
        Task MettreAJourAsync(GrilleDdpStrategieGestionProjetDto grilleDStrategieGestionProje);
        Task SupprimerAsync(byte IdGrilleDdpStrategieGestionProjet);
        Task<List<GrilleDdpStrategieGestionProjetDto>> ObtenirTousAsync();
        Task<GrilleDdpStrategieGestionProjetDto?> ObtenirParIdAsync(byte id);
    }
}
