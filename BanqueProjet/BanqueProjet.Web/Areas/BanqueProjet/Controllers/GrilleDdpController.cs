////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Mvc;
////using BanqueProjet.Web.Models;
////using BanqueProjet.Application.Dtos;
////using BanqueProjet.Application.Interfaces;

////namespace BanqueProjet.Web.Controllers
////{
////    [Area("BanqueProjet")]
////    public class GrilleDdpController : Controller
////    {
////        private readonly IGrilleDdpIdentificationProjetService _identificationService;
////        private readonly IGrilleDdpStrategieGestionProjetService _strategieService;
////        private readonly IGrilleDdpResumeProjetService _resumeService;
////        private readonly IGrilleDdpEtudesPrefaisabiliteService _etudesService;
////        private readonly IGrilleDdpCalendrierFinancierProjetService _calendrierFinancierService;
////        private readonly IGrilleDdpCalendrierExecutionService _calendrierExecutionService;
////        private readonly IGrilleDdpAspectsLegauxService _aspectsLegauxService;
////        private readonly IGrilleDdpCommentairesGenerauxService _commentairesService;
////        private readonly IGrilleDdpAviService _avisService;

////        public GrilleDdpController(
////            IGrilleDdpIdentificationProjetService identificationService,
////            IGrilleDdpStrategieGestionProjetService strategieService,
////            IGrilleDdpResumeProjetService resumeService,
////            IGrilleDdpEtudesPrefaisabiliteService etudesService,
////            IGrilleDdpCalendrierFinancierProjetService calendrierFinancierService,
////            IGrilleDdpCalendrierExecutionService calendrierExecutionService,
////            IGrilleDdpAspectsLegauxService aspectsLegauxService,
////            IGrilleDdpCommentairesGenerauxService commentairesService,
////            IGrilleDdpAviService avisService)
////        {
////            _identificationService = identificationService;
////            _strategieService = strategieService;
////            _resumeService = resumeService;
////            _etudesService = etudesService;
////            _calendrierFinancierService = calendrierFinancierService;
////            _calendrierExecutionService = calendrierExecutionService;
////            _aspectsLegauxService = aspectsLegauxService;
////            _commentairesService = commentairesService;
////            _avisService = avisService;
////        }

////        // GET: GrilleDdp/Create
////        [HttpGet("Create")]
////        public IActionResult Create()
////        {
////            var vm = new GrilleDDPViewModel();
////            return View(vm);
////        }

////        // POST: GrilleDdp/Create
////        [HttpPost("Create")]
////        public async Task<IActionResult> Create(GrilleDDPViewModel model)
////        {
////            if (!ModelState.IsValid)
////                return View(model);

////            await _identificationService.AjouterAsync(model.GDdpIdentificationProjets);
////            await _strategieService.AjouterAsync(model.GDdpStrategieGestion);
////            await _resumeService.AjouterAsync(model.GDdpResumeProjet);
////            await _etudesService.AjouterAsync(model.GDdpEtudePreFaisabilite);
////            await _calendrierFinancierService.AjouterAsync(model.GDdpCalendrierFinancier);
////            await _calendrierExecutionService.AjouterAsync(model.GDdpCalendrierExecution);
////            await _aspectsLegauxService.AjouterAsync(model.GDdpAspectsLegaux);
////            await _commentairesService.AjouterAsync(model.GDdpCommentaireGeneral);
////            await _avisService.AjouterAsync(model.GDdpAvis);

////            TempData["Success"] = "Grille DDP créée avec succès.";
////            return RedirectToAction(nameof(Edit), new { id = model.GDdpIdentificationProjets.IdIdentificationProjet });
////        }

////        // GET: GrilleDdp/Edit/{id}
////        [HttpGet("Edit/{id}")]
////        public async Task<IActionResult> Edit(string id)
////        {
////            var vm = new GrilleDDPViewModel
////            {
////                GDdpIdentificationProjets = await _identificationService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpIdentificationProjetDto { IdIdentificationProjet = id },
////                GDdpStrategieGestion = await _strategieService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpStrategieGestionProjetDto { IdIdentificationProjet = id },
////                GDdpResumeProjet = await _resumeService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpResumeProjetDto { IdIdentificationProjet = id },
////                GDdpEtudePreFaisabilite = await _etudesService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpEtudesPrefaisabiliteDto { IdIdentificationProjet = id },
////                GDdpCalendrierFinancier = await _calendrierFinancierService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpCalendrierFinancierProjetDto { IdIdentificationProjet = id },
////                GDdpCalendrierExecution = await _calendrierExecutionService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpCalendrierExecutionDto { IdIdentificationProjet = id },
////                GDdpAspectsLegaux = await _aspectsLegauxService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpAspectsLegauxDto { IdIdentificationProjet = id },
////                GDdpCommentaireGeneral = await _commentairesService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpCommentairesGenerauxDto { IdIdentificationProjet = id },
////                GDdpAvis = await _avisService.ObtenirParIdAsync(byte.Parse(id)) ?? new GrilleDdpAviDto { IdIdentificationProjet = id }
////            };
////            return View(vm);
////        }

