using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Dtos
{
    public class ParagrapheNomenclatureBudgetaireDto
    {
        public int IdParagrapheNomenclatureBudgetaire { get; set; }
        public string? NomParagrapheNomenclatureBudgetaire { get; set; }
        public List<AlineaNomenclatureBudgetaireDto> listAlineaNomenclatureBudgetaire { get; set; } = new();
    }
}
