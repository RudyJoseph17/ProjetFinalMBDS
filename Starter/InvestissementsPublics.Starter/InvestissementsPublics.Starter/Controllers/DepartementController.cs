using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;

namespace InvestissementsPublics.Starter.Controllers
{
    public class DepartementController : Controller
    {
        private readonly IDepartementService _service;

        public DepartementController(IDepartementService service)
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
            var vm = new DepartementDto();
            vm.ListArrondissements.Add(new ArrondissementDto
            {
                ListCommunes = { new CommuneDto { ListSections = { new SectionCommunaleDto() } } }
            });
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartementDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
