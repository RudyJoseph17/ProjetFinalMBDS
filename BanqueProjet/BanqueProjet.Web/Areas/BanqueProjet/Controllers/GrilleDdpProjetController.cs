using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class GrilleDdpProjetController : Controller
    {
        private readonly IGrilleDdpProjetService _service;

        public GrilleDdpProjetController(IGrilleDdpProjetService service)
        {
            _service = service;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var grilles = await _service.ObtenirTousAsync();
            return View(grilles);
        }

        // GET: Details
        public async Task<IActionResult> Details(byte id)
        {
            var grille = await _service.ObtenirParIdAsync(id);
            if (grille == null)
                return NotFound();

            return View(grille);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrilleDdpProjetDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: GrilleDdpProjet/Edit/5
        public async Task<IActionResult> Edit(byte id)
        {
            if (id == 0)
                return NotFound();

            var grille = await _service.ObtenirParIdAsync(id);
            if (grille == null)
                return NotFound();

            return View(grille);
        }

        // POST: GrilleDdpProjet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, GrilleDdpProjetDto grilleDdpProjetDto)
        {
            if (id != grilleDdpProjetDto.IdGrilleDdpProjet)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(grilleDdpProjetDto);

            try
            {
                await _service.MettreAJourAsync(grilleDdpProjetDto);
                TempData["SuccessMessage"] = "La grille DDP a été mise à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ici tu peux logger l'erreur
                ModelState.AddModelError("", $"Erreur lors de la mise à jour : {ex.Message}");
                return View(grilleDdpProjetDto);
            }
        }
    }
}
