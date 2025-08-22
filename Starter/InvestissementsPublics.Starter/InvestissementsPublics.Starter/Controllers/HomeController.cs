using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvestissementsPublics.Starter.Models;

namespace InvestissementsPublics.Starter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: / ou /Home/Index
        [HttpGet("")]
        [HttpGet("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/GoTo?module=BanqueProjet
        [HttpGet("Home/GoTo")]
        public IActionResult GoTo(string module)
        {
            // Dictionnaire des URLs de chaque module
            var moduleUrls = new Dictionary<string, string>
            {
                ["BanqueProjet"] = "https://localhost:7234",
                ["Programmation"] = "https://localhost:5030",
                ["SuiviEvaluation"] = "https://localhost:5035",
                ["TableauxDeBord"] = "https://localhost:5040"
            };

            if (string.IsNullOrEmpty(module) || !moduleUrls.ContainsKey(module))
            {
                return NotFound($"Module inconnu : {module}");
            }

            return Redirect(moduleUrls[module]);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
