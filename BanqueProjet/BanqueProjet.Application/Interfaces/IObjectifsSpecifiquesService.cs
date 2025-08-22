using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IObjectifsSpecifiquesService
    {
        Task AjouterAsync(ObjectifsSpecifiquesDto objectifsSpecifiques);
        Task MettreAJourAsync(ObjectifsSpecifiquesDto objectifsSpecifiques);
        Task SupprimerAsync(byte IdObjectifsSpecifiques);
        Task<List<ObjectifsSpecifiquesDto>> ObtenirTousAsync();
    }
}
