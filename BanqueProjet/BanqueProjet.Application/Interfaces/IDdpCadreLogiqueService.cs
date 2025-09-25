using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IDdpCadreLogiqueService
    {
        Task AjouterAsync(DdpCadreLogiqueDto cadreLogique);
        Task MettreAJourAsync(DdpCadreLogiqueDto cadreLogique);
        Task SupprimerAsync(byte IdDdpCadreLogique);
        Task GetNextIdAsync(byte IdDdpCadreLogique);
        Task<List<DdpCadreLogiqueDto>> ObtenirTousAsync();
        Task<byte> GetNextIdAsync();
        // remplace ou complète l’existant
        Task<DdpCadreLogiqueDto?> ObtenirParIdentificationProjetAsync(string idIdentificationProjet);
    }
}

