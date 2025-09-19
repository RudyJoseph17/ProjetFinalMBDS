using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;
using Newtonsoft.Json;
using BanqueProjet.Web.Models;
using Shared.Domain.Dtos;
using Shared.Domain.Helpers;

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
        private readonly ILogger<IdentificationProjetController> _logger;
        private const int TotalSteps = 6;

        public IdentificationProjetController(
            IProjetsBPService projetService,
            IDdpCadreLogiqueService cadreService,
            IAspectsJuridiquesService aspectsJuridiquesService,
            ILocalisationGeographiqueProjService localisationService,
            IGrilleDdpProjetService grilleService,
            ILogger<IdentificationProjetController> logger)
        {
            _projetService = projetService;
            _cadreService = cadreService;
            _aspectsJuridiquesService = aspectsJuridiquesService;
            _localisationService = localisationService;
            _grilleService = grilleService;
            _logger = logger;
        }

        #region CRUD Standard

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

            // Récupération du projet principal
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null)
                return NotFound();

            // Création du ViewModel
            var vm = new DdpViewModel
            {
                Projets = projet,
                CadreLogique = await _cadreService.ObtenirParIdAsync(id),
                //ActiviteBP = await _grilleService.ObtenirParIdAsync(id),
                //AspectsJuridiques = await _aspectsJuridiquesService.ObtenirParIdAsync(id),
                //LocalisationGeographique = await _localisationService.ObtenirParIdAsync(id)
            };

            // Propagation de l'ID dans toutes les sous-entités
            EnsureProjectId(vm);

            return View(vm);
        }




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
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();
            return View(projet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjetsBPDto model)
        {
            if (!ModelState.IsValid) return View(model);
            EnsureProjectId(model);
            await _projetService.MettreAJourAsync(model);
            TempData["SuccessMessage"] = "Projet mis à jour avec succès !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();
            return View(projet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _projetService.SupprimerAsync(id);
            TempData["SuccessMessage"] = "Projet supprimé avec succès !";
            return RedirectToAction(nameof(Index));
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
                    LocalisationGeographique = new LocalisationGeographiqueProjDto { IdIdentificationProjet = newId }
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

                _logger.LogInformation("🚨 Vérification avant AJOUTER_DDP_CADRE_LOGIQUE_JSON : {Id}", stored.CadreLogique.IdIdentificationProjet);

                await _projetService.AjouterAsync(stored.Projets);
                await _cadreService.AjouterAsync(stored.CadreLogique);
                foreach (var dto in stored.AspectsJuridiques)
                    await _aspectsJuridiquesService.AjouterAsync(dto);
                await _localisationService.AjouterAsync(stored.LocalisationGeographique);

                TempData.Remove("WizardModel");
                TempData["SuccessMessage"] = "Projet ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Wizard), new { step });
        }

        #endregion

        #region Helpers

        #region Helpers

        // ... vos autres helpers ...

        /// <summary>
        /// Convertit un Id de type string en byte[] (pour les services qui attendent un RAW ou byte[])
        /// </summary>
        private byte[] ConvertIdStringToBytes(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Array.Empty<byte>();
            // UTF8 est suffisant si votre ID contient des caractères ASCII/UTF8
            return System.Text.Encoding.UTF8.GetBytes(id);
        }

        #endregion

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

                    // TODO: cases 3 à 6 si nécessaire
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

        #endregion
    }
}
