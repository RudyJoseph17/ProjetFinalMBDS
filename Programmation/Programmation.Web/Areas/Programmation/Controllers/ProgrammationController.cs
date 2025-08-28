using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Programmation.Web.Models;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programmation.Web.Controllers
{
    [Area("Programmation")]
    public class ProgrammationController : Controller
    {
        private const int TotalSteps = 3;
        private readonly IQuantiteALivrerParAnneeService _quantiteService;
        private readonly IPrevisionInformationFinanciereService _previsionService;
        private readonly ILogger<ProgrammationController> _logger;

        public ProgrammationController(
            IQuantiteALivrerParAnneeService quantiteService,
            IPrevisionInformationFinanciereService previsionService,
            ILogger<ProgrammationController> logger)
        {
            _quantiteService = quantiteService ?? throw new ArgumentNullException(nameof(quantiteService));
            _previsionService = previsionService ?? throw new ArgumentNullException(nameof(previsionService));
            _logger = logger;
        }

        // -----------------------
        // INDEX : liste combinée
        // -----------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["TotalSteps"] = TotalSteps;

            try
            {
                var result = new List<ProgrammationViewModel>();

                var previsions = await _previsionService.ObtenirTousAsync();
                foreach (var p in previsions)
                {
                    result.Add(new ProgrammationViewModel
                    {
                        PrevisionsFinancieres = p,
                        QuantiteALivrer = new QuantiteALivrerParAnneeDto()
                    });
                }

                var quantites = await _quantiteService.ObtenirTousAsync();
                foreach (var q in quantites)
                {
                    result.Add(new ProgrammationViewModel
                    {
                        QuantiteALivrer = q,
                        PrevisionsFinancieres = new PrevisionInformationFinanciereDto()
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la liste Programmation");
                TempData["Error"] = "Impossible de charger les données de programmation.";
                return View(new List<ProgrammationViewModel>());
            }
        }

        // -----------------------
        // DETAILS : prévision ou quantité (choisir par paramètres)
        // -----------------------
        [HttpGet]
        public async Task<IActionResult> Details(int? idInformationsFinancieres, int? idActivite, string? idIdentificationProjet, byte? idLivrablesProjet)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (idInformationsFinancieres == null && idActivite == null && (string.IsNullOrWhiteSpace(idIdentificationProjet) || idLivrablesProjet == null))
                return BadRequest("Paramètres manquants pour afficher les détails.");

            try
            {
                // Prévision (priorité)
                if (idInformationsFinancieres != null || idActivite != null)
                {
                    PrevisionInformationFinanciereDto? prevision = null;

                    // Si on a un id (route) try cast & call ObtenirParIdAsync
                    var idInfoByte = ToByteSafe(idInformationsFinancieres);
                    if (idInfoByte.HasValue)
                    {
                        prevision = await SafeObtenirPrevisionParId(idInfoByte.Value);
                    }

                    // fallback by IdActivite
                    if (prevision == null && idActivite != null)
                    {
                        prevision = (await _previsionService.ObtenirTousAsync())
                                        .FirstOrDefault(p => p.IdActivites == idActivite.Value);
                    }

                    if (prevision == null) return NotFound();

                    var vm = new ProgrammationViewModel { PrevisionsFinancieres = prevision, QuantiteALivrer = new QuantiteALivrerParAnneeDto() };
                    return View(vm);
                }

                // Quantité : on a idIdentificationProjet + idLivrablesProjet
                QuantiteALivrerParAnneeDto? quantite = null;

                if (idLivrablesProjet != null)
                {
                    // essayer ObtenirParIdAsync (service attend un byte id)
                    quantite = await SafeObtenirQuantiteParId(idLivrablesProjet.Value);

                    // si service renvoie qqch, vérifier le projet
                    if (quantite != null && !string.IsNullOrWhiteSpace(idIdentificationProjet))
                    {
                        if (!string.Equals(quantite.IdIdentificationProjet, idIdentificationProjet, StringComparison.OrdinalIgnoreCase))
                        {
                            // mismatch -> ignore this result, fallback to filtering all
                            quantite = null;
                        }
                    }
                }

                if (quantite == null)
                {
                    var all = await _quantiteService.ObtenirTousAsync();
                    quantite = all.FirstOrDefault(q =>
                        string.Equals(q.IdIdentificationProjet, idIdentificationProjet, StringComparison.OrdinalIgnoreCase)
                        && q.IdLivrablesProjet == idLivrablesProjet);
                }

                if (quantite == null) return NotFound();

                var vmQuant = new ProgrammationViewModel { QuantiteALivrer = quantite, PrevisionsFinancieres = new PrevisionInformationFinanciereDto() };
                return View(vmQuant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Details Programmation");
                TempData["Error"] = "Erreur lors de la récupération du détail.";
                return RedirectToAction(nameof(Index));
            }
        }

        // -----------------------
        // CREATE : GET => form vide
        // -----------------------
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TotalSteps"] = TotalSteps;

            var vm = new ProgrammationViewModel
            {
                PrevisionsFinancieres = new PrevisionInformationFinanciereDto(),
                QuantiteALivrer = new QuantiteALivrerParAnneeDto()
            };
            return View(vm);
        }

        // -----------------------
        // CREATE : POST => enregistrer (distinction via submitAction)
        // -----------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgrammationViewModel vm, string submitAction, int? currentStep)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                if (string.Equals(submitAction, "createPrevision", StringComparison.OrdinalIgnoreCase))
                {
                    await _previsionService.AjouterAsync(vm.PrevisionsFinancieres);
                    TempData["Success"] = "Prévision financière créée.";
                }
                else if (string.Equals(submitAction, "createQuantite", StringComparison.OrdinalIgnoreCase))
                {
                    await _quantiteService.AjouterAsync(vm.QuantiteALivrer);
                    TempData["Success"] = "Quantité à livrer créée.";
                }
                else
                {
                    // fallback : tenter d'ajouter ce qui est renseigné
                    if (vm.PrevisionsFinancieres != null && vm.PrevisionsFinancieres.IdActivites != 0)
                    {
                        await _previsionService.AjouterAsync(vm.PrevisionsFinancieres);
                    }
                    if (vm.QuantiteALivrer != null && !string.IsNullOrWhiteSpace(vm.QuantiteALivrer.IdIdentificationProjet))
                    {
                        await _quantiteService.AjouterAsync(vm.QuantiteALivrer);
                    }
                    TempData["Success"] = "Données créées.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Create Programmation");
                ModelState.AddModelError(string.Empty, "Erreur lors de la création.");
                return View(vm);
            }
        }

        // -----------------------
        // EDIT : GET => formulaire pré-rempli
        // -----------------------
        [HttpGet]
        public async Task<IActionResult> Edit(int? idInformationsFinancieres, int? idActivite, string? idIdentificationProjet, byte? idLivrablesProjet)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (idInformationsFinancieres == null && idActivite == null && (string.IsNullOrWhiteSpace(idIdentificationProjet) || idLivrablesProjet == null))
                return BadRequest("Paramètres manquants pour l'édition.");

            try
            {
                // Prévision
                if (idInformationsFinancieres != null || idActivite != null)
                {
                    PrevisionInformationFinanciereDto? prevision = null;
                    var idInfoByte = ToByteSafe(idInformationsFinancieres);
                    if (idInfoByte.HasValue)
                    {
                        prevision = await SafeObtenirPrevisionParId(idInfoByte.Value);
                    }

                    if (prevision == null && idActivite != null)
                    {
                        prevision = (await _previsionService.ObtenirTousAsync()).FirstOrDefault(p => p.IdActivites == idActivite.Value);
                    }

                    if (prevision == null) return NotFound();

                    var vm = new ProgrammationViewModel { PrevisionsFinancieres = prevision, QuantiteALivrer = new QuantiteALivrerParAnneeDto() };
                    return View(vm);
                }

                // Quantité
                QuantiteALivrerParAnneeDto? quantite = null;
                if (idLivrablesProjet != null)
                {
                    quantite = await SafeObtenirQuantiteParId(idLivrablesProjet.Value);
                    if (quantite != null && !string.IsNullOrWhiteSpace(idIdentificationProjet)
                        && !string.Equals(quantite.IdIdentificationProjet, idIdentificationProjet, StringComparison.OrdinalIgnoreCase))
                    {
                        quantite = null;
                    }
                }

                if (quantite == null)
                {
                    var all = await _quantiteService.ObtenirTousAsync();
                    quantite = all.FirstOrDefault(q =>
                        string.Equals(q.IdIdentificationProjet, idIdentificationProjet, StringComparison.OrdinalIgnoreCase)
                        && q.IdLivrablesProjet == idLivrablesProjet);
                }

                if (quantite == null) return NotFound();

                var vmQuant = new ProgrammationViewModel { QuantiteALivrer = quantite, PrevisionsFinancieres = new PrevisionInformationFinanciereDto() };
                return View(vmQuant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Edit GET Programmation");
                TempData["Error"] = "Erreur lors du chargement du formulaire d'édition.";
                return RedirectToAction(nameof(Index));
            }
        }

        // -----------------------
        // EDIT : POST => sauvegarder modifications
        // -----------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProgrammationViewModel vm, string submitAction, int? currentStep)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                if (string.Equals(submitAction, "editPrevision", StringComparison.OrdinalIgnoreCase))
                {
                    await _previsionService.MettreAJourAsync(vm.PrevisionsFinancieres);
                    TempData["Success"] = "Prévision mise à jour.";
                }
                else if (string.Equals(submitAction, "editQuantite", StringComparison.OrdinalIgnoreCase))
                {
                    await _quantiteService.MettreAJourAsync(vm.QuantiteALivrer);
                    TempData["Success"] = "Quantité mise à jour.";
                }
                else
                {
                    if (vm.PrevisionsFinancieres != null && vm.PrevisionsFinancieres.IdActivites != 0)
                        await _previsionService.MettreAJourAsync(vm.PrevisionsFinancieres);
                    if (vm.QuantiteALivrer != null && !string.IsNullOrWhiteSpace(vm.QuantiteALivrer.IdIdentificationProjet))
                        await _quantiteService.MettreAJourAsync(vm.QuantiteALivrer);

                    TempData["Success"] = "Données mises à jour.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Edit POST Programmation");
                ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour.");
                return View(vm);
            }
        }

        // -----------------------
        // DELETE : GET => confirmation
        // -----------------------
        [HttpGet]
        public async Task<IActionResult> Delete(int? idInformationsFinancieres, int? idActivite, string? idIdentificationProjet, byte? idLivrablesProjet)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (idInformationsFinancieres == null && idActivite == null && (string.IsNullOrWhiteSpace(idIdentificationProjet) || idLivrablesProjet == null))
                return BadRequest("Paramètres manquants pour la suppression.");

            try
            {
                if (idInformationsFinancieres != null || idActivite != null)
                {
                    PrevisionInformationFinanciereDto? prevision = null;
                    var idInfoByte = ToByteSafe(idInformationsFinancieres);
                    if (idInfoByte.HasValue)
                    {
                        prevision = await SafeObtenirPrevisionParId(idInfoByte.Value);
                    }

                    if (prevision == null && idActivite != null)
                    {
                        prevision = (await _previsionService.ObtenirTousAsync()).FirstOrDefault(p => p.IdActivites == idActivite.Value);
                    }

                    if (prevision == null) return NotFound();

                    var vm = new ProgrammationViewModel { PrevisionsFinancieres = prevision, QuantiteALivrer = new QuantiteALivrerParAnneeDto() };
                    return View(vm);
                }

                var all = await _quantiteService.ObtenirTousAsync();
                var quantite = all.FirstOrDefault(q =>
                    string.Equals(q.IdIdentificationProjet, idIdentificationProjet, StringComparison.OrdinalIgnoreCase)
                    && q.IdLivrablesProjet == idLivrablesProjet);

                if (quantite == null) return NotFound();

                var vmQuant = new ProgrammationViewModel { QuantiteALivrer = quantite, PrevisionsFinancieres = new PrevisionInformationFinanciereDto() };
                return View(vmQuant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Delete GET Programmation");
                TempData["Error"] = "Erreur lors du chargement de la confirmation de suppression.";
                return RedirectToAction(nameof(Index));
            }
        }

        // -----------------------
        // DELETE : POST => suppression effective
        // -----------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? idInformationsFinancieres, int? idActivite, string? idIdentificationProjet, byte? idLivrablesProjet)
        {
            ViewData["TotalSteps"] = TotalSteps;

            if (idInformationsFinancieres == null && idActivite == null && (string.IsNullOrWhiteSpace(idIdentificationProjet) || idLivrablesProjet == null))
                return BadRequest("Paramètres manquants pour la suppression.");

            try
            {
                if (idInformationsFinancieres != null || idActivite != null)
                {
                    // supprime en privilégiant IdInformationsFinancieres
                    var idInfoByte = ToByteSafe(idInformationsFinancieres);
                    if (idInfoByte.HasValue)
                    {
                        await _previsionService.SupprimerAsync(idInfoByte.Value);
                        TempData["Success"] = "Prévision supprimée.";
                    }
                    else
                    {
                        var prevision = (await _previsionService.ObtenirTousAsync()).FirstOrDefault(p => p.IdActivites == idActivite);
                        if (prevision != null && prevision.IdInformationsFinancieres.HasValue)
                        {
                            var idToDelete = ToByteSafe(prevision.IdInformationsFinancieres);
                            if (idToDelete.HasValue)
                            {
                                await _previsionService.SupprimerAsync(idToDelete.Value);
                                TempData["Success"] = "Prévision supprimée.";
                            }
                            else
                            {
                                TempData["Error"] = "Impossible de supprimer : identifiant introuvable ou hors plage (0..255).";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Impossible de supprimer : identifiant introuvable.";
                        }
                    }
                }
                else
                {
                    // Suppression quantité : ton service SupprimerAsync attend un byte IdLivrablesProjet
                    if (idLivrablesProjet != null)
                    {
                        await _quantiteService.SupprimerAsync(idLivrablesProjet.Value);
                        TempData["Success"] = "Quantité supprimée.";
                    }
                    else
                    {
                        TempData["Error"] = "Impossible de supprimer la quantité : paramètres manquants.";
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur DeleteConfirmed Programmation");
                TempData["Error"] = "Erreur lors de la suppression.";
                return RedirectToAction(nameof(Index));
            }
        }

        // -----------------------
        // Helpers
        // -----------------------
        private static byte? ToByteSafe(int? value)
        {
            if (!value.HasValue) return null;
            if (value.Value < byte.MinValue || value.Value > byte.MaxValue) return null;
            return (byte)value.Value;
        }

        private async Task<PrevisionInformationFinanciereDto?> SafeObtenirPrevisionParId(byte id)
        {
            try
            {
                return await _previsionService.ObtenirParIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ObtenirParIdAsync(prevision) a échoué pour id={Id}", id);
                return null;
            }
        }

        private async Task<QuantiteALivrerParAnneeDto?> SafeObtenirQuantiteParId(byte id)
        {
            try
            {
                return await _quantiteService.ObtenirParIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ObtenirParIdAsync(quantite) a échoué pour id={Id}", id);
                return null;
            }
        }
    }
}
