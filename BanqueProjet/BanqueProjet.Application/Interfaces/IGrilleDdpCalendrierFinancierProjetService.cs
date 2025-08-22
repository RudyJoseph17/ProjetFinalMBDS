using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpCalendrierFinancierProjetService
    {
        Task AjouterAsync(GrilleDdpCalendrierFinancierProjetDto grilleDCalendrierFinancier);
        Task MettreAJourAsync(GrilleDdpCalendrierFinancierProjetDto grilleDCalendrierFinancier);
        Task SupprimerAsync(byte IdGrilleDdpCalendrierFinancierProjet);
        Task<List<GrilleDdpCalendrierFinancierProjetDto>> ObtenirTousAsync();
        Task<GrilleDdpCalendrierFinancierProjetDto?> ObtenirParIdAsync(byte id);
    }
}
