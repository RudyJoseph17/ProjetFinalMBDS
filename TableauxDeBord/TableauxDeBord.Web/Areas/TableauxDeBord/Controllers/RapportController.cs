using Microsoft.AspNetCore.Mvc;

namespace TableauxDeBord.Web.Areas.TableauxDeBord.Controllers
{


    [Area("TableauxDeBord")]
    public class RapportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
