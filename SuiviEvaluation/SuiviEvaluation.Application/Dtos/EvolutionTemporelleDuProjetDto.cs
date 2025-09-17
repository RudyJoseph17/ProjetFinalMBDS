using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SuiviEvaluation.Application.Dtos
{
    public class EvolutionTemporelleDuProjetDto
    {
        /// <summary>
        /// Identifiant du projet.
        /// </summary>
        [JsonProperty("idIdentificationProjet")]
        public string IdIdentificationProjet { get; set; } = string.Empty;

        /// <summary>
        /// Date de démarrage effective du projet.
        /// </summary>
        [JsonProperty("dateDemarrage")]
        public DateTime DateDeDemmarage { get; set; }

        /// <summary>
        /// Durée du projet en mois (héritée de ProjetsBPDto / IdentificationProjetDto).
        /// Vous devez initialiser cette propriété lors de la création du DTO.
        /// </summary>
        [JsonProperty("dureeProjet")]
        public int DureeProjet { get; set; }

        /// <summary>
        /// Date d’achèvement prévue calculée : DateDeDemmarage + DureeProjet mois.
        /// </summary>
        [NotMapped]
        [JsonProperty("dateAchevementPrevue")]
        public DateTime DateAchevementPrevue
            => DateDeDemmarage.AddMonths(DureeProjet);

        /// <summary>
        /// Mois écoulés depuis la date de démarrage jusqu’à aujourd’hui.
        /// </summary>
        [NotMapped]
        [JsonProperty("tempsEcoule")]
        public int TempsEcoule
        {
            get
            {
                var start = DateDeDemmarage.Date;
                var today = DateTime.UtcNow.Date;
                if (today <= start)
                    return 0;

                // Calcul des mois pleins écoulés
                int months = (today.Year - start.Year) * 12 + today.Month - start.Month;
                if (today.Day < start.Day)
                    months--;

                return Math.Max(0, months);
            }
        }

        /// <summary>
        /// Mois restant avant la date d’achèvement prévue.
        /// </summary>
        [NotMapped]
        [JsonProperty("tempsRestant")]
        public int TempsRestant
        {
            get
            {
                var remaining = DureeProjet - TempsEcoule;
                return remaining > 0 ? remaining : 0;
            }
        }

        /// <summary>
        /// Indique si la date d’achèvement prévue est dépassée.
        /// </summary>
        [NotMapped]
        [JsonProperty("situationProjet")]
        public string SituationProjet
            => DateTime.UtcNow.Date > DateAchevementPrevue
                ? "Temps d'execution depasse"
                : "Dans les temps";
    }
}
