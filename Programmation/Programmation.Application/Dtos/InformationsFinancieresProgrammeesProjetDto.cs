using Newtonsoft.Json;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Programmation.Application.Dtos
{
    public class InformationsFinancieresProgrammeesProjetDto : InformationsFinancieresProjetDto
    {
        [JsonProperty("IdIdentificationProjet")]
        public int IdIdentificationProjet { get; set; }
        public int IdInformationsFinancieres { get; set; }
        public byte? ExerciceFiscalDebut { get; set; }
        public byte? ExerciceFiscalFin { get; set; }
        public string? SourcesFinancement { get; set; }
        public string? Article { get; set; }
        public string? Alinea { get; set; }
        public string? MoisPrevision { get; set; }
        public decimal? MontantPrevu { get; set; }

        /// <summary>
        /// Valeur non-null pour les calculs (remplace null par 0).
        /// </summary>
        public decimal MontantPrevuValue => MontantPrevu ?? 0m;
    }

    /// <summary>
    /// DTO agrégateur + utilitaires (somme par alinéa, article, activité, affichage, validation).
    /// </summary>
    public class AggregatedInformationsFinancieresProjetDto
    {
        public string? IdProjet { get; set; }
        public List<InformationsFinancieresProgrammeesProjetDto> Items { get; set; } = new();

        public decimal TotalMontantPrevu => Items.Sum(i => i.MontantPrevuValue);

        public IDictionary<string, decimal> MontantParAlinea()
        {
            return Items
                .GroupBy(i => NormalizeKey(i.Alinea))
                .ToDictionary(g => g.Key, g => g.Sum(i => i.MontantPrevuValue));
        }

        public IDictionary<string, decimal> MontantParArticle()
        {
            return Items
                .GroupBy(i => NormalizeKey(i.Article))
                .ToDictionary(g => g.Key, g => g.Sum(i => i.MontantPrevuValue));
        }

        public IDictionary<(string Article, string Alinea), decimal> MontantParArticleEtAlinea()
        {
            return Items
                .GroupBy(i => (Article: NormalizeKey(i.Article), Alinea: NormalizeKey(i.Alinea)))
                .ToDictionary(g => g.Key, g => g.Sum(i => i.MontantPrevuValue));
        }

        public static AggregatedInformationsFinancieresProjetDto From(
            IEnumerable<InformationsFinancieresProgrammeesProjetDto> items,
            string? idProjet = null)
        {
            return new AggregatedInformationsFinancieresProjetDto
            {
                IdProjet = idProjet,
                Items = items?.ToList() ?? new List<InformationsFinancieresProgrammeesProjetDto>()
            };
        }

        private static string NormalizeKey(string? key)
            => string.IsNullOrWhiteSpace(key) ? "(non précisé)" : key!.Trim();

        // ----- Structure prête à afficher: Article -> Alinea -> Montant -----

        /// <summary>
        /// Retourne une structure ordonnée et agrégée prête à être affichée :
        /// ArticleDisplayDto[] où chaque Article contient ses Alinea et montants.
        /// </summary>
        public IEnumerable<ArticleDisplayDto> GetDisplayStructure()
        {
            return Items
                .GroupBy(i => NormalizeKey(i.Article))
                .Select(g => new ArticleDisplayDto
                {
                    Article = g.Key,
                    Total = g.Sum(i => i.MontantPrevuValue),
                    Alineas = g
                        .GroupBy(i => NormalizeKey(i.Alinea))
                        .Select(ag => new AlineaDisplayDto
                        {
                            Alinea = ag.Key,
                            Montant = ag.Sum(i => i.MontantPrevuValue)
                        })
                        .OrderBy(a => a.Alinea)
                        .ToList()
                })
                .OrderBy(a => a.Article)
                .ToList();
        }

        // ----- Validation: interdiction des sommes négatives -----

        /// <summary>
        /// Vérifie que les agrégations (total, par article, par alinea, par activité)
        /// ne sont pas négatives. Retourne la liste des messages d'erreur (vide si ok).
        /// </summary>
       

        /// <summary>
        /// Lance une InvalidOperationException si des erreurs de validation existent.
        /// </summary>
      
    }

    // ----- DTOs d'affichage (Article -> Alinea) -----
    public class ArticleDisplayDto
    {
        public string Article { get; set; } = "";
        public decimal Total { get; set; }
        public List<AlineaDisplayDto> Alineas { get; set; } = new();
    }

    public class AlineaDisplayDto
    {
        public string Alinea { get; set; } = "";
        public decimal Montant { get; set; }
    }
}
