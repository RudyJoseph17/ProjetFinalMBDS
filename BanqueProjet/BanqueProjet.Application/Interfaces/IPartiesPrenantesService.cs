using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IPartiesPrenantesService
    {
        Task AjouterAsync(PartiesPrenantesDto partiesPrenantes);
        Task MettreAJourAsync(PartiesPrenantesDto partiesPrenantes);
        Task SupprimerAsync(byte IdPartiesPrenantes);
        Task<List<PartiesPrenantesDto>> ObtenirTousAsync();
    }
}
