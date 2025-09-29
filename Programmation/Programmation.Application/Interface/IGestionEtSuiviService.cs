using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IGestionEtSuiviService
    {
        Task AjouterAsync(GestionEtSuiviProjetDto cadreLogique);
        Task MettreAJourAsync(GestionEtSuiviProjetDto cadreLogique);
        Task SupprimerAsync(int IdGestionEtSuiviProjetDto);
        Task GetNextIdAsync(int IdGestionEtSuiviProjet);
        Task<List<GestionEtSuiviProjetDto>> ObtenirTousAsync();
        Task<byte> GetNextIdAsync();
        // remplace ou complète l’existant
        Task<GestionEtSuiviProjetDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet);
    }
}
