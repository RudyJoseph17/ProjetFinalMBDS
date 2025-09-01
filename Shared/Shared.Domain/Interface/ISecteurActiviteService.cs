using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface ISecteurActiviteService
    {
        Task AjouterAsync(SecteurActiviteDto secteurActivite);
        Task MettreAJourAsync(SecteurActiviteDto secteurActivite);
        Task SupprimerAsync(int IdSecteurActivite);
        Task<List<SecteurActiviteDto>> ObtenirTousAsync();
        Task<SecteurActiviteDto?> ObtenirParIdAsync(int IdDepartement);
    }
}
