using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;

namespace Programmation.Web.Controllers
{
    [Area("Programmation")]
    public class ProgrammationController : Controller
    {
        private readonly IProgrammationProjetService _programmationService;
        private readonly IHypothesesEtRisquesService _hypothesesService;
        private readonly IInformationsFinancieresProgrammeesProjetService _infosFinService;
        private readonly ILivrablesProjetService _livrablesService;
        private readonly IPrevisionActiviteAnnuelleService _previsionService;
        private readonly IGestionEtSuiviService _gestionService;
        private readonly ILogger<ProgrammationController> _logger;

        public ProgrammationController(
            IProgrammationProjetService programmationService,
            IHypothesesEtRisquesService hypothesesService,
            IInformationsFinancieresProgrammeesProjetService infosFinService,
            ILivrablesProjetService livrablesService,
            IPrevisionActiviteAnnuelleService previsionService,
            IGestionEtSuiviService gestionService,
            ILogger<ProgrammationController> logger)
        {
            _programmationService = programmationService;
            _hypothesesService = hypothesesService;
            _infosFinService = infosFinService;
            _livrablesService = livrablesService;
            _previsionService = previsionService;
            _gestionService = gestionService;
            _logger = logger;
        }

        // GET: /Programmation
        public async Task<IActionResult> Index()
        {
            var list = await _programmationService.ObtenirTousAsync();
            return View(list);
        }

        // GET: /Programmation/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var main = await _programmationService.ObtenirParIdAsync(id);
            if (main == null)
                return NotFound();

            var vm = new ProgrammationViewModel
            {
                Programmation = main,
                Hypotheses = await _hypothesesService.ObtenirParIdentificationProjetAsync(id) is var h && h != null
                                   ? new List<HypothesesEtRisquesDto> { h }
                                   : new List<HypothesesEtRisquesDto>(),
                InformationsFinancieres = await _infosFinService.ObtenirParProjetAsync(id),
                Livrables = await _livrablesService.ObtenirParProjetAsync(id),
                Previsions = await _previsionService.ObtenirParIdentificationProjetAsync(id) is var p && p != null
                                   ? new List<PrevisionActiviteAnnuelleDto> { p }
                                   : new List<PrevisionActiviteAnnuelleDto>(),
                Gestion = await _gestionService.ObtenirParIdentificationProjetAsync(id) is var g && g != null
                                   ? new List<GestionEtSuiviProjetDto> { g }
                                   : new List<GestionEtSuiviProjetDto>()
            };

            return View(vm);
        }

        // GET: /Programmation/Create?projetId=XYZ
        public IActionResult Create([FromQuery] string projetId)
        {
            if (string.IsNullOrWhiteSpace(projetId))
                return BadRequest("IdIdentificationProjet manquant.");

            var vm = new ProgrammationViewModel
            {
                Programmation = new ProgrammationProjetDto
                {
                    IdIdentificationProjet = projetId
                },
                Hypotheses = new List<HypothesesEtRisquesDto>(),
                InformationsFinancieres = new List<InformationsFinancieresProgrammeesProjetDto>(),
                Livrables = new List<LivrablesProgrameProjetDto>(),
                Previsions = new List<PrevisionActiviteAnnuelleDto>(),
                Gestion = new List<GestionEtSuiviProjetDto>()
            };

            return View(vm);
        }

        // POST: /Programmation/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgrammationViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                _logger.LogInformation("🚀 [CTRL] Création Programmation pour projet {Id}", vm.Programmation.IdIdentificationProjet);

                // 1) Créer le volet principal
                await _programmationService.AjouterAsync(vm.Programmation);

                // 2) Créer chaque partie imbriquée
                foreach (var h in vm.Hypotheses)
                    await _hypothesesService.AjouterAsync(h);

                foreach (var info in vm.InformationsFinancieres)
                    await _infosFinService.AjouterAsync(info);

