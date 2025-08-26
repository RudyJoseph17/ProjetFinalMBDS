using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IActiviteService
    {
        Task AjouterAsync(ActiviteDto activitesAnnuelles);
        Task MettreAJourAsync(ActiviteDto activitesAnnuelles);
        Task SupprimerAsync(byte IdActivites);
        Task<List<ActiviteDto>> ObtenirTousAsync();
        Task<ActiviteDto?> ObtenirParIdAsync(byte id);
    }
}
