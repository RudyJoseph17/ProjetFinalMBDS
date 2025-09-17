using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class LivrablesRealisesProjetController : Controller
    {
        private readonly ILivrablesRealisesProjetService _service;
        private readonly ILogger<LivrablesRealisesProjetController> _logger;

        public LivrablesRealisesProjetController(
            ILivrablesRealisesProjetService service,
            ILogger<LivrablesRealisesProjetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: /SuiviEvaluation/LivrablesRealisesProjet
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _service.ObtenirTousAsync();
            return View("Index", list);
        }

        // GET: /SuiviEvaluation/LivrablesRealisesProjet/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Details", dto);
        }

        // GET: /SuiviEvaluation/LivrablesRealisesProjet/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new LivrablesRealisesProjetDto());
        }

        // POST: /SuiviEvaluation/LivrablesRealisesProjet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivrablesRealisesProjetDto model, string quantiteParAnneeJson)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            // Désérialiser le JSON en dictionnaire
            model.QuantiteLivreeParAnnee =
                Newtonsoft.Json.JsonConvert
                .DeserializeObject<Dictionary<byte, int>>(quantiteParAnneeJson)
                ?? new Dictionary<byte, int>();

            await _service.AjouterAsync(model);
            TempData["Success"] = "Livrables réalisés créés avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: /SuiviEvaluation/LivrablesRealisesProjet/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Edit", dto);
        }

        // POST: /SuiviEvaluation/LivrablesRealisesProjet/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LivrablesRealisesProjetDto model, string quantiteParAnneeJson)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            model.QuantiteLivreeParAnnee =
                Newtonsoft.Json.JsonConvert
                .DeserializeObject<Dictionary<byte, int>>(quantiteParAnneeJson)
                ?? new Dictionary<byte, int>();

            await _service.MettreAJourAsync(model);
            TempData["Success"] = "Livrables réalisés mis à jour !";
            return RedirectToAction(nameof(Index));
        }

        // POST: /SuiviEvaluation/LivrablesRealisesProjet/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            await _service.SupprimerProjetAsync(id);
            TempData["Success"] = "Livrables réalisés supprimés !";
            return RedirectToAction(nameof(Index));
        }
    }
}
