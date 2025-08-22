using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Application.Interfaces
{
    public interface IActiviteService
    {
        Task AjouterAsync(ActiviteDto activite);
        Task MettreAJourAsync(ActiviteDto activite);
        Task SupprimerAsync(string IdActivites);
        Task<List<ActiviteDto>> ObtenirTousAsync();
    }
}
