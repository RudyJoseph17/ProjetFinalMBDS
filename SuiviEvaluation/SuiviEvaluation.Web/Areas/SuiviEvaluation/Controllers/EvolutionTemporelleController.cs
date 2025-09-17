// Web/Areas/SuiviEvaluation/Controllers/EvolutionTemporelleController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class EvolutionTemporelleController : Controller
    {
        private readonly IEvolutionTemporelleService _service;
        private readonly ILogger<EvolutionTemporelleController> _logger;

        public EvolutionTemporelleController(
            IEvolutionTemporelleService service,
            ILogger<EvolutionTemporelleController> logger)
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
            return View(new EvolutionTemporelleDuProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvolutionTemporelleDuProjetDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.AjouterAsync(model);
            TempData["Success"] = "Entrée temporelle créée avec succès";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EvolutionTemporelleDuProjetDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.MettreAJourAsync(model);
            TempData["Success"] = "Entrée temporelle mise à jour";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.SupprimerAsync(id);
            TempData["Success"] = "Entrée temporelle supprimée";
            return RedirectToAction(nameof(Index));
        }
    }
}
