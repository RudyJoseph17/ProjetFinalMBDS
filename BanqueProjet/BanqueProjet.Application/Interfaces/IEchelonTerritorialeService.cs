using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IEchelonTerritorialeService
    {
        Task AjouterAsync(EchelonTerritorialeDto echelonTerritoriale);
        Task MettreAJourAsync(EchelonTerritorialeDto echelonTerritoriale);
        Task SupprimerAsync(byte IdEchelonTerritoriale);
        Task<List<EchelonTerritorialeDto>> ObtenirTousAsync();
        Task<EchelonTerritorialeDto?> ObtenirParIdAsync(byte id);
    }
}
