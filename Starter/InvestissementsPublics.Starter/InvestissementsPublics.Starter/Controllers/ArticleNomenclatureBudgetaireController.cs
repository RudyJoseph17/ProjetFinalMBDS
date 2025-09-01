using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;

namespace InvestissementsPublics.Starter.Controllers
{
    public class ArticleNomenclatureBudgetaireController : Controller
    {
        private readonly IArticleNomenclatureBudgetaireService _service;

        public ArticleNomenclatureBudgetaireController(
            IArticleNomenclatureBudgetaireService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _service.ObtenirTousAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        public IActionResult Create()
        {
            var vm = new ArticleNomenclatureBudgetaireDto();
            var p = new ParagrapheNomenclatureBudgetaireDto();
            p.listAlineaNomenclatureBudgetaire.Add(new AlineaNomenclatureBudgetaireDto());
            vm.listParagrapheNomenclatureBudgetaire.Add(p);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleNomenclatureBudgetaireDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
