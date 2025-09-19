using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

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

        // GET: Create?projetId=CQ0-04-EQ-4C&nomProjet=Entretien
        public async Task<IActionResult> Create(
            [FromQuery(Name = "projetId")] string idProjet,
            [FromQuery(Name = "nomProjet")] string nomProjet)
        {
            if (string.IsNullOrEmpty(idProjet))
                return BadRequest("ID projet manquant.");

            var existingGrille = await _service.ObtenirParProjetIdAsync(idProjet);

            var dto = existingGrille ?? new GrilleDdpProjetDto
            {
                IdIdentificationProjet = idProjet,
                TitreProjet = !string.IsNullOrEmpty(nomProjet) ? nomProjet : "Nom du projet ici si nécessaire"
            };

            return View(dto);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrilleDdpProjetDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _service.AjouterAsync(dto);
                TempData["SuccessMessage"] = "La grille DDP a été créée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                // PK violation
                ModelState.AddModelError("", "Une grille DDP existe déjà pour ce projet.");
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erreur lors de la création : {ex.Message}");
                return View(dto);
            }
        }

        // GET: Edit
        public async Task<IActionResult> Edit(byte id)
        {
            if (id == 0)
                return NotFound();

            var grille = await _service.ObtenirParIdAsync(id);
            if (grille == null)
                return NotFound();

            return View(grille);
        }

        // POST: Edit
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
                ModelState.AddModelError("", $"Erreur lors de la mise à jour : {ex.Message}");
                return View(grilleDdpProjetDto);
            }
        }
    }
}
