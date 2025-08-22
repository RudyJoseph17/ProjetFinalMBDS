using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface ISuiviEtControleService
    {
        Task AjouterAsync(SuiviEtControleDto suiviEtControle);
        Task MettreAJourAsync(SuiviEtControleDto suiviEtControle);
        Task SupprimerAsync(byte IdSuiviEtControle);
        Task<List<SuiviEtControleDto>> ObtenirTousAsync();
        Task<SuiviEtControleDto?> ObtenirParIdAsync(byte id);
    }
}
