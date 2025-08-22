using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IPhaseDuProjetService
    {
        Task AjouterAsync(PhaseDuProjetDto phaseDuProjet);
        Task MettreAJourAsync(PhaseDuProjetDto phaseDuProjet);
        Task SupprimerAsync(byte IdPhaseDuProjet);
        Task<List<PhaseDuProjetDto>> ObtenirTousAsync();
    }
}
