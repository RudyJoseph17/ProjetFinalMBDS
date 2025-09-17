using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using System.Threading.Tasks;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class AutorisationSurProjetController : Controller
    {
        private readonly IAutorisationSurProjetService _service;
        private readonly ILogger<AutorisationSurProjetController> _logger;

        public AutorisationSurProjetController(
            IAutorisationSurProjetService service,
            ILogger<AutorisationSurProjetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _service.ObtenirTousAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AutorisationSurProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutorisationSurProjetDto model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.AjouterAsync(model);
            TempData["Success"] = "Autorisation ajoutée avec succès";
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
        public async Task<IActionResult> Edit(AutorisationSurProjetDto model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.MettreAJourAsync(model);
            TempData["Success"] = "Autorisation mise à jour";
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
            TempData["Success"] = "Autorisation supprimée";
            return RedirectToAction(nameof(Index));
        }
    }
}
