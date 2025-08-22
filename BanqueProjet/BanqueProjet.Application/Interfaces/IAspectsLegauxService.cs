using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IAspectsLegauxService
    {
        Task AjouterAsync(AspectsJuridiquesDto aspectsLegaux);
        Task MettreAJourAsync(AspectsJuridiquesDto aspectsLegaux);
        Task SupprimerAsync(byte IdAspectsLegaux);
        Task<List<AspectsJuridiquesDto>> ObtenirTousAsync();
    }
}
