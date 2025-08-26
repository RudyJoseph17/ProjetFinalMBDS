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
using Shared.Infrastructure.Persistence;
using Shared.Domain.ApplicationUsers;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using BanqueProjet.Infrastructure.Persistence;
using Shared.Domain.Interface;
using Shared.Domain.Dtos;
using AutoMapper;
////using InvestissementsPublics.Starter.ApplicationUsers;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class IdentificationProjetController : Controller
    {
        private readonly IIdentificationProjetService _projetService;
        private readonly IActiviteBPService _activiteBPService;
        private readonly IActivitesAnnuellesService _activitesAnnuellesService;
        private readonly IBailleursDeFondService _bailleursDeFondsService;
        private readonly IDdpCadreLogiqueService _cadreLogiqueService;
        private readonly IAspectsJuridiquesService _aspectsJuridiquesService;
        private readonly IPartiesPrenantesService _partiesPrenantesService;
        private readonly ILocalisationGeographiqueProjService _localisationGeographiqueService;
        private readonly ICoutAnnuelDuProjetService _coutannuelProjetService;
        private readonly IEffetsDuProjetService _effetsProjetService;
        private readonly IImpactsDuProjetService _impactsProjetsService;
        private readonly IIndicateursDeResultatService _indicateursResultatsService;
        private readonly IInformationsFinancieresService _informationsFinancieresService;
        private readonly IDefinitionLivrablesDuProjetService _livrablesProjetService;
        private readonly IObjectifsSpecifiquesService _objectifsSpecifiquesService;
        private readonly ILogger<IdentificationProjetController> _logger;
        private readonly IMapper _mapper;
        private const int TotalSteps = 6;

        public IdentificationProjetController(
            IIdentificationProjetService ProjetService,
            IActiviteBPService ActiviteBPService,
            IActivitesAnnuellesService ActivitesAnnuellesService,
            IBailleursDeFondService BailleursDeFondsService,
            IDdpCadreLogiqueService CadreLogiqueService,
            IAspectsJuridiquesService AspectsJuridiquesService,
            IPartiesPrenantesService PartiesPrenantesService,
            ILocalisationGeographiqueProjService LocalisationGeographiqueService,
            ICoutAnnuelDuProjetService CoutannuelProjetService,
            IEffetsDuProjetService EffetsProjetService,
            IImpactsDuProjetService ImpactsProjetsService,
            IIndicateursDeResultatService IndicateursResultatsService,
            IInformationsFinancieresService InformationsFInancieresService,
            IDefinitionLivrablesDuProjetService LivrablesProjetService,
            IObjectifsSpecifiquesService ObjectifsSpecifiquesService,
            IMapper mapper,
            ILogger<IdentificationProjetController> logger)
        {
            _projetService = ProjetService;
            _activiteBPService = ActiviteBPService;
            _activitesAnnuellesService = ActivitesAnnuellesService;
            _bailleursDeFondsService = BailleursDeFondsService;
            _cadreLogiqueService = CadreLogiqueService;
            _aspectsJuridiquesService = AspectsJuridiquesService;
            _partiesPrenantesService = PartiesPrenantesService;
            _localisationGeographiqueService = LocalisationGeographiqueService;
            _coutannuelProjetService = CoutannuelProjetService;
            _effetsProjetService = EffetsProjetService;
            _impactsProjetsService = ImpactsProjetsService;
            _indicateursResultatsService = IndicateursResultatsService;
            _informationsFinancieresService = InformationsFInancieresService;
            _livrablesProjetService = LivrablesProjetService;
            _objectifsSpecifiquesService = ObjectifsSpecifiquesService;
            _mapper = mapper;
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
                    Projets = new ProjetsBPDto { IdIdentificationProjet = newId },
                    ActiviteBP = new ActiviteBPDto { IdIdentificationProjet = newId },
                    ActiviteAnuelle = new ActivitesAnnuellesDto { IdIdentificationProjet = newId },
                    BailleursDeFonds = new BailleursDeFondsDto { IdIdentificationProjet = newId },
                    CadreLogique = new DdpCadreLogiqueDto { IdIdentificationProjet = newId },
                    AspectsJuridiques = new AspectsJuridiquesDto { IdIdentificationProjet = newId } ,
                    PartiesPrenantesProjets = new PartiesPrenantesDto { IdIdentificationProjet = newId },
                    LocalisationGeographique = new LocalisationGeographiqueProjDto { IdIdentificationProjet = newId },
                    CoutAnnuelProjet = new CoutAnnuelDuProjetDto { IdIdentificationProjet = newId },
                    EffetsProjets = new EffetsDuProjetDto { IdIdentificationProjet = newId },
                    ImpactsDuProjets = new ImpactsDuProjetDto { IdIdentificationProjet = newId },
                    IndicateursResultats = new IndicateursDeResultatDto { IdIdentificationProjet = newId },
                    DefinitionLivrables = new DefinitionLivrablesDuProjetDto { IdIdentificationProjet = newId },
                    ObjectifsSpecifiques = new ObjectifsSpecifiquesDto { IdIdentificationProjet = newId }
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
                await _activiteBPService.AjouterAsync(model.ActiviteBP);
                await _activitesAnnuellesService.AjouterAsync(model.ActiviteAnuelle);
                await _bailleursDeFondsService.AjouterAsync(model.BailleursDeFonds);
                await _cadreLogiqueService.AjouterAsync(model.CadreLogique);
                await _aspectsJuridiquesService.AjouterAsync(model.AspectsJuridiques);
                await _partiesPrenantesService.AjouterAsync(model.PartiesPrenantesProjets);
                await _localisationGeographiqueService.AjouterAsync(model.LocalisationGeographique);
                await _coutannuelProjetService.AjouterAsync(model.CoutAnnuelProjet);
                await _effetsProjetService.AjouterAsync(model.EffetsProjets);
                await _impactsProjetsService.AjouterAsync(model.ImpactsDuProjets);
                await _indicateursResultatsService.AjouterAsync(model.IndicateursResultats);
                await _informationsFinancieresService.AjouterAsync(model.InformationsFinancieresBP);
                await _livrablesProjetService.AjouterAsync(model.DefinitionLivrables);
                await _objectifsSpecifiquesService.AjouterAsync(model.ObjectifsSpecifiques);

                //await _SuiviControleService.AjouterAsync(model.SuiviControle);

                TempData.Remove("WizardModel");
                TempData["SuccessMessage"] = "Projet ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ActiviteList = new SelectList(
        model.Projets.Activites,
        nameof(ActiviteBPDto.IdActivites),
        nameof(ActiviteBPDto.NomActivite),
        model.InformationsFinancieresBP.IdActivites
    );

            // Fallback: redisplay current step
            return RedirectToAction(nameof(Wizard), new { step });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string letter)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLetter"] = letter;

            var identList = await _projetService.ObtenirTousAsync()
                             ?? new List<IdentificationProjetDto>();

            _logger.LogInformation("identList count = {count}", identList?.Count ?? -1);
            _logger.LogInformation("_mapper is null? {isNull}", _mapper == null);

            // mapper vers ProjetsBPDto
            var projets = _mapper.Map<List<ProjetsBPDto>>(identList);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projets = projets
                    .Where(p => p.NomProjet != null &&
                                p.NomProjet.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(letter))
            {
                projets = projets
                    .Where(p => !string.IsNullOrEmpty(p.NomProjet) &&
                                p.NomProjet.StartsWith(letter, StringComparison.OrdinalIgnoreCase))
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

        //[HttpGet]
        //public async Task<IActionResult> IndexMPCE()
        //{
        //    // 1. Récupère tous les projets DTO
        //    var tousLesProjets = await _projetService.ObtenirTousAsync();
        //    //    (ou GetAllDtosAsync selon votre service)

        //    // 2. Filtre ceux dont le Ministère est "MPCE"
        //    var projetsMpce = tousLesProjets
        //        .Where(p =>
        //            string.Equals(p.Ministere, "MPCE", StringComparison.OrdinalIgnoreCase))
        //        .ToList();

        //    // 3. Retourne la vue IndexMPCE.cshtml
        //    return View("IndexMPCE", projetsMpce);
        //}
    }
}

