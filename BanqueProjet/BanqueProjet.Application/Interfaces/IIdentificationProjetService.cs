using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Application.Interfaces
{
    public interface IIdentificationProjetService
    {
        // Récupère tous les projets
        Task AjouterAsync(IdentificationProjetDto projet);
        Task MettreAJourAsync(IdentificationProjetDto projet);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<IdentificationProjetDto>> ObtenirTousAsync();
        Task<IdentificationProjetDto?> ObtenirParIdAsync(string id);
    }
}
