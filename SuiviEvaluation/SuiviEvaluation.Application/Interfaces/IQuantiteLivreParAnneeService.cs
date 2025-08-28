using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface IQuantiteLivreParAnneeService
    {
        Task AjouterAsync(QuantiteLivreeParAnneeDto quantiteLivreParAnnee);
        Task MettreAJourAsync(QuantiteLivreeParAnneeDto quantiteLivreParAnnee);
        Task SupprimerAsync(byte IdLivrablesProjet);
        Task<List<QuantiteLivreeParAnneeDto>> ObtenirTousAsync();
        Task<QuantiteLivreeParAnneeDto?> ObtenirParIdAsync(byte id);
    }
}
