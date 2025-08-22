using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IEmploisCreesService
    {
        Task AjouterAsync(EmploisCreesDto emploisCrees);
        Task MettreAJourAsync(EmploisCreesDto emploisCrees);
        Task SupprimerAsync(byte IdEmploisCrees);
        Task<List<EmploisCreesDto>> ObtenirTousAsync();
        Task<EmploisCreesDto?> ObtenirParIdAsync(byte id);
    }
}
