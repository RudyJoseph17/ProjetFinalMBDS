//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Oracle.ManagedDataAccess.Client;
//using BanqueProjet.Infrastructure.Data;
//using BanqueProjet.Web.Models;


//namespace BanqueProjet.Web.Areas.BanqueProjet.Controllers
//{
//    [Area("BanqueProjet")]
//    public class SectionInstitutionController : Controller
//    {
//        private readonly BanquePDbContext _context;

//        public SectionInstitutionController(BanquePDbContext context)
//        {
//            _context = context;
//        }

//        // GET: BanqueProjet/SectionInstitution
//        public async Task<IActionResult> Index()
//        {
//            // Charge la vue plate, sans jointure
//            var sections = await _context.OViewSectionInstitution
//                                         .ToListAsync();

//            var model = sections
//                .Select(s => new SectionInstitutionViewModel
//                {
//                    IdSectionInstitution = s.IdSectionInstitution,
//                    IdInstitutionSectorielle = s.IdInstitutionSectorielle.ToString(),
//                    NomSection = s.NomSection
//                })
//                .ToList();

//            return View(model);
//        }

//        // GET: BanqueProjet/SectionInstitution/Create
//        public IActionResult Create()
//            => View();

//        // POST: BanqueProjet/SectionInstitution/Create
//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(
//            [Bind("IdSectionInstitution,IdInstitutionSectorielle,NomSection")]
//            SectionInstitutionViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            const string sql = @"
//                BEGIN
//                  AJOUTER_SECTION_INSTITUTION(
//                    :p_ID_SECTION,
//                    :p_ID_INSTITUTION,
//                    :p_NOM_SECTION
//                  );
//                END;";

//            var parms = new[]
//            {
//                new OracleParameter("p_ID_SECTION", model.IdSectionInstitution),
//                new OracleParameter("p_ID_INSTITUTION", model.IdInstitutionSectorielle),
//                new OracleParameter("p_NOM_SECTION", model.NomSection)
//            };

//            await _context.Database.ExecuteSqlRawAsync(sql, parms);
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: BanqueProjet/SectionInstitution/Delete/5
//        public async Task<IActionResult> Delete(long id)
//        {
//            var s = await _context.OViewSectionInstitution
//                                  .FirstOrDefaultAsync(x => x.IdSectionInstitution == id);

//            if (s == null)
//                return NotFound();

//            var model = new SectionInstitutionViewModel
//            {
//                IdSectionInstitution = s.IdSectionInstitution,
//                IdInstitutionSectorielle = s.IdInstitutionSectorielle.ToString(),
//                NomSection = s.NomSection
//            };

//            return View(model);
//        }

//        // POST: BanqueProjet/SectionInstitution/DeleteConfirmed/5
//        [HttpPost, ActionName("DeleteConfirmed"), ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(long id)
//        {
//            const string sql = "BEGIN SUPPRIMER_SECTION_INSTITUTION(:p_ID_SECTION); END;";
//            var parm = new OracleParameter("p_ID_SECTION", id);

//            await _context.Database.ExecuteSqlRawAsync(sql, parm);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
