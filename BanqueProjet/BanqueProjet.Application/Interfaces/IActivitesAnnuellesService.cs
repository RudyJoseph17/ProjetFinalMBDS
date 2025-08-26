using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IActivitesAnnuellesService
    {
        Task AjouterAsync(ActivitesAnnuellesDto activitesAnnuelles);
        Task MettreAJourAsync(ActivitesAnnuellesDto activitesAnnuelles);
        Task SupprimerAsync(byte IdActivitesAnnuelles);
        Task<List<ActivitesAnnuellesDto>> ObtenirTousAsync();
        Task<ActivitesAnnuellesDto?> ObtenirParIdAsync(byte id);
    }
}
