//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using BanqueProjet.Application.Interfaces;
//using BanqueProjet.Application.Dtos;

//namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
//{
//    [Area("BanqueProjet")]
//    public class DdpCadreLogiqueController : Controller
//    {
//        private readonly IDdpCadreLogiqueService _service;
//        private readonly ILogger<DdpCadreLogiqueController> _logger;

//        public DdpCadreLogiqueController(
//            IDdpCadreLogiqueService service,
//            ILogger<DdpCadreLogiqueController> logger)
//        {
//            _service = service;
//            _logger = logger;
//        }

//        // GET: Index
//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var items = await _service.ObtenirTousAsync();
//            return View(items);
//        }

//        // GET: Details
//        [HttpGet]
//        public async Task<IActionResult> Details(byte id)
//        {
//            var dto = await _service.ObtenirParIdAsync(id);
//            if (dto == null) return NotFound();
//            return View(dto);
//        }

//        // GET: Create
//        // Passez l'ID du projet parent pour rattacher le cadre logique
//        [HttpGet]
//        public async Task<IActionResult> Create(string idIdentificationProjet)
//        {
//            // Générer l'ID du cadre logique via la séquence Oracle
//            var newCadreId = await _service.GetNextIdAsync();

//            var dto = new DdpCadreLogiqueDto
//            {
//                IdDdpCadreLogique = newCadreId,
//                IdIdentificationProjet = idIdentificationProjet
//            };
//            return View(dto);
//        }

//        // POST: Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(DdpCadreLogiqueDto model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            // Si l'ID n'a pas été généré en GET, on le génère ici
//            if (model.IdDdpCadreLogique == 0)
//            {
//                model.IdDdpCadreLogique = await _service.GetNextIdAsync();
//            }

//            _logger.LogInformation("🚀 Création DDP Cadre Logique, Projet = {Projet}, CadreId = {Id}",
//                model.IdIdentificationProjet, model.IdDdpCadreLogique);

//            await _service.AjouterAsync(model);
//            TempData["SuccessMessage"] = "Entrée ajoutée avec succès !";
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Edit
//        [HttpGet]
//        public async Task<IActionResult> Edit(byte id)
//        {
//            var dto = await _service.ObtenirParIdAsync(id);
//            if (dto == null) return NotFound();
//            return View(dto);
//        }

//        // POST: Edit
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(DdpCadreLogiqueDto model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            _logger.LogInformation("✏️ Mise à jour DDP Cadre Logique, ID = {Id}", model.IdDdpCadreLogique);
//            await _service.MettreAJourAsync(model);
//            TempData["SuccessMessage"] = "Entrée mise à jour avec succès !";
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Delete
//        [HttpGet]
//        public async Task<IActionResult> Delete(byte id)
//        {
//            var dto = await _service.ObtenirParIdAsync(id);
//            if (dto == null) return NotFound();
//            return View(dto);
//        }

//        // POST: Delete
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(byte id)
//        {
//            _logger.LogInformation("🗑️ Suppression DDP Cadre Logique, ID = {Id}", id);
//            await _service.SupprimerAsync(id);
//            TempData["SuccessMessage"] = "Entrée supprimée avec succès !";
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
