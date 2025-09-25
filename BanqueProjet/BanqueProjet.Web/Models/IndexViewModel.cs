using BanqueProjet.Application.Dtos;

namespace BanqueProjet.Web.Models
{
    public class IndexViewModel
    {
        public List<ProjetsBPDto> Projets { get; set; }
        public DashboardStatsViewModel Stats { get; set; }
    }
}
