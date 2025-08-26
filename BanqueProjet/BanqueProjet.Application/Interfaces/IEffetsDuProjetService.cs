using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IEffetsDuProjetService
    {
        Task AjouterAsync(EffetsDuProjetDto effetsDuProjet);
        Task MettreAJourAsync(EffetsDuProjetDto effetsDuProjet);
        Task SupprimerAsync(byte IdEffetsDuProjet);
        Task<List<EffetsDuProjetDto>> ObtenirTousAsync();
    }
}
