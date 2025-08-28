using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Interfaces
{
    public interface ISuiviInformationFinanciereService
    {
        Task AjouterAsync(SuiviInformationFinanciereDto suiviInformationFinanciereDto);
        Task MettreAJourAsync(SuiviInformationFinanciereDto suiviInformationFinanciereDto);
        Task SupprimerAsync(byte IdInformationsFinancieres);
        Task<List<SuiviInformationFinanciereDto>> ObtenirTousAsync();
        Task<SuiviInformationFinanciereDto?> ObtenirParIdAsync(byte id);
    }
}
