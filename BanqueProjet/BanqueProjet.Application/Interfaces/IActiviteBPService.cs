using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IActiviteBPService
    {
        Task AjouterAsync(ActiviteBPDto activiteBP);
        Task MettreAJourAsync(ActiviteBPDto activiteBP);
        Task SupprimerAsync(byte IdActivites);
        Task<List<ActiviteBPDto>> ObtenirTousAsync();
        Task<ActiviteBPDto?> ObtenirParIdAsync(byte id);
    }
}
