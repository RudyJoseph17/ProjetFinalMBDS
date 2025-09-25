using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BanqueProjet.Web.Models;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using Shared.Domain.Interface;
using Shared.Domain.Dtos;
using AutoMapper;

namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
{
    [Area("BanqueProjet")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIdentificationProjetService _projetService;
        private readonly IMapper _mapper;

        public HomeController(
            ILogger<HomeController> logger,
            IIdentificationProjetService projetService,
            IMapper mapper)
        {
            _logger = logger;
            _projetService = projetService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Page d'accueil : affiche le dashboard (cartes + graphique)
        /// </summary>
       [HttpGet]
public async Task<IActionResult> Index()
{
    // 1) Charger et mapper tous les projets
    var identList = await _projetService.ObtenirTousAsync()
                     ?? new List<IdentificationProjetDto>();
    var projets = _mapper.Map<List<ProjetsBPDto>>(identList)
                  ?? new List<ProjetsBPDto>();

    // 2) Calcul des stats “cartes”
    var totalProjets   = projets.Count;
    var projetsValides = projets.Count(p => p.AvisProjet == "Valide");

    // 3) Calcul des stats “graphique”
    var projetsParMinistere = projets
        .GroupBy(p => string.IsNullOrWhiteSpace(p.Ministere) ? "Non spécifié" : p.Ministere)
        .Select(g => new { Ministere = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .ToList();

    var ministeres = projetsParMinistere.Select(x => x.Ministere).ToList();
    var counts     = projetsParMinistere.Select(x => x.Count).ToList();

    // 4) Fournir les JSON à Chart.js via ViewData
    ViewData["Ministeres"] = JsonSerializer.Serialize(ministeres);
    ViewData["Counts"]     = JsonSerializer.Serialize(counts);

    // 5) Remplir le DashboardStatsViewModel
    var statsVm = new DashboardStatsViewModel
    {
        TotalProjets   = totalProjets,
        ProjetsValides = projetsValides,
        Ministeres     = ministeres,
        Counts         = counts
    };

    // 6) Construire et renvoyer l’IndexViewModel
    var vm = new IndexViewModel
    {
        Projets = projets,
        Stats   = statsVm
    };

    return View(vm);
}



        public IActionResult Privacy()
            => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
