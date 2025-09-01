using InvestissementsPublics.Starter.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using System.Threading.Tasks;

namespace InvestissementsPublics.Starter.Controllers
{
    public class BailleurDeFondsController : Controller
    {
        private readonly IBailleurDeFondsService _service;

        public BailleurDeFondsController(IBailleurDeFondsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var liste = await _service.ObtenirTousAsync();
            //return Json(liste);
            return View(liste);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BailleurDeFondsDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.AjouterAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BailleurDeFondsDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.MettreAJourAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.SupprimerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.ObtenirParIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }
    }
}
