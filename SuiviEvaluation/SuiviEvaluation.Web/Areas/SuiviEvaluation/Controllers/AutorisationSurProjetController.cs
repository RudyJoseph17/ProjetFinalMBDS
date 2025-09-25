using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Web.Models;
using Shared.Domain.Dtos;
using BanqueProjet.Application.Dtos;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class AutorisationSurProjetController : Controller
    {
        private readonly IAutorisationSurProjetService _service;
        private readonly ISuiviProjetService _projetService;
        private readonly ILogger<AutorisationSurProjetController> _logger;

        public AutorisationSurProjetController(
            IAutorisationSurProjetService service,
            ISuiviProjetService projetService,
            ILogger<AutorisationSurProjetController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _projetService = projetService ?? throw new ArgumentNullException(nameof(projetService));
            _logger = logger;
        }

        // INDEX : liste des projets avec total autorisations
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projets = await _projetService.ObtenirTousAsync();
            var autorisations = await _service.ObtenirTousAsync();

            var vm = projets.Select(p =>
            {
                var total = autorisations
                    .Where(a => a.IdIdentificationProjet == p.IdIdentificationProjet)
                    .Sum(a => a.MontantAutorisation ?? 0m);

                return new ProjetListItemViewModel
                {
                    IdIdentificationProjet = p.IdIdentificationProjet,
                    NomProjet = p.NomProjet ?? p.IdIdentificationProjet,
                    TotalAutorisation = total
                };
            }).ToList();

            return View(vm);
        }

        // CREATE GET : pré-remplit projet + activités (passer projetId ou nomProjet en querystring)
        [HttpGet]
        public async Task<IActionResult> Create(
            [FromQuery(Name = "projetId")] string projetId,
            [FromQuery(Name = "nomProjet")] string nomProjet
            )
        {
            if (string.IsNullOrEmpty(projetId))
                return BadRequest("ID projet manquant.");

            try
            {
                // Récupère le projet depuis le service projet (option 1)
                var projet = await _projetService.ObtenirParIdAsync(projetId);

                // Récupère les activités liées au projet pour le select list (si disponible)
                var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(projetId);
                var activitesSelect = MapActivitesToSelectList(rawActivites);

                var vm = new AutorisationViewModel
                {
                    IdIdentificationProjet = projetId,
                    SuiviProjets = projet,
                    NomProjet = projet?.NomProjet ?? projetId,
                    Activites = MapActivitesToSelectList(rawActivites),
                    AutorisationsActivite = new List<AutorisationSurProjetDto>() // vide pour création
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du projet {projId} pour Create", projetId);
                // On peut retourner une erreur mais pour l'instant on renvoie BadRequest avec message simple
                return StatusCode(500, "Erreur lors du chargement des données du projet.");
            }
        }


        // CREATE POST : recoit le ViewModel contenant AutorisationsActivite (List<AutorisationSurProjetDto>)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutorisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(model.IdIdentificationProjet ?? string.Empty);
                model.Activites = MapActivitesToSelectList(rawActivites);
                return View(model);
            }

            if (model.AutorisationsActivite == null || !model.AutorisationsActivite.Any())
            {
                ModelState.AddModelError(string.Empty, "Aucune autorisation à enregistrer.");
                var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(model.IdIdentificationProjet ?? string.Empty);
                model.Activites = MapActivitesToSelectList(rawActivites);
                return View(model);
            }

            try
            {
                var toSave = model.AutorisationsActivite.Select(a => new AutorisationSurProjetDto
                {
                    IdIdentificationProjet = model.IdIdentificationProjet,
                    IdActivites = a.IdActivites,
                    ExerciceFiscalDebut = a.ExerciceFiscalDebut,
                    ExerciceFiscalFin = a.ExerciceFiscalFin,
                    Article = a.Article,
                    Alinea = a.Alinea,
                    MoisAutorisation = a.MoisAutorisation,
                    MontantAutorisation = a.MontantAutorisation
                }).ToList();

                foreach (var dto in toSave)
                {
                    await _service.AjouterAsync(dto);
                }

                TempData["Success"] = "Autorisation(s) créées avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création des autorisations pour projet {projId}", model.IdIdentificationProjet);
                ModelState.AddModelError(string.Empty, "Erreur lors de l’enregistrement des autorisations.");
                var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(model.IdIdentificationProjet ?? string.Empty);
                model.Activites = MapActivitesToSelectList(rawActivites);
                return View(model);
            }
        }

        // EDIT / DETAILS / DELETE sont gardés simples (utilise DTO existant)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenirParIdActiviteAsync(id);
            if (dto == null) return NotFound();

            // obtenir projet & activités pour préremplir
            var projet = await _projetService.ObtenirParIdAsync(dto.IdIdentificationProjet);
            var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(dto.IdIdentificationProjet);

            var vm = new AutorisationViewModel
            {
                SuiviProjets = projet,
                Activites = MapActivitesToSelectList(rawActivites),
                AutorisationsActivite = new List<AutorisationSurProjetDto> { dto } // on affiche la seule autorisation en édition
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AutorisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var rawActivites = await _projetService.ObtenirActivitesParProjetAsync(model.SuiviProjets?.IdIdentificationProjet ?? string.Empty);
                model.Activites = MapActivitesToSelectList(rawActivites);
                return View(model);
            }

            // on attend au moins une autorisation (la première) pour mettre à jour
            var dto = model.AutorisationsActivite?.FirstOrDefault();
            if (dto == null) return BadRequest();

            dto.IdIdentificationProjet ??= model.SuiviProjets?.IdIdentificationProjet ?? string.Empty;
            await _service.MettreAJourAsync(dto);

            TempData["Success"] = "Autorisation mise à jour.";
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
            TempData["Success"] = "Autorisation supprimée.";
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private List<SelectListItem> MapActivitesToSelectList(IEnumerable<object> raw)
        {
            var list = new List<SelectListItem>();
            if (raw == null) return list;

            foreach (var o in raw)
            {
                if (o is ActiviteDto dto)
                {
                    list.Add(new SelectListItem
                    {
                        Value = dto.IdActivites.ToString(),
                        Text = dto.NomActivite ?? $"Activité {dto.IdActivites}"
                    });
                }
            }

            return list;
        }

        #endregion
    }
}
