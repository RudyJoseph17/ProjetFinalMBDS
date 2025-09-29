using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Programmation.Application.Dtos
{
    public class GestionEtSuiviProjetDto
    {
        public int IdGestionDeProjetEtSuivi { get; set; }
        public byte? ExerciceFiscaleDebut { get; set; }
        public byte? ExerciceFiscaleFin { get; set; }
        public string? DescriptionRole { get; set; }
        public string? CadreDeReferenceContrat { get; set; }
        public string? DescriptionDesMethodes { get; set; }
        public string? IdentificationDesAgentsImpliques { get; set; }

        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
