using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IGestionDeProjetService
    {
        Task AjouterAsync(GestionDeProjetDto gestionDeProjet);
        Task MettreAJourAsync(GestionDeProjetDto gestionDeProjet);
        Task SupprimerAsync(byte IdGestionDeProjet);
        Task<List<GestionDeProjetDto>> ObtenirTousAsync();
        Task<GestionDeProjetDto?> ObtenirParIdAsync(byte id);
    }
}
