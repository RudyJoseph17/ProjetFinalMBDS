using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGrilleDdpCalendrierExecutionService
    {
        Task AjouterAsync(GrilleDdpCalendrierExecutionDto grilleDCalendrierExecution);
        Task MettreAJourAsync(GrilleDdpCalendrierExecutionDto grilleDCalendrierExecution);
        Task SupprimerAsync(byte IdGrilleDdpCalendrierExecution);
        Task<List<GrilleDdpCalendrierExecutionDto>> ObtenirTousAsync();
        Task<GrilleDdpCalendrierExecutionDto?> ObtenirParIdAsync(byte id);
    }
}
