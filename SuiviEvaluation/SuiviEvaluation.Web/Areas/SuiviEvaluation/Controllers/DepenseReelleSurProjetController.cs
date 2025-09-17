// Web/Areas/SuiviEvaluation/Controllers/DepenseReelleSurProjetController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using System.Threading.Tasks;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class DepenseReelleSurProjetController : Controller
    {
        private readonly IDepenseReelleSurProjetService _service;
        private readonly ILogger<DepenseReelleSurProjetController> _logger;

        public DepenseReelleSurProjetController(
            IDepenseReelleSurProjetService service,
            ILogger<DepenseReelleSurProjetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _service.ObtenirTousAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepenseReelleSurProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepenseReelleSurProjetDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.AjouterAsync(model);
            TempData["Success"] = "Dépense réelle ajoutée avec succès";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenirParIdActiviteAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepenseReelleSurProjetDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.MettreAJourAsync(model);
            TempData["Success"] = "Dépense réelle mise à jour";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.ObtenirParIdActiviteAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.SupprimerAsync(id);
            TempData["Success"] = "Dépense réelle supprimée";
            return RedirectToAction(nameof(Index));
        }
    }
}
