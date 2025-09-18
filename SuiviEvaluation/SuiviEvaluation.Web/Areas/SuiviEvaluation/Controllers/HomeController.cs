using System.Diagnostics;
using BanqueProjet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SuiviEvaluation.Web.Models;

namespace SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers;

[Area("SuiviEvaluation")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // injecter un service qui interroge le module BanqueProjet, par ex. IBanqueProjetService
    public async Task<IActionResult> Index([FromServices] IProjetsBPService banqueService)
    {
        var projetsDto = await banqueService.GetProjetsSommaireAsync();

        // mappez vers le ViewModel attendu par la vue
        var vm = projetsDto.Select(p => new ProjetListItemViewModel
        {
            Id = p.IdIdentificationProjet,
            NomProjet = p.NomProjet,
            CoutTotal = p.CoutTotalProjet /* <-- Remplacez par la propriété correcte pour le coût total */
                is decimal ? (decimal)p.CoutTotalProjet : 0m
        }).ToList();

        return View(vm);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
