using BanqueProjet.Application.Dtos;
using System.Collections.Generic;

namespace BanqueProjet.Web.Models
{
    public class DashboardStatsViewModel
    {
        // Chiffres visibles aujourd'hui
        public int TotalProjets { get; set; }

        // Champs pour futur usage (validés)
        public int ProjetsValides { get; set; } = 0;
        public List<string> NomsProjetsValides { get; set; } = new();
        public IEnumerable<ProjetsBPDto> Projets { get; set; } = new List<ProjetsBPDto>();

        // (Optionnel) autres champs à ajouter plus tard
    }
}
