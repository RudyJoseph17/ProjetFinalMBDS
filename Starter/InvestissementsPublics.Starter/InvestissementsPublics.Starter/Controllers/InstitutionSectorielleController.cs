using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;

namespace InvestissementsPublics.Starter.Controllers
{
    public class InstitutionSectorielleController : Controller
    {
        private readonly IInstitutionSectorielleService _service;

        public InstitutionSectorielleController(IInstitutionSectorielleService service)
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
            // On initialise au moins un élément vide
            var vm = new InstitutionSectorielleDto();
            vm.ListAttributions.Add(new AttributionsInstitutionDto());
            vm.ListSections.Add(new SectionInstitutionDto());
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstitutionSectorielleDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
