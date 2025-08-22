using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IAspectsJuridiquesService
    {
        Task AjouterAsync(AspectsJuridiquesDto aspectsJuridiques);
        Task MettreAJourAsync(AspectsJuridiquesDto aspectsJuridiques);
        Task SupprimerAsync(byte IdAspectsJuridiques);
        Task<List<AspectsJuridiquesDto>> ObtenirTousAsync();
        Task<AspectsJuridiquesDto?> ObtenirParIdAsync(byte id);
    }

}

