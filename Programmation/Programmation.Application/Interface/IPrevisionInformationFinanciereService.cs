using Programmation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Interface
{
    public interface IPrevisionInformationFinanciereService
    {
        Task AjouterAsync(PrevisionInformationFinanciereDto previsionInformationFinanciere);
        Task MettreAJourAsync(PrevisionInformationFinanciereDto previsionInformationFinanciere);
        Task SupprimerAsync(byte IdInformationsFinancieres);
        Task<List<PrevisionInformationFinanciereDto>> ObtenirTousAsync();
        Task<PrevisionInformationFinanciereDto?> ObtenirParIdAsync(byte id);
    }
}
