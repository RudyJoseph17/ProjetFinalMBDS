using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IPrevisionActiviteAnnuelleService
    {
        Task AjouterAsync(PrevisionActiviteAnnuelleDto previsionActiviteAnnuelle);
        Task MettreAJourAsync(PrevisionActiviteAnnuelleDto previsionActiviteAnnuelle);
        Task SupprimerAsync(int IdActivitesAnnuelles);
        Task GetNextIdAsync(int IdActivitesAnnuelles);
        Task<List<PrevisionActiviteAnnuelleDto>> ObtenirTousAsync();
        Task<byte> GetNextIdAsync();
        // remplace ou complète l’existant
        Task<PrevisionActiviteAnnuelleDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet);
    }
}
