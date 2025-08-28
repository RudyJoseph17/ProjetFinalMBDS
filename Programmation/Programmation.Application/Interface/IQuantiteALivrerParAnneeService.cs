using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IQuantiteALivrerParAnneeService
    {
        Task AjouterAsync(QuantiteALivrerParAnneeDto quantiteALivrerParAnnee);
        Task MettreAJourAsync(QuantiteALivrerParAnneeDto quantiteALivrerParAnnee);
        Task SupprimerAsync(byte IdLivrablesProjet);
        Task<List<QuantiteALivrerParAnneeDto>> ObtenirTousAsync();
        Task<QuantiteALivrerParAnneeDto?> ObtenirParIdAsync(byte id);
    }
}
