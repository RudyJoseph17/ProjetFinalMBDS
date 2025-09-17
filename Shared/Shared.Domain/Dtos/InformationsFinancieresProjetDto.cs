using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared.Domain.Dtos
{
    public class InformationsFinancieresProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public int IdActivites { get; set; }
        public int? IdInformationsFinancieres { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? SourcesFinancement { get; set; }
        public decimal? MontantPrevu { get; set; }
        public decimal? MontantAutorisation { get; set; }
        public decimal? MontantDecaissement { get; set; }
        public decimal? MontantDepense { get; set; }
        public string? Article { get; set; }
        public string? Alinea { get; set; }
        public string? MoisPrevision { get; set; }
        public string? MoisAutorisation { get; set; }
        public string? MoisDepense { get; set; }
        public string? MoisDecaissement { get; set; }
    
    }
}
