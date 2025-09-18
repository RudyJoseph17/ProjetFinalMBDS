using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IProgrammationProjetService
    {
        Task AjouterAsync(ProgrammationProjetDto programmationProjetDto);
        Task MettreAJourAsync(ProgrammationProjetDto programmationProjetDto);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<ProgrammationProjetDto>> ObtenirTousAsync();
        Task<ProgrammationProjetDto?> ObtenirParIdAsync(string id);
    }
}
