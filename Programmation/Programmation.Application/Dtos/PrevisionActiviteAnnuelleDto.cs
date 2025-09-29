using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Domain.Dtos;

namespace Programmation.Application.Dtos
{
    public class PrevisionActiviteAnnuelleDto: ActiviteAnnuelleDto
    {

        public int IdActivitesAnnuelles { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public decimal? CoutAnnuelProgramme { get; set; }


        [JsonProperty("ID_IDENTIFICATION_PROJET")]
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
