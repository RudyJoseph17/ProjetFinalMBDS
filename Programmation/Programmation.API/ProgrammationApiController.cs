using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programmation.Application.Dtos;
using Programmation.Application.Interface;

namespace Programmation.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgrammationApiController : ControllerBase
    {
        private readonly IProgrammationProjetService _programmationService;
        private readonly ILivrablesProjetService _livrablesService;
        private readonly IInformationsFinancieresProgrammeesProjetService _financieresService;

        public ProgrammationApiController(
            IProgrammationProjetService programmationService,
            ILivrablesProjetService livrablesService,
            IInformationsFinancieresProgrammeesProjetService financieresService)
        {
            _programmationService = programmationService;
            _livrablesService = livrablesService;
            _financieresService = financieresService;
        }

        #region ProgrammationProjet Endpoints

        [HttpGet("projets")]
        public async Task<ActionResult<List<ProgrammationProjetDto>>> GetAllProjets()
        {
            var projets = await _programmationService.ObtenirTousAsync();
            return Ok(projets);
        }

        [HttpGet("projets/{id}")]
        public async Task<ActionResult<ProgrammationProjetDto>> GetProjetById(string id)
        {
            var projet = await _programmationService.ObtenirParIdAsync(id);
            if (projet is null)
                return NotFound();

            return Ok(projet);
        }

        [HttpPost("projets")]
        public async Task<ActionResult> CreateProjet([FromBody] ProgrammationProjetDto dto)
        {
            await _programmationService.AjouterAsync(dto);
            return CreatedAtAction(
                nameof(GetProjetById),
                new { id = dto.IdIdentificationProjet },
                dto
            );
        }

        [HttpPut("projets/{id}")]
        public async Task<ActionResult> UpdateProjet(string id, [FromBody] ProgrammationProjetDto dto)
        {
            if (id != dto.IdIdentificationProjet)
                return BadRequest("L'identifiant ne correspond pas.");

            await _programmationService.MettreAJourAsync(dto);
            return NoContent();
        }

        [HttpDelete("projets/{id}")]
        public async Task<ActionResult> DeleteProjet(string id)
        {
            await _programmationService.SupprimerAsync(id);
            return NoContent();
        }

        #endregion

        #region LivrablesProjet Endpoints

        [HttpGet("livrables")]
        public async Task<ActionResult<List<LivrablesProgrameProjetDto>>> GetAllLivrables()
        {
            var livrables = await _livrablesService.ObtenirTousAsync();
            return Ok(livrables);
        }

        [HttpGet("livrables/{id}")]
        public async Task<ActionResult<LivrablesProgrameProjetDto>> GetLivrableById(byte id)
        {
            var livrable = await _livrablesService.ObtenirParIdAsync(id);
            if (livrable is null)
                return NotFound();

            return Ok(livrable);
        }

        [HttpPost("livrables")]
        public async Task<ActionResult> CreateLivrable([FromBody] LivrablesProgrameProjetDto dto)
        {
            await _livrablesService.AjouterAsync(dto);
            return CreatedAtAction(
                nameof(GetLivrableById),
                new { id = dto.IdLivrablesProjet },
                dto
            );
        }

        [HttpPut("livrables/{id}")]
        public async Task<ActionResult> UpdateLivrable(byte id, [FromBody] LivrablesProgrameProjetDto dto)
        {
            if (id != dto.IdLivrablesProjet)
                return BadRequest("L'identifiant ne correspond pas.");

            await _livrablesService.MettreAJourAsync(dto);
            return NoContent();
        }

        [HttpDelete("livrables/{id}")]
        public async Task<ActionResult> DeleteLivrable(byte id)
        {
            await _livrablesService.SupprimerAsync(id);
            return NoContent();
        }

        #endregion

        #region InformationsFinancieresProjet Endpoints

        [HttpGet("informations-financieres")]
        public async Task<ActionResult<List<InformationsFinancieresProgrammeesProjetDto>>> GetAllInformationsFinancieres()
        {
            var infos = await _financieresService.ObtenirTousAsync();
            return Ok(infos);
        }

        [HttpGet("informations-financieres/{id}")]
        public async Task<ActionResult<InformationsFinancieresProgrammeesProjetDto>> GetInformationsFinancieresById(byte id)
        {
            var info = await _financieresService.ObtenirParIdAsync(id);
            if (info is null)
                return NotFound();

            return Ok(info);
        }

        [HttpPost("informations-financieres")]
        public async Task<ActionResult> CreateInformationsFinancieres([FromBody] InformationsFinancieresProgrammeesProjetDto dto)
        {
            await _financieresService.AjouterAsync(dto);
            return CreatedAtAction(
                nameof(GetInformationsFinancieresById),
                new { id = dto.IdInformationsFinancieres},
                dto
            );
        }

        [HttpPut("informations-financieres/{id}")]
        public async Task<ActionResult> UpdateInformationsFinancieres(byte id, [FromBody] InformationsFinancieresProgrammeesProjetDto dto)
        {
            if (id != dto.IdInformationsFinancieres)
                return BadRequest("L'identifiant ne correspond pas.");

            await _financieresService.MettreAJourAsync(dto);
            return NoContent();
        }

        [HttpDelete("informations-financieres/{id}")]
        public async Task<ActionResult> DeleteInformationsFinancieres(byte id)
        {
            await _financieresService.SupprimerAsync(id);
            return NoContent();
        }

        #endregion
    }
}
