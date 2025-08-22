using Microsoft.AspNetCore.Mvc;
using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using SuiviEvaluation.Web.Models;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers
{
    [Area("SuiviEvaluation")]
    public class BilanController : Controller
    {
        private readonly IEvolutionTemporelleService _evolutionService;
        private readonly IFluxFinancierService _fluxService;
        private readonly IQuantiteLivreParAnneeService _quantiteService;

        public BilanController(
            IEvolutionTemporelleService evolutionService,
            IFluxFinancierService fluxService,
            IQuantiteLivreParAnneeService quantiteService)
        {
            _evolutionService = evolutionService;
            _fluxService = fluxService;
            _quantiteService = quantiteService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new Bilan
            {
                Evolution = (await _evolutionService.GetEvolutionCouranteAsync()).ToList(),
                FluxFinanciers = (await _fluxService.GetFluxFinancierAsync()).ToList(),
                QuantitesLivrees = (await _quantiteService.GetAllAsync()).ToList()
            };

            return View(model);
        }

        // Cette méthode suppose que tu saisis uniquement les quantités livrées dans Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Bilan
            {
                Evolution = new List<EvolutionTemporelleDuProjetDto> { new EvolutionTemporelleDuProjetDto() },
                FluxFinanciers = new List<FluxFinancierDto> { new FluxFinancierDto() },
                QuantitesLivrees = new List<QuantiteLivreParAnneeDto> { new QuantiteLivreParAnneeDto() }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bilan model)
        {
            if (ModelState.IsValid)
            {
                // Supposons qu'il n'y ait qu'un seul élément par section à saisir à la fois.
                if (model.Evolution.FirstOrDefault() is EvolutionTemporelleDuProjetDto evolution)
                    await _evolutionService.AddAsync(evolution);

                if (model.FluxFinanciers.FirstOrDefault() is FluxFinancierDto flux)
                    await _fluxService.AddAsync(flux);

                if (model.QuantitesLivrees.FirstOrDefault() is QuantiteLivreParAnneeDto quantite)
                    await _quantiteService.AddAsync(quantite);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}
