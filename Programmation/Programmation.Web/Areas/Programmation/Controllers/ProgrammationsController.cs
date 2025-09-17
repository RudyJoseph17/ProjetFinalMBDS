using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Programmation.Application.Interface;
using Programmation.Application.Dtos;

namespace Programmation.Web.Areas.Programmation.Controllers
{
    [Area("Programmation")]
    public class ProgrammationsController : Controller
    {
        private readonly IProgrammationProjetService _programmationService;
        private readonly ILivrablesProjetService _livrablesService;
        private readonly IInformationsFinancieresProgrammeesProjetService _infosFinService;
        private readonly ILogger<ProgrammationsController> _logger;

        public ProgrammationsController(
            IProgrammationProjetService programmationService,
            ILivrablesProjetService livrablesService,
            IInformationsFinancieresProgrammeesProjetService infosFinService,
            ILogger<ProgrammationsController> logger)
        {
            _programmationService = programmationService;
            _livrablesService = livrablesService;
            _infosFinService = infosFinService;
            _logger = logger;
        }

        // ----------------------------------------------------------------
        // Programmation Projet
        // ----------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _programmationService.ObtenirTousAsync();
            return View("Index", list);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var dto = await _programmationService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Details", dto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new ProgrammationProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgrammationProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            await _programmationService.AjouterAsync(model);
            TempData["Success"] = "Programmation projet créée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var dto = await _programmationService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Edit", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProgrammationProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            await _programmationService.MettreAJourAsync(model);
            TempData["Success"] = "Programmation projet mise à jour !";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _programmationService.SupprimerAsync(byte.Parse(id));
            TempData["Success"] = "Programmation projet supprimée !";
            return RedirectToAction(nameof(Index));
        }

        // ----------------------------------------------------------------
        // Livrables Projet
        // ----------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> IndexLivrables()
        {
            var list = await _livrablesService.ObtenirTousAsync();
            return View("Livrables/Index", list);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsLivrable(byte id)
        {
            var dto = await _livrablesService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Livrables/Details", dto);
        }

        [HttpGet]
        public IActionResult CreateLivrable()
        {
            return View("Livrables/Create", new LivrablesProgrameProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLivrable(LivrablesProgrameProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("Livrables/Create", model);

            await _livrablesService.AjouterAsync(model);
            TempData["Success"] = "Livrable projet créé avec succès !";
            return RedirectToAction(nameof(IndexLivrables));
        }

        [HttpGet]
        public async Task<IActionResult> EditLivrable(byte id)
        {
            var dto = await _livrablesService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("Livrables/Edit", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLivrable(LivrablesProgrameProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("Livrables/Edit", model);

            await _livrablesService.MettreAJourAsync(model);
            TempData["Success"] = "Livrable projet mis à jour !";
            return RedirectToAction(nameof(IndexLivrables));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLivrable(byte id)
        {
            await _livrablesService.SupprimerAsync(id);
            TempData["Success"] = "Livrable projet supprimé !";
            return RedirectToAction(nameof(IndexLivrables));
        }

        // ----------------------------------------------------------------
        // Informations Financières Programmées Projet
        // ----------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> IndexInfosFinancieres()
        {
            var list = await _infosFinService.ObtenirTousAsync();
            return View("InfosFinancieres/Index", list);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsInfosFinancieres(byte id)
        {
            var dto = await _infosFinService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("InfosFinancieres/Details", dto);
        }

        [HttpGet]
        public IActionResult CreateInfosFinancieres()
        {
            return View("InfosFinancieres/Create", new InformationsFinancieresProgrammeesProjetDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInfosFinancieres(InformationsFinancieresProgrammeesProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("InfosFinancieres/Create", model);

            await _infosFinService.AjouterAsync(model);
            TempData["Success"] = "Information financière programmée créée !";
            return RedirectToAction(nameof(IndexInfosFinancieres));
        }

        [HttpGet]
        public async Task<IActionResult> EditInfosFinancieres(byte id)
        {
            var dto = await _infosFinService.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();
            return View("InfosFinancieres/Edit", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfosFinancieres(InformationsFinancieresProgrammeesProjetDto model)
        {
            if (!ModelState.IsValid)
                return View("InfosFinancieres/Edit", model);

            await _infosFinService.MettreAJourAsync(model);
            TempData["Success"] = "Information financière programmée mise à jour !";
            return RedirectToAction(nameof(IndexInfosFinancieres));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInfosFinancieres(byte id)
        {
            await _infosFinService.SupprimerAsync(id);
            TempData["Success"] = "Information financière programmée supprimée !";
            return RedirectToAction(nameof(IndexInfosFinancieres));
        }
    }
}
