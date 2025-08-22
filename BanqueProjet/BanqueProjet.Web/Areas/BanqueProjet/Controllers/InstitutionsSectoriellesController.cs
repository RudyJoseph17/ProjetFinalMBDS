//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;                 // <-- pour ToListAsync() et ExecuteSqlRawAsync()
//using Oracle.ManagedDataAccess.Client;
//using BanqueProjet.Infrastructure.Data;
////using BanqueProjet.Infrastructure.Entities;
//using BanqueProjet.Web.Models;

//namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
//{
//    [Area("BanqueProjet")]
//    public class InstitutionSectorielleController : Controller
//    {
//        private readonly BanquePDbContext _context;

//        public InstitutionSectorielleController(BanquePDbContext context)
//        {
//            _context = context;
//        }

//        // GET: BanqueProjet/InstitutionSectorielle
//        public async Task<IActionResult> Index()
//        {
//            // 1) Charge toutes les institutions
//            var institutions = await _context.OViewInstitutionSectorielle
//                                             .ToListAsync();

//            // 2) Charge toutes les sections
//            var sections = await _context.OViewSectionInstitution
//                                         .ToListAsync();

//            // 3) Jointure en mémoire (string vs number)
//            var data = institutions
//                .Select(i => new InstitutionSectorielleViewModel
//                {
//                    IdInstitutionSectorielle = i.IdInstitutionSectorielle,
//                    NomInstitutionSectorielle = i.NomInstitutionSectorielle,
//                    MissionInstitutionSectorielle = i.MissionInstitutionSectorielle,
//                    AttributionsInstitutionSectorielle = i.AttributionsInstitutionSectorielle,
//                    NomsSections = sections
//                        .Where(s => s.IdInstitutionSectorielle.ToString() == i.IdInstitutionSectorielle)
//                        .Select(s => s.NomSection)
//                        .ToList()
//                })
//                .ToList();

//            return View(data);
//        }

//        // GET: BanqueProjet/InstitutionSectorielle/Create
//        public IActionResult Create()
//            => View();

//        // POST: BanqueProjet/InstitutionSectorielle/Create
//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(
//            [Bind("IdInstitutionSectorielle,NomInstitutionSectorielle,MissionInstitutionSectorielle,AttributionsInstitutionSectorielle")]
//            InstitutionSectorielleViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            const string sql = @"
//                BEGIN
//                  AJOUTER_INSTITUTION_SECTORIELLE(
//                    :p_ID, :p_NOM, :p_MISSION, :p_ATTRIBUTIONS
//                  );
//                END;";

//            var parms = new[]
//            {
//                new OracleParameter("p_ID",          model.IdInstitutionSectorielle),
//                new OracleParameter("p_NOM",         model.NomInstitutionSectorielle),
//                new OracleParameter("p_MISSION",     model.MissionInstitutionSectorielle),
//                new OracleParameter("p_ATTRIBUTIONS",model.AttributionsInstitutionSectorielle)
//            };

//            await _context.Database.ExecuteSqlRawAsync(sql, parms);
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: BanqueProjet/InstitutionSectorielle/Delete/5
//        public async Task<IActionResult> Delete(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//                return BadRequest();

//            // Même pattern que pour Index : on charge tout et on filtre
//            var inst = await _context.OViewInstitutionSectorielle
//                                     .FirstOrDefaultAsync(i => i.IdInstitutionSectorielle == id);
//            if (inst == null)
//                return NotFound();

//            var sections = await _context.OViewSectionInstitution.ToListAsync();
//            var model = new InstitutionSectorielleViewModel
//            {
//                IdInstitutionSectorielle = inst.IdInstitutionSectorielle,
//                NomInstitutionSectorielle = inst.NomInstitutionSectorielle,
//                MissionInstitutionSectorielle = inst.MissionInstitutionSectorielle,
//                AttributionsInstitutionSectorielle = inst.AttributionsInstitutionSectorielle,
//                NomsSections = sections
//                    .Where(s => s.IdInstitutionSectorielle.ToString() == id)
//                    .Select(s => s.NomSection)
//                    .ToList()
//            };

//            return View(model);
//        }

//        // POST: BanqueProjet/InstitutionSectorielle/DeleteConfirmed/5
//        [HttpPost, ActionName("DeleteConfirmed"), ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(string id)
//        {
//            const string sql = "BEGIN SUPPRIMER_INSTITUTION_SECTORIELLE(:p_ID); END;";
//            var parm = new OracleParameter("p_ID", id);

//            await _context.Database.ExecuteSqlRawAsync(sql, parm);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
