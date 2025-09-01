using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IArticleNomenclatureBudgetaireService
    {
        Task AjouterAsync(ArticleNomenclatureBudgetaireDto articleNomenclatureBudgetaire);
        Task MettreAJourAsync(ArticleNomenclatureBudgetaireDto articleNomenclatureBudgetaire);
        Task SupprimerAsync(int IdArticleNomenclatureBudgetaire);
        Task<List<ArticleNomenclatureBudgetaireDto>> ObtenirTousAsync();
        Task<ArticleNomenclatureBudgetaireDto?> ObtenirParIdAsync(int IdArticleNomenclatureBudgetaire);
    }
}
