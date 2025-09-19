using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface ILocalisationGeographiqueProjService
    {
        Task AjouterAsync(LocalisationGeographiqueProjDto localisationGeographique);
        Task MettreAJourAsync(LocalisationGeographiqueProjDto localisationGeographique);
        Task SupprimerAsync(byte IdLocalisationGeographique);
        Task<List<LocalisationGeographiqueProjDto>> ObtenirTousAsync();
        Task<LocalisationGeographiqueProjDto?> ObtenirParIdAsync(byte id);
        //Task<LocalisationGeographiqueProjDto> ObtenirParIdAsync(string id);
    }
}
