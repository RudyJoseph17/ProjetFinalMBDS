using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BanqueProjet.Application.Dtos
{
    public class LocalisationGeographiqueProjDto
    {

        [JsonProperty("ID_LOCALISATION_GEOGRAPHIQUE")]
        public byte IdLocalisationGeographique { get; set; }

        [JsonProperty("ID_IDENTIFICATION_PROJET")]
        public string IdIdentificationProjet { get; set; }

        [JsonProperty("DEPARTEMENT")]
        public string Departement { get; set; }

        [JsonProperty("ARRONDISSEMENT")]
        public string Arrondissement { get; set; }

        [JsonProperty("COMMUNE")]
        public string Commune { get; set; }

        [JsonProperty("SECTION_COMMUNALE")]
        public string SectionCommunale { get; set; }
    }
}
