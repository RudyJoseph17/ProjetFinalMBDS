using BanqueProjet.Application.Dtos;


namespace BanqueProjet.Web.Models
{
    public class InstitutionSectorielleViewModel
    {
        public string IdInstitutionSectorielle { get; set; }
        public string NomInstitutionSectorielle { get; set; }
        public string MissionInstitutionSectorielle { get; set; }
        public string AttributionsInstitutionSectorielle { get; set; }
        public List<string> NomsSections { get; set; } = new();
    }
}
