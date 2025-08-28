using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Dtos
{
    public class QuantiteLivreeParAnneeDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = null!;
        public byte IdLivrablesProjet { get; set; }
        public string? DefinitionLivrablesDuProjet { get; set; }
        public int? QuantiteLivree { get; set; }
    }
}
