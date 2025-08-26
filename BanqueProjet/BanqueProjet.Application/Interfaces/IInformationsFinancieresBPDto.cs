using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IInformationsFinancieresBPDto
    {
        Task AjouterAsync(IInformationsFinancieresBPDto informationsFinancieresProjetBP);
        Task MettreAJourAsync(IInformationsFinancieresBPDto informationsFinancieresProjetBP);
        Task SupprimerAsync(byte IdInformationsFinancieres);
        Task<List<IInformationsFinancieresBPDto>> ObtenirTousAsync();
        Task<IInformationsFinancieresBPDto?> ObtenirParIdAsync(byte id);
    }
}
