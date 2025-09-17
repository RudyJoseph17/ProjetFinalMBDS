using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;
using Newtonsoft.Json;
using Shared.Domain.Helpers;
using BanqueProjet.Web.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class IdentificationProjetController : Controller
    {
        private readonly IProjetsBPService _projetService;
        private readonly IDdpCadreLogiqueService _cadreService;
        private readonly IAspectsJuridiquesService _aspectsJuridiquesService;
        private readonly ILocalisationGeographiqueProjService _LocalisationGeographiqueService;
        //private readonly INotificationService _notif;
        private readonly ILogger<IdentificationProjetController> _logger;
        private const int TotalSteps = 6;

        public IdentificationProjetController(
            IProjetsBPService projetService,
            IDdpCadreLogiqueService cadreService,
            IAspectsJuridiquesService aspectsJuridiquesService,
            ILocalisationGeographiqueProjService LocalisationGeographiqueService,
            ILogger<IdentificationProjetController> logger)
        {
            _projetService = projetService;
            _cadreService = cadreService;
            _aspectsJuridiquesService = aspectsJuridiquesService;
            _LocalisationGeographiqueService = LocalisationGeographiqueService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Wizard(int step = 1)
        {
            DdpViewModel model;
            if (TempData.ContainsKey("WizardModel"))
            {
                var serialized = TempData.Peek("WizardModel").ToString();
                model = JsonConvert.DeserializeObject<DdpViewModel>(serialized);

                // s'assurer que l'ID existe même en cas de reprise de session
                EnsureProjectId(model);
            }
            else
            {
                var newId = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
                model = new DdpViewModel
                {
                    Projets = new ProjetsBPDto { IdIdentificationProjet = newId },
                    CadreLogique = new DdpCadreLogiqueDto { IdIdentificationProjet = newId },
                    ActiviteBP = new List<ActiviteBPDto>
                    {
                    new ActiviteBPDto { IdIdentificationProjet = newId } 
                    },
                    AspectsJuridiques = new List<AspectsJuridiquesDto>
                    {
                    new AspectsJuridiquesDto { IdIdentificationProjet = newId } 
                    },
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
            // 0) ID garanti sur current
            EnsureProjectId(model);

            // 1) Chargement de l’état complet précédent
            var stored = TempData.Get<DdpViewModel>("WizardModel")
                         ?? new DdpViewModel();
            EnsureProjectId(stored);

            // 2) Fusion des données de l’étape courante
            MergeStepData(stored, model, step);

            // 3) Propagation de l’ID sur les listes enfants
            PropagateAllLists(stored);

            // 4) Sauvegarde de l’état complet fusionné
            TempData.Put("WizardModel", stored);

            // 5) Navigation
            if (action == "Prev" && step > 1)
                return RedirectToAction(nameof(Wizard), new { step = step - 1 });

            if (action == "Next" && step < TotalSteps)
                return RedirectToAction(nameof(Wizard), new { step = step + 1 });

            // 6) Finish
            if (action == "Finish")
            {
                // Diagnostics
                _logger.LogInformation("=== Création projet {Id} ===", stored.Projets.IdIdentificationProjet);
                _logger.LogInformation("NomProjet: {Nom}", stored.Projets.NomProjet);
                _logger.LogInformation("JSON envoyé aux procédures:\n{Json}",
                    JsonConvert.SerializeObject(stored, Formatting.Indented));

                // Envoi final
                await _projetService.AjouterAsync(stored.Projets);
                await _cadreService.AjouterAsync(stored.CadreLogique);
                foreach (var dto in stored.AspectsJuridiques)
                    await _aspectsJuridiquesService.AjouterAsync(dto);
                await _LocalisationGeographiqueService.AjouterAsync(stored.LocalisationGeographique);

                TempData.Remove("WizardModel");
                TempData["SuccessMessage"] = "Projet ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }

            // Fallback: réaffiche la même étape
            return RedirectToAction(nameof(Wizard), new { step });
        }

        // -------------------------------------------------------
        // En-dehors de l’action : helper de fusion
        // -------------------------------------------------------

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

                    // TODO : cases 3 à 6...
            }
        }

        // -------------------------------------------------------
        // En-dehors de l’action : helper de propagation d’ID
        // -------------------------------------------------------

        private void PropagateAllLists(DdpViewModel model)
        {
            var id = model.Projets.IdIdentificationProjet;
            PropagateIdToList(model.Projets.Activites, id);
            PropagateIdToList(model.Projets.AspectsJuridiques, id);
            PropagateIdToList(model.Projets.PartiesPrenantes, id);
            PropagateIdToList(model.Projets.IndicateursDeResultats, id);
            PropagateIdToList(model.Projets.LivrablesProjets, id);
            PropagateIdToList(model.Projets.EffetsProjets, id);
            PropagateIdToList(model.Projets.ObjectifsSpecifiques, id);
            PropagateIdToList(model.Projets.ImpactsDesProjets, id);
            PropagateIdToList(model.Projets.BailleursDeFonds, id);
            PropagateIdToList(model.Projets.ActivitesAnnuelles, id);
            PropagateIdToList(model.Projets.CoutAnnuelDuProjet, id);
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


        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string letter)
        {
            // Conserver les filtres pour la vue
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLetter"] = letter;

            // Récupérer tous les projets (DTO)
            var projets = await _projetService.ObtenirTousAsync();

            // Filtrer par searchString si non vide
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projets = projets
                    .Where(p => p.NomProjet != null
                             && p.NomProjet.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filtrer par lettre si non vide
            if (!string.IsNullOrWhiteSpace(letter))
            {
                projets = projets
                    .Where(p => !string.IsNullOrEmpty(p.NomProjet)
                             && p.NomProjet.StartsWith(letter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(projets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();

            return View(projet);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var projet = await _projetService.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();

            return View(projet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjetsBPDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _projetService.MettreAJourAsync(model);

            TempData["SuccessMessage"] = "Projet mis à jour avec succès !";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
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

        [HttpPost]
        public async Task<IActionResult> SaveSection(string sectionName, [FromBody] JObject data)
        {
            // Sauvegarde partielle selon la section
            return Ok(new { status = "saved", section = sectionName });
        }

        [HttpGet]
        public async Task<IActionResult> IndexMPCE()
        {
            var tousLesProjets = await _projetService.ObtenirTousAsync();

            var projetsMpce = tousLesProjets
                .Where(p =>
                    string.Equals(p.Ministere, "MPCE", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View("IndexMPCE", projetsMpce);
        }

        // -------------------------
        // Méthodes utilitaires privées
        // -------------------------

        /// <summary>
        /// Garantit que le modèle possède un IdIdentificationProjet et le propage aux DTOs enfants.
        /// </summary>
        private void EnsureProjectId(DdpViewModel model)
        {
            if (model == null) return;

            // Créer les sous-objets si nécessaire
            if (model.Projets == null) model.Projets = new ProjetsBPDto();
            if (model.CadreLogique == null) model.CadreLogique = new DdpCadreLogiqueDto();
            if (model.AspectsJuridiques == null) model.AspectsJuridiques = new List<AspectsJuridiquesDto>();
            if (model.LocalisationGeographique == null) model.LocalisationGeographique = new LocalisationGeographiqueProjDto();

            // Générer l'ID si absent
            if (string.IsNullOrWhiteSpace(model.Projets.IdIdentificationProjet))
            {
                var newId = IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
                model.Projets.IdIdentificationProjet = newId;

                // Propager l'ID aux autres DTOs simples
                model.CadreLogique.IdIdentificationProjet = newId;
                model.AspectsJuridiques.Add(
                    new AspectsJuridiquesDto {IdIdentificationProjet = model.Projets.IdIdentificationProjet });
                model.LocalisationGeographique.IdIdentificationProjet = newId;
            }
            else
            {
                // si ID existant, s'assurer que les DTOs simples ont aussi la valeur
                model.CadreLogique.IdIdentificationProjet = model.CadreLogique.IdIdentificationProjet ?? model.Projets.IdIdentificationProjet;
                model.AspectsJuridiques.Add(
                    new AspectsJuridiquesDto { IdIdentificationProjet = model.Projets.IdIdentificationProjet });

                model.LocalisationGeographique.IdIdentificationProjet = model.LocalisationGeographique.IdIdentificationProjet ?? model.Projets.IdIdentificationProjet;
            }

            // Propager aux listes contenues dans Projets (si non null)
            PropagateIdToList(model.Projets?.Activites, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.AspectsJuridiques, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.PartiesPrenantes, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.IndicateursDeResultats, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.LivrablesProjets, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.EffetsProjets, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.ObjectifsSpecifiques, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.ImpactsDesProjets, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.BailleursDeFonds, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.ActivitesAnnuelles, model.Projets.IdIdentificationProjet);
            PropagateIdToList(model.Projets?.CoutAnnuelDuProjet, model.Projets.IdIdentificationProjet);
        }

        /// <summary>
        /// Propage l'IdIdentificationProjet sur chaque item d'une collection si la propriété existe.
        /// </summary>

    }
}
