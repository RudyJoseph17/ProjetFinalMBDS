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
        public async Task<IActionResult> Index()
        {
            // Récupération asynchrone de tous les projets
            var identList = await _projetService.ObtenirTousAsync()
                             ?? new List<IdentificationProjetDto>();

            // 2) mapper en ProjetsBPDto (la vue attend IEnumerable<ProjetsBPDto>)
            var projets = _mapper.Map<List<ProjetsBPDto>>(identList) ?? new List<ProjetsBPDto>();

            // Statistiques pour les cards
            var total = projets.Count;
            var validesCount = 0;
            var nomsProjetsValides = new List<string>();
            // Exemple futur si le DTO a une propriété Statut :
            //validesCount = projets.Count(p => p.Statut == "Valide");
            //nomsProjetsValides = projets
            //    .Where(p => p.Statut == "Valide")
            //    .Select(p => p.NomProjet)
            //    .ToList();

            // Préparation des données pour Chart.js : nombre de projets par ministère
            var projetsParMinistere = projets
                .GroupBy(p => string.IsNullOrWhiteSpace(p.Ministere)
                              ? "Non spécifié"
                              : p.Ministere)
                .Select(g => new { Ministere = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            ViewData["Ministeres"] = JsonSerializer.Serialize(
                projetsParMinistere.Select(x => x.Ministere));
            ViewData["Counts"] = JsonSerializer.Serialize(
                projetsParMinistere.Select(x => x.Count));

            // Construction du ViewModel
            var vm = new DashboardStatsViewModel
            {
                TotalProjets = total,
                ProjetsValides = validesCount,
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
