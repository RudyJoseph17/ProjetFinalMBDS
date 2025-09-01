using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class ArticleNomenclatureBudgetaireDto
    {
        public int IdArticleNomenclatureBudgetaire { get; set; }
        public string? NomArticleNomenclatureBudgetaire { get; set; }
        public List<ParagrapheNomenclatureBudgetaireDto> listParagrapheNomenclatureBudgetaire { get; set; } = new();
    }
}
