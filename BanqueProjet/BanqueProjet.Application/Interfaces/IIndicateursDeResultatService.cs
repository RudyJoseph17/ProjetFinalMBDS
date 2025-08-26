using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IIndicateursDeResultatService
    {
        Task AjouterAsync(IndicateursDeResultatDto indicateursDeResultats);
        Task MettreAJourAsync(IndicateursDeResultatDto indicateursDeResultats);
        Task SupprimerAsync(byte IdIndicateursDeResultats);
        Task<List<IndicateursDeResultatDto>> ObtenirTousAsync();
    }
}