                foreach (var l in vm.Livrables)
                    await _livrablesService.AjouterAsync(l);

                foreach (var p in vm.Previsions)
                    await _previsionService.AjouterAsync(p);

                foreach (var g in vm.Gestion)
                    await _gestionService.AjouterAsync(g);

                TempData["Success"] = "Programmation créée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la programmation");
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        // GET: /Programmation/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var main = await _programmationService.ObtenirParIdAsync(id);
            if (main == null)
                return NotFound();

            var vm = new ProgrammationViewModel
            {
                Programmation = main,
                Hypotheses = await _hypothesesService.ObtenirParIdentificationProjetAsync(id) is var h && h != null
                                   ? new List<HypothesesEtRisquesDto> { h }
                                   : new List<HypothesesEtRisquesDto>(),
                InformationsFinancieres = await _infosFinService.ObtenirParProjetAsync(id),
                Livrables = await _livrablesService.ObtenirParProjetAsync(id),
                Previsions = await _previsionService.ObtenirParIdentificationProjetAsync(id) is var p && p != null
                                   ? new List<PrevisionActiviteAnnuelleDto> { p }
                                   : new List<PrevisionActiviteAnnuelleDto>(),
                Gestion = await _gestionService.ObtenirParIdentificationProjetAsync(id) is var g && g != null
                                   ? new List<GestionEtSuiviProjetDto> { g }
                                   : new List<GestionEtSuiviProjetDto>()
            };
            return View(vm);
        }

        // POST: /Programmation/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProgrammationViewModel vm)
        {
            if (id != vm.Programmation.IdIdentificationProjet)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                _logger.LogInformation("✏️ [CTRL] Mise à jour Programmation pour projet {Id}", id);

                await _programmationService.MettreAJourAsync(vm.Programmation);

                foreach (var h in vm.Hypotheses)
                    await _hypothesesService.MettreAJourAsync(h);

                foreach (var info in vm.InformationsFinancieres)
                    await _infosFinService.MettreAJourAsync(info);

                foreach (var l in vm.Livrables)
                    await _livrablesService.MettreAJourAsync(l);

                foreach (var p in vm.Previsions)
                    await _previsionService.MettreAJourAsync(p);

                foreach (var g in vm.Gestion)
                    await _gestionService.MettreAJourAsync(g);

                TempData["Success"] = "Programmation mise à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la programmation");
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        // GET: /Programmation/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var main = await _programmationService.ObtenirParIdAsync(id);
            if (main == null)
                return NotFound();

            return View(main);
        }

        // POST: /Programmation/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                _logger.LogInformation("🗑️ [CTRL] Suppression Programmation pour projet {Id}", id);

                // Supprimer d'abord chaque partie imbriquée
                if (await _hypothesesService.ObtenirParIdentificationProjetAsync(id) is var h && h != null)
                    await _hypothesesService.SupprimerAsync(h.IdHypothesesEtRisques);

                foreach (var info in await _infosFinService.ObtenirParProjetAsync(id))
                    await _infosFinService.SupprimerAsync(info.IdInformationsFinancieres);

                foreach (var l in await _livrablesService.ObtenirParProjetAsync(id))
                    await _livrablesService.SupprimerAsync(l.IdLivrablesProjet);

                if (await _previsionService.ObtenirParIdentificationProjetAsync(id) is var p && p != null)
                    await _previsionService.SupprimerAsync(p.IdActivitesAnnuelles);

                if (await _gestionService.ObtenirParIdentificationProjetAsync(id) is var g && g != null)
                    await _gestionService.SupprimerAsync(g.IdGestionDeProjetEtSuivi);

                // Enfin supprimer la programmation principale
                await _programmationService.SupprimerAsync(id);

                TempData["Success"] = "Programmation supprimée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la programmation");
                return RedirectToAction(nameof(Delete), new { id, error = ex.Message });
            }
        }
    }


}
