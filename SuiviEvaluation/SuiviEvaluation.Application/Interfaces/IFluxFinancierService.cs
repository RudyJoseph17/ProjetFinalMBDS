using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SuiviEvaluation.Application.Interfaces
{
    public interface IFluxFinancierService
    {
        Task AjouterAsync(FluxFinancierDto fluxFinancier);
        Task MettreAJourAsync(FluxFinancierDto fluxFinancier);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<FluxFinancierDto>> ObtenirTousAsync();
        Task<FluxFinancierDto?> ObtenirParIdAsync(string id);
    }
}
