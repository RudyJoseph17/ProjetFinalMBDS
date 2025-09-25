using BanqueProjet.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IAspectsJuridiquesService
    {
        Task AjouterAsync(AspectsJuridiquesDto aspectsJuridiques);
        Task MettreAJourAsync(AspectsJuridiquesDto aspectsJuridiques);
        Task SupprimerAsync(byte idAspectsJuridiques);

        Task<IEnumerable<AspectsJuridiquesDto>> ObtenirTousAsync();
        Task<AspectsJuridiquesDto?> ObtenirParIdAsync(byte id);
        Task SupprimerAsync(int idAspectsJuridiques);
    }
}
