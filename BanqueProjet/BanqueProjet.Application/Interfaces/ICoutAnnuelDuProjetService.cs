using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface ICoutAnnuelDuProjetService
    {
        Task AjouterAsync(CoutAnnuelDuProjetDto coutAnnuelDuProjet);
        Task MettreAJourAsync(CoutAnnuelDuProjetDto coutAnnuelDuProjet);
        Task SupprimerAsync(byte IdActivites);
        Task<List<CoutAnnuelDuProjetDto>> ObtenirTousAsync();
        Task<CoutAnnuelDuProjetDto?> ObtenirParIdAsync(byte id);
    }
}
