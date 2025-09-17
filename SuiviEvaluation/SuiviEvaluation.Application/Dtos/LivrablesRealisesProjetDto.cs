using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SuiviEvaluation.Application.Dtos
{
    public class LivrablesRealisesProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public string IdIdentificationProjet { get; set; }
        /// <summary>
        /// Quantités livrées par exercice fiscal (non persisté).
        /// </summary>
        [NotMapped]
        public Dictionary<byte, int> QuantiteLivreeParAnnee { get; set; } = new();

        /// <summary>
        /// Somme des quantités annuelles (persisté en base).
        /// </summary>
        public int QuantiteLivree => QuantiteLivreeParAnnee.Values.Sum();

        /// <summary>
        /// Valeur cible livrée récupérée (non persisté ici).
        /// </summary>
        [NotMapped]
        public int ValeurLivree { get; private set; }

        /// <summary>
        /// Différence calculée entre réalisé et cible (non persisté).
        /// </summary>
        [NotMapped]
        public int Difference => QuantiteLivree - ValeurLivree;

        /// <summary>
        /// Méthode d’initialisation de ValeurLivree (ex: via AutoMapper ou service).
        /// </summary>
        public void InitializeValeurLivree(int valeur)
        {
            ValeurLivree = valeur;
        }
    }
}
