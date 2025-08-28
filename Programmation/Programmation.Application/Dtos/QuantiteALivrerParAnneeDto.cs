using Microsoft.EntityFrameworkCore;
using Shared.Domain.Dtos;
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
    public class QuantiteALivrerParAnneeDto: LivrablesDuProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = null!;
        public byte IdLivrablesProjet { get; set; }
        public string? DefinitionLivrablesDuProjet { get; set; }
        public int? QuantiteALivrer { get; set; }
    }
}
