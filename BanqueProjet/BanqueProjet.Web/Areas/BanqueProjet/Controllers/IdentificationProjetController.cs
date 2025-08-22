using System;
using System.Collections.Generic;
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
using Shared.Domain.ApplicationUsers;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using BanqueProjet.Infrastructure.Persistence;
////using InvestissementsPublics.Starter.ApplicationUsers;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class IdentificationProjetController : Controller
    {
        private readonly IIdentificationProjetService _projetService;
        private readonly IDdpCadreLogiqueService _cadreService;
        private readonly IAspectsJuridiquesService _aspectsJuridiquesService;
        private readonly IGestionDeProjetService _gestionProjetService;
        private readonly ILocalisationGeographiqueProjService _LocalisationGeographiqueService;
        private readonly ISuiviEtControleService _SuiviControleService;
        //private readonly INotificationService _notif;
        ////private readonly UserManager<ApplicationUser> _userMgr;
        private readonly ILogger<IdentificationProjetController> _logger;
        private const int TotalSteps = 13;

        public IdentificationProjetController(
            IIdentificationProjetService projetService,
            IDdpCadreLogiqueService cadreService,
            IAspectsJuridiquesService aspectsJuridiquesService,
            IGestionDeProjetService gestionProjetService,
            ILocalisationGeographiqueProjService LocalisationGeographiqueService,
            //ISuiviEtControleService SuiviControleService,
            //INotificationService notif,
            ////UserManager<ApplicationUser> userMgr,
            ILogger<IdentificationProjetController> logger)
        {
            _projetService = projetService;
            _cadreService = cadreService;
            _aspectsJuridiquesService = aspectsJuridiquesService;
            _gestionProjetService = gestionProjetService;
            _LocalisationGeographiqueService = LocalisationGeographiqueService;
            //_SuiviControleService = SuiviControleService;
            ////_userMgr = userMgr;
            //_notif = notif;
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
            }
            else
            {
                var newId = IdGenerator.GenererIdPour(nameof(IdentificationProjetDto.IdIdentificationProjet));
                model = new DdpViewModel
                {
                    Projets = new IdentificationProjetDto { IdIdentificationProjet = newId },
                    CadreLogique = new DdpCadreLogiqueDto { IdIdentificationProjet = newId },
                    AspectsJuridiques = new AspectsJuridiquesDto { IdIdentificationProjet = newId },
                    DureeProjet = new DureeProjetDto { IdIdentificationProjet = newId },
                    Echelon = new EchelonTerritorialeDto { IdIdentificationProjet = newId },
                    EmploisCrees = new EmploisCreesDto { IdIdentificationProjet = newId },
                    GestionProjet = new GestionDeProjetDto { IdIdentificationProjet = newId },
                    LocalisationGeographique = new LocalisationGeographiqueProjDto { IdIdentificationProjet = newId },
                    //SuiviControle = new SuiviEtControleDto { IdIdentificationProjet = newId },


                };
            }

            ViewData["Step"] = step;
            return View($"WizardStep{step}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wizard(DdpViewModel model, int step, string action)
        {
            // Serialize and store model in TempData
            var serialized = JsonConvert.SerializeObject(model);
            TempData["WizardModel"] = serialized;

            // Navigate steps
            if (action == "Prev" && step > 1)
            {
                step--;
                return RedirectToAction(nameof(Wizard), new { step });
            }
            else if (action == "Next" && step < TotalSteps)
            {
                step++;
                return RedirectToAction(nameof(Wizard), new { step });
            }
            else if (action == "Finish")
            {
                // Propagate IDs to nested lists
                void PropagateId<T>(List<T> list) where T : class
                {
                    foreach (var item in list)
                    {
                        var prop = item.GetType().GetProperty(nameof(IdentificationProjetDto.IdIdentificationProjet));
                        if (prop != null)
                            prop.SetValue(item, model.Projets.IdIdentificationProjet);
                    }
                }

                PropagateId(model.Projets.Activites);
                PropagateId(model.Projets.AspectsJuridiques);
                PropagateId(model.Projets.PartiesPrenantes);
                PropagateId(model.Projets.IndicateursDeResultats);
                PropagateId(model.Projets.LivrablesProjets);
                PropagateId(model.Projets.EffetsProjets);
                PropagateId(model.Projets.ObjectifsSpecifiques);
                PropagateId(model.Projets.ImpactsDesProjets);
                PropagateId(model.Projets.BailleursDeFonds);
                PropagateId(model.Projets.ActivitesAnuelles);
                PropagateId(model.Projets.CoutAnnuelsProjets);
              

                // 1. Création du projet
                await _projetService.AjouterAsync(model.Projets);

                // 2. Création du cadre logique
                await _cadreService.AjouterAsync(model.CadreLogique);

                await _aspectsJuridiquesService.AjouterAsync(model.AspectsJuridiques);

                await _LocalisationGeographiqueService.AjouterAsync(model.LocalisationGeographique);

                //await _SuiviControleService.AjouterAsync(model.SuiviControle);

                TempData.Remove("WizardModel");
                TempData["SuccessMessage"] = "Projet ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ActiviteList = new SelectList(
        model.Projets.Activites,
        nameof(ActiviteDto.IdActivites),
        nameof(ActiviteDto.NomActivite),
        model.Prevision.IdActivites
    );

            // Fallback: redisplay current step
            return RedirectToAction(nameof(Wizard), new { step });
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

            ////var directs = await _userMgr.GetUsersInRoleAsync("DirecteurUEP");
            ////foreach (var d in directs)
            ////{
            ////    foreach (var projet in projets)
            ////    {
            ////        var msg = $"Nouveau projet « {projet.NomProjet} » soumis par un analyste UEP.";
            ////        await _notif.NotifyAsync(d.Id, "Nouveau projet UEP", msg);
            ////        await _notif.NotifyByEmailAsync(d.Id, "Nouveau projet UEP", msg);
            ////    }
            ////}




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
        public async Task<IActionResult> Edit(IdentificationProjetDto model)
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
            // 1. Récupère tous les projets DTO
            var tousLesProjets = await _projetService.ObtenirTousAsync();
            //    (ou GetAllDtosAsync selon votre service)

            // 2. Filtre ceux dont le Ministère est "MPCE"
            var projetsMpce = tousLesProjets
                .Where(p =>
                    string.Equals(p.Ministere, "MPCE", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // 3. Retourne la vue IndexMPCE.cshtml
            return View("IndexMPCE", projetsMpce);
        }
    }
}

