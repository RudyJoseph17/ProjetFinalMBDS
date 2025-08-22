using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IDureeProjetService
    {
        Task AjouterAsync(DureeProjetDto dureeProjet);
        Task MettreAJourAsync(DureeProjetDto dureeProjet);
        Task SupprimerAsync(byte IdDureeProjet);
        Task<List<DureeProjetDto>> ObtenirTousAsync();
        Task<DureeProjetDto?> ObtenirParIdAsync(byte id);
    }
}
