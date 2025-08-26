using BanqueProjet.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Interfaces
{
    public interface IProjetsBPService
    {
        Task AjouterAsync(ProjetsBPDto projetsBPD);
        Task MettreAJourAsync(ProjetsBPDto projetsBPD);
        Task SupprimerAsync(string IdIdentificationProjet);
        Task<List<ProjetsBPDto>> ObtenirTousAsync();
        Task<ProjetsBPDto?> ObtenirParIdAsync(string id);
    }
}