////        // POST: GrilleDdp/Edit
////        [HttpPost("Edit")]
////        public async Task<IActionResult> Edit(GrilleDDPViewModel model)
////        {
////            if (!ModelState.IsValid)
////                return View(model);

////            // Identification
////            if (model.GDdpIdentificationProjets.IdGrilleDdpIdentificationProjet > 0)
////                await _identificationService.MettreAJourAsync(model.GDdpIdentificationProjets);
////            else
////                await _identificationService.AjouterAsync(model.GDdpIdentificationProjets);
////            // ... répéter pour chaque section comme avant ...

////            TempData["Success"] = "Grille DDP mise à jour avec succès.";
////            return RedirectToAction(nameof(Edit), new { id = model.GDdpIdentificationProjets.IdIdentificationProjet });
////        }

////        // GET: GrilleDdp/Details/{id}
////        [HttpGet("Details/{id}")]
////        public async Task<IActionResult> Details(string id)
////        {
////            var vm = await LoadViewModelAsync(id);
////            return View(vm);
////        }

////        // GET: GrilleDdp/Delete/{id}
////        [HttpGet("Delete/{id}")]
////        public IActionResult Delete(string id)
////        {
////            ViewBag.Id = id;
////            return View();
////        }

////        // POST: GrilleDdp/Delete/{id}
////        [HttpPost("Delete/{id}"), ActionName("Delete")]
////        public async Task<IActionResult> DeleteConfirmed(string id)
////        {
////            var key = byte.Parse(id);
////            await _identificationService.SupprimerAsync(key);
////            await _strategieService.SupprimerAsync(key);
////            await _resumeService.SupprimerAsync(key);
////            await _etudesService.SupprimerAsync(key);
////            await _calendrierFinancierService.SupprimerAsync(key);
////            await _calendrierExecutionService.SupprimerAsync(key);
////            await _aspectsLegauxService.SupprimerAsync(key);
////            await _commentairesService.SupprimerAsync(key);
////            await _avisService.SupprimerAsync(key);

////            TempData["Success"] = "Grille DDP supprimée avec succès.";
////            return RedirectToAction("Create");
////        }

////        // Helper to load all DTOs
////        private async Task<GrilleDDPViewModel> LoadViewModelAsync(string id)
////        {
////            var key = byte.Parse(id);
////            return new GrilleDDPViewModel
////            {
////                GDdpIdentificationProjets = await _identificationService.ObtenirParIdAsync(key) ?? new GrilleDdpIdentificationProjetDto { IdIdentificationProjet = id },
////                GDdpStrategieGestion = await _strategieService.ObtenirParIdAsync(key) ?? new GrilleDdpStrategieGestionProjetDto { IdIdentificationProjet = id },
////                GDdpResumeProjet = await _resumeService.ObtenirParIdAsync(key) ?? new GrilleDdpResumeProjetDto { IdIdentificationProjet = id },
////                GDdpEtudePreFaisabilite = await _etudesService.ObtenirParIdAsync(key) ?? new GrilleDdpEtudesPrefaisabiliteDto { IdIdentificationProjet = id },
////                GDdpCalendrierFinancier = await _calendrierFinancierService.ObtenirParIdAsync(key) ?? new GrilleDdpCalendrierFinancierProjetDto { IdIdentificationProjet = id },
////                GDdpCalendrierExecution = await _calendrierExecutionService.ObtenirParIdAsync(key) ?? new GrilleDdpCalendrierExecutionDto { IdIdentificationProjet = id },
////                GDdpAspectsLegaux = await _aspectsLegauxService.ObtenirParIdAsync(key) ?? new GrilleDdpAspectsLegauxDto { IdIdentificationProjet = id },
////                GDdpCommentaireGeneral = await _commentairesService.ObtenirParIdAsync(key) ?? new GrilleDdpCommentairesGenerauxDto { IdIdentificationProjet = id },
////                GDdpAvis = await _avisService.ObtenirParIdAsync(key) ?? new GrilleDdpAviDto { IdIdentificationProjet = id }
////            };
////        }
////    }
////}
