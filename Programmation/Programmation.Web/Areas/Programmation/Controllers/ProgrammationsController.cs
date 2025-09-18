using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Programmation.Application.Interface;
using Programmation.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;
using Programmation.Web.Models;

namespace Programmation.Web.Areas.Programmation.Controllers
{
    [Area("Programmation")]
    public class ProgrammationsController : Controller
    {
        private readonly IProgrammationProjetService _programmationService;
        private readonly ILivrablesProjetService _livrablesService;
        private readonly IInformationsFinancieresProgrammeesProjetService _infosFinService;
        private readonly IProjetsBPService _projetService;
        private readonly ILogger<ProgrammationsController> _logger;

        public ProgrammationsController(
            IProgrammationProjetService programmationService,
            ILivrablesProjetService livrablesService,
            IInformationsFinancieresProgrammeesProjetService infosFinService,
            IProjetsBPService projetService,
            ILogger<ProgrammationsController> logger)
        {
            _programmationService = programmationService;
            _livrablesService = livrablesService;
            _infosFinService = infosFinService;
            _projetService = projetService;
            _logger = logger;
        }

        // ----------------------------------------------------------------
        // Index : liste des projets avec avis "Favorable"
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var projets = await _projetService.ObtenirTousAsync();
                var projetsFavorables = projets.FindAll(p => p.AvisProjet == "Favorable");
                return View("Index", projetsFavorables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des projets avec avis favorable.");
                TempData["Error"] = "Impossible de récupérer les projets.";
                return View("Index", Array.Empty<ProjetsBPDto>());
            }
        }

        // ----------------------------------------------------------------
        // Détails d'un projet et préparation de la programmation
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Details(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet)) return BadRequest();

            try
            {
                var projetDto = await _projetService.ObtenirParIdAsync(idProjet);
                if (projetDto == null) return NotFound();

                var viewModel = new ProgrammationViewModel
                {
                    ProjetsCrees = new ProgrammationProjetDto
                    {
                        IdIdentificationProjet = projetDto.IdIdentificationProjet,
                        NomProjet = projetDto.NomProjet
                    },
                    LivrablesProgramme = await _livrablesService.ObtenirParProjetAsync(idProjet),
                    InfosFinancieresProgrammees = await _infosFinService.ObtenirParProjetAsync(idProjet)
                };

                return View("Details", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails du projet {Id}.", idProjet);
                TempData["Error"] = "Impossible de récupérer les informations du projet.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ----------------------------------------------------------------
        // Création d'une nouvelle programmation pour un projet
        // ----------------------------------------------------------------
        [HttpGet]
        public IActionResult Create(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet)) return BadRequest();

            var viewModel = new ProgrammationViewModel
            {
                ProjetsCrees = new ProgrammationProjetDto
                {
                    IdIdentificationProjet = idProjet
                }
            };

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgrammationViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            try
            {
                await _programmationService.AjouterAsync(model.ProjetsCrees);
                TempData["Success"] = "Programmation projet créée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la programmation pour le projet {Id}.", model?.ProjetsCrees?.IdIdentificationProjet);
                TempData["Error"] = "Impossible de créer la programmation.";
                return View("Create", model);
            }
        }

        // ----------------------------------------------------------------
        // Edition d'une programmation existante
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet)) return BadRequest();

            try
            {
                var programmation = await _programmationService.ObtenirParIdAsync(idProjet);
                if (programmation == null) return NotFound();

                var viewModel = new ProgrammationViewModel
                {
                    ProjetsCrees = programmation,
                    LivrablesProgramme = await _livrablesService.ObtenirParProjetAsync(idProjet),
                    InfosFinancieresProgrammees = await _infosFinService.ObtenirParProjetAsync(idProjet)
                };

                return View("Edit", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ouverture du formulaire d'édition du projet {Id}.", idProjet);
                TempData["Error"] = "Impossible d'ouvrir l'édition du projet.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProgrammationViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            try
            {
                await _programmationService.MettreAJourAsync(model.ProjetsCrees);
                TempData["Success"] = "Programmation projet mise à jour !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la programmation du projet {Id}.", model?.ProjetsCrees?.IdIdentificationProjet);
                TempData["Error"] = "Impossible de mettre à jour la programmation.";
                return View("Edit", model);
            }
        }

        // ----------------------------------------------------------------
        // Suppression d'une programmation
        // ----------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string idProjet)
        {
            if (string.IsNullOrWhiteSpace(idProjet)) return BadRequest();

            try
            {
                // Passe directement le string
                await _programmationService.SupprimerAsync(idProjet);
                TempData["Success"] = "Programmation projet supprimée !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la programmation Id={Id}.", idProjet);
                TempData["Error"] = "Impossible de supprimer la programmation.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
