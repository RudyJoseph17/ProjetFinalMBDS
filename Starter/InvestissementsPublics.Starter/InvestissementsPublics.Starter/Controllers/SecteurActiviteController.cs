using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace InvestissementsPublics.Starter.Controllers
{
    public class SecteurActiviteController : Controller
    {
        private readonly ISecteurActiviteService _service;
        private readonly ILogger<SecteurActiviteController> _logger;

        public SecteurActiviteController(
            ISecteurActiviteService service,
            ILogger<SecteurActiviteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _service.ObtenirTousAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        // GET: Create
        public IActionResult Create()
        {
            var vm = new SecteurActiviteDto();
            // au moins un champ vide pour index 0
            vm.ListSousSecteurActivite.Add(new SousSecteurActiviteDto());
            return View(vm);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SecteurActiviteDto dto)
        {
            // debug
            _logger.LogInformation("Sous-secteurs reçus : {Count}",
                dto.ListSousSecteurActivite?.Count ?? 0);

            if (!ModelState.IsValid)
                return View(dto);

            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }


        // … reste des actions (Index, Details, Edit, Delete) inchangées
    }
}
