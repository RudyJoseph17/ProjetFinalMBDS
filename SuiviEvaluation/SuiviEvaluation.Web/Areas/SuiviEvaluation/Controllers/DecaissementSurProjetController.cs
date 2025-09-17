using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using System.Threading.Tasks;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class DecaissementSurProjetController : Controller
    {
        private readonly IDecaissementSurProjetService _service;
        private readonly ILogger<DecaissementSurProjetController> _logger;

        public DecaissementSurProjetController(
            IDecaissementSurProjetService service,
            ILogger<DecaissementSurProjetController> logger)
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
            return View(new DecaissementSurProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DecaissementSurProjetDto model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AjouterAsync(model);
            TempData["Success"] = "Décaissement ajouté avec succès";
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
        public async Task<IActionResult> Edit(DecaissementSurProjetDto model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.MettreAJourAsync(model);
            TempData["Success"] = "Décaissement mis à jour";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.SupprimerAsync(id);
            TempData["Success"] = "Décaissement supprimé";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.ObtenirParIdActiviteAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }
    }
}
