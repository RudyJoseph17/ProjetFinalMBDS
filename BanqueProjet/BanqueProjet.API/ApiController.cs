using Microsoft.AspNetCore.Mvc;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;

namespace BanqueProjet.API.Controllers
{
    [ApiController]
    [Route("api/banqueprojet/identification")]
    public class IdentificationProjetApiController : ControllerBase
    {
        private readonly IIdentificationProjetService _service;

        public IdentificationProjetApiController(IIdentificationProjetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<IdentificationProjetDto>>> GetAll()
        {
            var projets = await _service.ObtenirTousAsync();
            return Ok(projets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentificationProjetDto>> GetById(string id)
        {
            var projet = await _service.ObtenirParIdAsync(id);
            if (projet == null) return NotFound();
            return Ok(projet);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] IdentificationProjetDto dto)
        {
            await _service.AjouterAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.IdIdentificationProjet }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] IdentificationProjetDto dto)
        {
            if (id != dto.IdIdentificationProjet)
                return BadRequest("L'identifiant ne correspond pas.");
            await _service.MettreAJourAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _service.SupprimerAsync(id);
            return NoContent();
        }
    }
}

