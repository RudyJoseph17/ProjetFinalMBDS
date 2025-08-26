using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IIdentificationProjetService
    {
        Task AjouterAsync(IdentificationProjetDto identificationProjet);
        Task MettreAJourAsync(IdentificationProjetDto identificationProjet);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<IdentificationProjetDto>> ObtenirTousAsync();
        Task<IdentificationProjetDto?> ObtenirParIdAsync(string id);
    }
}
