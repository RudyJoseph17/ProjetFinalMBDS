using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Dtos;

namespace Shared.Domain.Interface
{
    public interface IInformationsFinancieresService
    {
        Task AjouterAsync(InformationsFinancieresProjetDto activitesAnnuelles);
        Task MettreAJourAsync(InformationsFinancieresProjetDto activitesAnnuelles);
        Task SupprimerAsync(byte IdInformationsFinancieres);
        Task<List<InformationsFinancieresProjetDto>> ObtenirTousAsync();
        Task<InformationsFinancieresProjetDto?> ObtenirParIdAsync(byte id);
    }
}
