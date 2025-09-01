using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;

namespace InvestissementsPublics.Starter.Controllers
{
    public class SecteurActiviteController : Controller
    {
        private readonly ISecteurActiviteService _service;

        public SecteurActiviteController(ISecteurActiviteService service)
        {
            _service = service;
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

        public IActionResult Create()
        {
            var vm = new SecteurActiviteDto();
            vm.ListSousSecteurActivite.Add(new SousSecteurActiviteDto());
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SecteurActiviteDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
