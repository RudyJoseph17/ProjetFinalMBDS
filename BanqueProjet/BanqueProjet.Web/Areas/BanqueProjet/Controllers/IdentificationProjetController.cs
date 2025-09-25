using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Transactions;

using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Web.Models;
using Shared.Domain.Helpers;
using Shared.Domain.Dtos;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class IdentificationProjetController : Controller
    {
        private readonly IProjetsBPService _projetService;
        private readonly IDdpCadreLogiqueService _cadreService;
        private readonly IAspectsJuridiquesService _aspectsJuridiquesService;
        private readonly ILocalisationGeographiqueProjService _localisationService;
        private readonly IGrilleDdpProjetService _grilleService;
        private readonly IPartiesPrenantesService _partiesPrenantesService;
        private readonly IIndicateursDeResultatService _indicateursService;
        private readonly IDefinitionLivrablesDuProjetService _livrablesService;
        private readonly IEffetsDuProjetService _effetsService;
        private readonly IObjectifsSpecifiquesService _objectifsService;
        private readonly IImpactsDuProjetService _impactsService;
        private readonly IBailleursDeFondService _bailleursService;
        private readonly ICoutAnnuelDuProjetService _coutService;
        private readonly IActivitesAnnuellesService _activitesAnnuellesService;
        private readonly IActiviteBPService _activiteBPService;

        private readonly ILogger<IdentificationProjetController> _logger;
        private const int TotalSteps = 6;

        public IdentificationProjetController(
            IProjetsBPService projetService,
            IDdpCadreLogiqueService cadreService,
            IAspectsJuridiquesService aspectsJuridiquesService,
            ILocalisationGeographiqueProjService localisationService,
            IGrilleDdpProjetService grilleService,
            IPartiesPrenantesService partiesPrenantesService,
            IIndicateursDeResultatService indicateursService,
            IDefinitionLivrablesDuProjetService livrablesService,
            IEffetsDuProjetService effetsService,
            IObjectifsSpecifiquesService objectifsService,
            IImpactsDuProjetService impactsService,
            IBailleursDeFondService bailleursService,
            ICoutAnnuelDuProjetService coutService,
            IActivitesAnnuellesService activitesAnnuellesService,
            IActiviteBPService activiteBPService,
            ILogger<IdentificationProjetController> logger)
        {
            _projetService = projetService;
            _cadreService = cadreService;
            _aspectsJuridiquesService = aspectsJuridiquesService;
            _localisationService = localisationService;
            _grilleService = grilleService;
            _partiesPrenantesService = partiesPrenantesService;
            _indicateursService = indicateursService;
            _livrablesService = livrablesService;
            _effetsService = effetsService;
            _objectifsService = objectifsService;
            _impactsService = impactsService;
            _bailleursService = bailleursService;
            _coutService = coutService;
            _activitesAnnuellesService = activitesAnnuellesService;
            _activiteBPService = activiteBPService;
            _logger = logger;
        }

        #region CRUD Standard

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projets = await _projetService.ObtenirTousAsync();

            return View(projets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null)
                return NotFound();

            var vm = new DdpViewModel
            {
                Projets = projet,
                CadreLogique = await _cadreService.ObtenirParIdentificationProjetAsync(id),

                // Collections filtrées par IdIdentificationProjet (là où c’est possible)
                AspectsJuridiques = (await _aspectsJuridiquesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                PartiesPrenantesProjets = (await _partiesPrenantesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                IndicateursResultats = (await _indicateursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                DefinitionLivrables = (await _livrablesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                EffetsProjets = (await _effetsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ObjectifsSpecifiques = (await _objectifsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ImpactsDuProjet = (await _impactsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                BailleursDeFonds = (await _bailleursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                CoutAnnuelDuProjet = (await _coutService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ActivitesAnnuelles = (await _activitesAnnuellesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                // Activités BP (shared)
                ActiviteBP = await _activiteBPService.ObtenirTousAsync(),

                // Localisation : pas de méthode par projet, on filtre sur les vues si nécessaire
                LocalisationGeographique = (await _localisationService.ObtenirTousAsync())
                    .FirstOrDefault(x => x.IdIdentificationProjet == id)
                    ?? new LocalisationGeographiqueProjDto { IdIdentificationProjet = id }
            };

            EnsureProjectId(vm);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProjetsBPDto
            {
                IdIdentificationProjet = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet))
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjetsBPDto model)
        {
            if (!ModelState.IsValid) return View(model);
            EnsureProjectId(model);

            await _projetService.AjouterAsync(model);
            TempData["SuccessMessage"] = "Projet créé avec succès !";
            // après avoir persisté ProjetsBPDto…
            var cadre = new DdpCadreLogiqueDto { IdIdentificationProjet = model.IdIdentificationProjet };
            await _cadreService.AjouterAsync(cadre);
            return RedirectToAction(nameof(Index));
        }

        // ======= EDIT (Get) : charge projet + sous-entités =======
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();

            var vm = new DdpViewModel
            {
                Projets = projet,
                CadreLogique = await _cadreService.ObtenirParIdentificationProjetAsync(id),

                AspectsJuridiques = (await _aspectsJuridiquesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                PartiesPrenantesProjets = (await _partiesPrenantesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                IndicateursResultats = (await _indicateursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                DefinitionLivrables = (await _livrablesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                EffetsProjets = (await _effetsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ObjectifsSpecifiques = (await _objectifsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ImpactsDuProjet = (await _impactsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                BailleursDeFonds = (await _bailleursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                CoutAnnuelDuProjet = (await _coutService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                ActivitesAnnuelles = (await _activitesAnnuellesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),

                LocalisationGeographique = (await _localisationService.ObtenirTousAsync())
                    .FirstOrDefault(x => x.IdIdentificationProjet == id)
                    ?? new LocalisationGeographiqueProjDto { IdIdentificationProjet = id }
            };

            EnsureProjectId(vm);
            return View(vm);
        }

        // ======= EDIT (Post) : met à jour projet + sous-entités via services =======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DdpViewModel model)
        {
            if (model?.Projets == null || id != model.Projets.IdIdentificationProjet)
                return BadRequest();

            if (!ModelState.IsValid) return View(model);

            EnsureProjectId(model);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Projet principal
                await _projetService.MettreAJourAsync(model.Projets);

                // Cadre logique
                if (model.CadreLogique != null)
                {
                    model.CadreLogique.IdIdentificationProjet = model.Projets.IdIdentificationProjet;
                    await _cadreService.MettreAJourAsync(model.CadreLogique);
                }

                // Collections (une par une)
                await PersistEntitiesAsync(model.AspectsJuridiques, _aspectsJuridiquesService.MettreAJourAsync);
                await PersistEntitiesAsync(model.PartiesPrenantesProjets, _partiesPrenantesService.MettreAJourAsync);
                await PersistEntitiesAsync(model.IndicateursResultats, _indicateursService.MettreAJourAsync);
                await PersistEntitiesAsync(model.DefinitionLivrables, _livrablesService.MettreAJourAsync);
                await PersistEntitiesAsync(model.EffetsProjets, _effetsService.MettreAJourAsync);
                await PersistEntitiesAsync(model.ObjectifsSpecifiques, _objectifsService.MettreAJourAsync);
                await PersistEntitiesAsync(model.ImpactsDuProjet, _impactsService.MettreAJourAsync);
                await PersistEntitiesAsync(model.BailleursDeFonds, _bailleursService.MettreAJourAsync);
                await PersistEntitiesAsync(model.CoutAnnuelDuProjet, _coutService.MettreAJourAsync);
                await PersistEntitiesAsync(model.ActivitesAnnuelles, _activitesAnnuellesService.MettreAJourAsync);

                // Localisation (singlet)
                if (model.LocalisationGeographique != null)
                {
                    model.LocalisationGeographique.IdIdentificationProjet = model.Projets.IdIdentificationProjet;
                    await _localisationService.MettreAJourAsync(model.LocalisationGeographique);
                }

                scope.Complete();
                TempData["SuccessMessage"] = "Projet mis à jour avec succès !";
                return RedirectToAction(nameof(Details), new { id = model.Projets.IdIdentificationProjet });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du projet {ProjetId}", id);
                TempData["ErrorMessage"] = "Erreur lors de la mise à jour.";
                return View(model);
            }
        }

        // ======= DELETE (Get) : confirmation =======
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();
            return View(projet);
        }

        // ======= DELETE (Post) : supprime projet et (optionnel) sous-entités =======
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Chargement des sous-entités pour suppression unitaire (si leurs IDs sont disponibles)
                var aspects = (await _aspectsJuridiquesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var parties = (await _partiesPrenantesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var indicateurs = (await _indicateursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var livrables = (await _livrablesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var effets = (await _effetsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var objectifs = (await _objectifsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var impacts = (await _impactsService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var bailleurs = (await _bailleursService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var couts = (await _coutService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var annuelles = (await _activitesAnnuellesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList();
                var loc = (await _localisationService.ObtenirTousAsync())
                    .FirstOrDefault(x => x.IdIdentificationProjet == id);

                // Suppressions unitaires basées sur les IDs byte des DTOs
                await DeleteEntitiesAsync(aspects, dto => _aspectsJuridiquesService.SupprimerAsync(dto.IdAspectsJuridiques));
                await DeleteEntitiesAsync(parties, dto => _partiesPrenantesService.SupprimerAsync(dto.IdPartiesPrenantes));
                await DeleteEntitiesAsync(indicateurs, dto => _indicateursService.SupprimerAsync(dto.IdIndicateursDeResultats));
                await DeleteEntitiesAsync(livrables, dto => _livrablesService.SupprimerAsync(dto.IdLivrablesProjet));
                await DeleteEntitiesAsync(effets, dto => _effetsService.SupprimerAsync(dto.IdEffetsDuProjet));
                await DeleteEntitiesAsync(objectifs, dto => _objectifsService.SupprimerAsync(dto.IdObjectifsSpecifiques));
                await DeleteEntitiesAsync(impacts, dto => _impactsService.SupprimerAsync(dto.IdImpactsProjet));
                await DeleteEntitiesAsync(bailleurs, dto => _bailleursService.SupprimerAsync(dto.IdBailleursDeFonds));
                await DeleteEntitiesAsync(couts, dto => _coutService.SupprimerAsync(dto.IdCoutAnnuelProjet));
                await DeleteEntitiesAsync(annuelles, dto => _activitesAnnuellesService.SupprimerAsync(dto.IdActivitesAnnuelles));

                if (loc != null)
                    await _localisationService.SupprimerAsync(loc.IdLocalisationGeographique);

                // Projet principal
                await _projetService.SupprimerAsync(id);

                scope.Complete();
                TempData["SuccessMessage"] = "Projet supprimé avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du projet {ProjetId}", id);
                TempData["ErrorMessage"] = "Erreur lors de la suppression.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        #endregion

        #region Wizard

        [HttpGet]
        public IActionResult Wizard(int step = 1)
        {
            DdpViewModel model;
            if (TempData.ContainsKey("WizardModel"))
            {
                var serialized = TempData.Peek("WizardModel")?.ToString();
                model = JsonConvert.DeserializeObject<DdpViewModel>(serialized);
                EnsureProjectId(model);
            }
            else
            {
                var newId = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
                model = new DdpViewModel
                {
                    Projets = new ProjetsBPDto { IdIdentificationProjet = newId },
                    CadreLogique = new DdpCadreLogiqueDto { IdIdentificationProjet = newId },
                    ActiviteBP = new List<ActiviteBPDto> { new ActiviteBPDto { IdIdentificationProjet = newId } },
                    AspectsJuridiques = new List<AspectsJuridiquesDto> { new AspectsJuridiquesDto { IdIdentificationProjet = newId } },
                    LocalisationGeographique = new LocalisationGeographiqueProjDto { IdIdentificationProjet = newId },
                    PartiesPrenantesProjets = new List<PartiesPrenantesDto>(),
                    IndicateursResultats = new List<IndicateursDeResultatDto>(),
                    DefinitionLivrables = new List<DefinitionLivrablesDuProjetDto>(),
                    EffetsProjets = new List<EffetsDuProjetDto>(),
                    ObjectifsSpecifiques = new List<ObjectifsSpecifiquesDto>(),
                    ImpactsDuProjet = new List<ImpactsDuProjetDto>(),
                    BailleursDeFonds = new List<BailleursDeFondsDto>(),
                    CoutAnnuelDuProjet = new List<CoutAnnuelDuProjetDto>(),
                    ActivitesAnnuelles = new List<ActivitesAnnuellesDto>()
                };
            }

            TempData.Put("WizardModel", model);
            ViewData["Step"] = step;
            return View($"WizardStep{step}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wizard(DdpViewModel model, int step, string action)
        {
            EnsureProjectId(model);

            var stored = TempData.Get<DdpViewModel>("WizardModel") ?? new DdpViewModel();
            EnsureProjectId(stored);

            MergeStepData(stored, model, step);
            PropagateAllLists(stored);
            TempData.Put("WizardModel", stored);

            if (action == "Prev" && step > 1)
                return RedirectToAction(nameof(Wizard), new { step = step - 1 });

            if (action == "Next" && step < TotalSteps)
                return RedirectToAction(nameof(Wizard), new { step = step + 1 });

            if (action == "Finish")
            {
                stored.CadreLogique ??= new DdpCadreLogiqueDto();
                stored.CadreLogique.IdIdentificationProjet = stored.Projets.IdIdentificationProjet;

                _logger.LogInformation("🚨 Vérification avant persistance transactionnelle : {Id}", stored.CadreLogique.IdIdentificationProjet);

                await PersistAllAsync(stored);

                TempData.Remove("WizardModel");
                TempData["SuccessMessage"] = "Projet ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Wizard), new { step });
        }

        #endregion

        #region Helpers

        private void MergeStepData(DdpViewModel stored, DdpViewModel current, int step)
        {
            switch (step)
            {
                case 1:
                    stored.Projets.NomProjet = current.Projets.NomProjet;
                    stored.Projets.Ministere = current.Projets.Ministere;
                    stored.Projets.Section = current.Projets.Section;
                    stored.Projets.CodePip = current.Projets.CodePip;
                    stored.Projets.CodeBailleur = current.Projets.CodeBailleur;
                    stored.Projets.NomDirecteurDeProjet = current.Projets.NomDirecteurDeProjet;
                    stored.Projets.TelephoneDirecteurDeProjet = current.Projets.TelephoneDirecteurDeProjet;
                    stored.Projets.CourrielDirecteurDeProjet = current.Projets.CourrielDirecteurDeProjet;
                    stored.Projets.DateInscription = current.Projets.DateInscription;
                    stored.Projets.DateMiseAJour = current.Projets.DateMiseAJour;
                    break;

                case 2:
                    stored.Projets.Programme = current.Projets.Programme;
                    stored.Projets.SousProgramme = current.Projets.SousProgramme;
                    stored.Projets.SecteurDActivites = current.Projets.SecteurDActivites;
                    stored.Projets.SousSecteurDActivites = current.Projets.SousSecteurDActivites;
                    stored.Projets.TypeDeProjet = current.Projets.TypeDeProjet;
                    stored.Projets.EchelonTerritorial = current.Projets.EchelonTerritorial;

                    stored.LocalisationGeographique.Departement = current.LocalisationGeographique.Departement;
                    stored.LocalisationGeographique.Arrondissement = current.LocalisationGeographique.Arrondissement;
                    stored.LocalisationGeographique.Commune = current.LocalisationGeographique.Commune;
                    stored.LocalisationGeographique.SectionCommunale = current.LocalisationGeographique.SectionCommunale;
                    break;

                case 3:
                    stored.PartiesPrenantesProjets = current.PartiesPrenantesProjets ?? new List<PartiesPrenantesDto>();
                    stored.AspectsJuridiques = current.AspectsJuridiques ?? new List<AspectsJuridiquesDto>();
                    break;

                case 4:
                    stored.IndicateursResultats = current.IndicateursResultats ?? new List<IndicateursDeResultatDto>();
                    stored.DefinitionLivrables = current.DefinitionLivrables ?? new List<DefinitionLivrablesDuProjetDto>();
                    break;

                case 5:
                    stored.EffetsProjets = current.EffetsProjets ?? new List<EffetsDuProjetDto>();
                    stored.ObjectifsSpecifiques = current.ObjectifsSpecifiques ?? new List<ObjectifsSpecifiquesDto>();
                    break;

                case 6:
                    stored.ImpactsDuProjet = current.ImpactsDuProjet ?? new List<ImpactsDuProjetDto>();
                    stored.BailleursDeFonds = current.BailleursDeFonds ?? new List<BailleursDeFondsDto>();
                    stored.CoutAnnuelDuProjet = current.CoutAnnuelDuProjet ?? new List<CoutAnnuelDuProjetDto>();
                    stored.ActivitesAnnuelles = current.ActivitesAnnuelles ?? new List<ActivitesAnnuellesDto>();
                    break;
            }
        }

        private void EnsureProjectId(ProjetsBPDto model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.IdIdentificationProjet))
                model.IdIdentificationProjet = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
        }

        private void EnsureProjectId(DdpViewModel model)
        {
            if (model == null) return;

            model.Projets ??= new ProjetsBPDto();
            model.CadreLogique ??= new DdpCadreLogiqueDto();
            model.AspectsJuridiques ??= new List<AspectsJuridiquesDto>();
            model.LocalisationGeographique ??= new LocalisationGeographiqueProjDto();

            if (string.IsNullOrWhiteSpace(model.Projets.IdIdentificationProjet))
                model.Projets.IdIdentificationProjet = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));

            model.CadreLogique.IdIdentificationProjet = model.Projets.IdIdentificationProjet;
            foreach (var dto in model.AspectsJuridiques)
                dto.IdIdentificationProjet = model.Projets.IdIdentificationProjet;

            model.LocalisationGeographique.IdIdentificationProjet = model.Projets.IdIdentificationProjet;

            PropagateAllLists(model);
        }

        private void PropagateAllLists(DdpViewModel model)
        {
            var id = model.Projets.IdIdentificationProjet;
            PropagateIdToList(model.Projets?.Activites, id);
            PropagateIdToList(model.Projets?.AspectsJuridiques, id);
            PropagateIdToList(model.Projets?.PartiesPrenantes, id);
            PropagateIdToList(model.Projets?.IndicateursDeResultats, id);
            PropagateIdToList(model.Projets?.LivrablesProjets, id);
            PropagateIdToList(model.Projets?.EffetsProjets, id);
            PropagateIdToList(model.Projets?.ObjectifsSpecifiques, id);
            PropagateIdToList(model.Projets?.ImpactsDesProjets, id);
            PropagateIdToList(model.Projets?.BailleursDeFonds, id);
            PropagateIdToList(model.Projets?.ActivitesAnnuelles, id);
            PropagateIdToList(model.Projets?.CoutAnnuelDuProjet, id);

            PropagateIdToList(model.AspectsJuridiques, id);
            PropagateIdToList(model.PartiesPrenantesProjets, id);
            PropagateIdToList(model.IndicateursResultats, id);
            PropagateIdToList(model.DefinitionLivrables, id);
            PropagateIdToList(model.EffetsProjets, id);
            PropagateIdToList(model.ObjectifsSpecifiques, id);
            PropagateIdToList(model.ImpactsDuProjet, id);
            PropagateIdToList(model.BailleursDeFonds, id);
            PropagateIdToList(model.ActivitesAnnuelles, id);
            PropagateIdToList(model.CoutAnnuelDuProjet, id);
        }

        private void PropagateIdToList<T>(IEnumerable<T> list, string id) where T : class
        {
            if (list == null || string.IsNullOrWhiteSpace(id)) return;
            foreach (var item in list)
            {
                var prop = item.GetType().GetProperty(nameof(IdentificationProjetDto.IdIdentificationProjet));
                if (prop != null && prop.CanWrite)
                    prop.SetValue(item, id);
            }
        }

        private async Task PersistEntitiesAsync<TDto>(
            IEnumerable<TDto> entities,
            Func<TDto, Task> persistFunc)
        {
            if (entities == null) return;
            foreach (var entity in entities)
            {
                await persistFunc(entity);
            }
        }

        private async Task PersistAllAsync(DdpViewModel storedViewModel)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Projet principal
                await _projetService.AjouterAsync(storedViewModel.Projets);

                // Cadre logique
                if (storedViewModel.CadreLogique != null)
                {
                    storedViewModel.CadreLogique.IdIdentificationProjet = storedViewModel.Projets.IdIdentificationProjet;
                    await _cadreService.AjouterAsync(storedViewModel.CadreLogique);
                }

                // Collections
                await PersistEntitiesAsync(storedViewModel.AspectsJuridiques, _aspectsJuridiquesService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.PartiesPrenantesProjets, _partiesPrenantesService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.IndicateursResultats, _indicateursService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.DefinitionLivrables, _livrablesService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.EffetsProjets, _effetsService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.ObjectifsSpecifiques, _objectifsService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.ImpactsDuProjet, _impactsService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.BailleursDeFonds, _bailleursService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.CoutAnnuelDuProjet, _coutService.AjouterAsync);
                await PersistEntitiesAsync(storedViewModel.ActivitesAnnuelles, _activitesAnnuellesService.AjouterAsync);

                // Localisation (singlet)
                if (storedViewModel.LocalisationGeographique != null)
                {
                    storedViewModel.LocalisationGeographique.IdIdentificationProjet = storedViewModel.Projets.IdIdentificationProjet;
                    await _localisationService.AjouterAsync(storedViewModel.LocalisationGeographique);
                }

                scope.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la persistance transactionnelle du projet {Projet}", storedViewModel.Projets?.NomProjet);
                throw; // rollback automatique
            }
        }

        private async Task DeleteEntitiesAsync<TDto>(IEnumerable<TDto> entities, Func<TDto, Task> deleteFunc)
        {
            if (entities == null) return;
            foreach (var entity in entities)
            {
                await deleteFunc(entity);
            }
        }

        // Overloads spécifiques pour supprimer à partir des IDs byte des DTOs
        private async Task DeleteEntitiesAsync(IEnumerable<AspectsJuridiquesDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById((byte)dto.IdAspectsJuridiques);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<PartiesPrenantesDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById(dto.IdPartiesPrenantes);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<IndicateursDeResultatDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById(dto.IdIndicateursDeResultats);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<DefinitionLivrablesDuProjetDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById(dto.IdLivrablesProjet);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<EffetsDuProjetDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById(dto.IdEffetsDuProjet);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<ObjectifsSpecifiquesDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById((byte)dto.IdObjectifsSpecifiques);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<ImpactsDuProjetDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById(dto.IdImpactsProjet);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<BailleursDeFondsDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById((byte)dto.IdBailleursDeFonds);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<CoutAnnuelDuProjetDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById((byte)dto.IdCoutAnnuelProjet);
        }
        private async Task DeleteEntitiesAsync(IEnumerable<ActivitesAnnuellesDto> list, Func<byte, Task> deleteById)
        {
            foreach (var dto in list) await deleteById((byte)dto.IdActivitesAnnuelles);
        }

        #endregion

        public async Task<IActionResult> DetailsForSuivi(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();

            var vr = new DdpViewModel
            {
                Projets = projet,
                CadreLogique = await _cadreService.ObtenirParIdentificationProjetAsync(id),
                AspectsJuridiques = (await _aspectsJuridiquesService.ObtenirTousAsync())
                    .Where(x => x.IdIdentificationProjet == id).ToList(),
                LocalisationGeographique = (await _localisationService.ObtenirTousAsync())
                    .FirstOrDefault(x => x.IdIdentificationProjet == id)
            };

            EnsureProjectId(vr);
            return View("DetailsForSuivi", vr);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // 1. Charger tous les projets
            var projets = await _projetService.ObtenirTousAsync();

            // 2. Calculer ministères + comptes
            var ministeres = projets
                .Where(p => !string.IsNullOrWhiteSpace(p.Ministere))
                .Select(p => p.Ministere!)
                .Distinct()
                .ToList();
            var counts = ministeres
                .Select(m => projets.Count(p => p.Ministere == m))
                .ToList();

            // 3. Construire le VM
            var vm = new DashboardStatsViewModel
            {
                TotalProjets = projets.Count,
                ProjetsValides = 0,           // ou calculez-les
                Ministeres = ministeres,
                Counts = counts
            };

            // 4. Sérialiser pour Chart.js
            ViewData["Ministeres"] = JsonConvert.SerializeObject(ministeres);
            ViewData["Counts"] = JsonConvert.SerializeObject(counts);

            // 5. Renvoyer la vue Dashboard.cshtml
            return View("Dashboard", vm);
        }

    }
}
