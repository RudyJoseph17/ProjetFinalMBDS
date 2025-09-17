using InvestissementsPublics.Starter.ApplicationUsers;
using InvestissementsPublics.Starter.Data;
using InvestissementsPublics.Starter.Models.Privileges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace InvestissementsPublics.Starter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PrivilegesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrivilegesController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Admin/Privileges
        public async Task<IActionResult> Index()
        {
            var list = await _db.Privileges
                                .AsNoTracking()
                                .Select(p => new PrivilegeListItemVm
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    RoleCount = p.RolePrivileges.Count(),
                                    UserDirectCount = _db.UserClaims.Count(uc => uc.ClaimType == "privilege" && uc.ClaimValue == p.Name)
                                })
                                .ToListAsync();

            var vm = new PrivilegeIndexVm { Privileges = list };
            return View(vm);
        }

        // GET: Admin/Privileges/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PrivilegeCreateVm();
            vm.AvailableRoles = await _roleManager.Roles
                                      .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(r.Name, r.Id))
                                      .ToListAsync();

            vm.AvailableUsers = await _userManager.Users
                                      .Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(u.UserName ?? u.Email ?? u.Id, u.Id))
                                      .ToListAsync();

            return View(vm);
        }

        // POST: Admin/Privileges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrivilegeCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                // reload lists
                model.AvailableRoles = await _roleManager.Roles
                                          .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(r.Name, r.Id))
                                          .ToListAsync();

                model.AvailableUsers = await _userManager.Users
                                          .Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(u.UserName ?? u.Email ?? u.Id, u.Id))
                                          .ToListAsync();

                return View(model);
            }

            // Vérifier unicité
            var exists = await _db.Privileges.AnyAsync(p => p.Name == model.Name);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Name), "Un privilège avec ce nom existe déjà.");
                model.AvailableRoles = await _roleManager.Roles
                                          .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(r.Name, r.Id))
                                          .ToListAsync();

                model.AvailableUsers = await _userManager.Users
                                          .Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(u.UserName ?? u.Email ?? u.Id, u.Id))
                                          .ToListAsync();

                return View(model);
            }

            var privilege = new Privilege
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim()
            };

            _db.Privileges.Add(privilege);
            await _db.SaveChangesAsync();

            // lier aux rôles sélectionnés (RolePrivileges)
            if (model.SelectedRoleIds?.Any() == true)
            {
                foreach (var roleId in model.SelectedRoleIds.Distinct())
                {
                    _db.RolePrivileges.Add(new RolePrivilege { RoleId = roleId, PrivilegeId = privilege.Id });
                }
                await _db.SaveChangesAsync();
            }

            // ajouter claims "privilege" aux utilisateurs sélectionnés (attribution directe)
            if (model.SelectedUserIds?.Any() == true)
            {
                foreach (var userId in model.SelectedUserIds.Distinct())
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        // n'ajoute pas en double
                        var already = await _userManager.GetClaimsAsync(user);
                        if (!already.Any(c => c.Type == "privilege" && c.Value == privilege.Name))
                        {
                            await _userManager.AddClaimAsync(user, new Claim("privilege", privilege.Name));
                        }
                    }
                }
            }

            TempData["Success"] = "Privilège créé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Privileges/Details/5
        // GET: Admin/Privileges/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var p = await _db.Privileges
                             .Include(x => x.RolePrivileges)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound();

            var vm = new PrivilegeDetailsVm
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            };

            // rôles liés
            var roles = await _db.RolePrivileges
                                 .Where(rp => rp.PrivilegeId == id)
                                 .Select(rp => rp.RoleId)
                                 .ToListAsync();

            vm.Roles = await _roleManager.Roles
                .Where(r => roles.Contains(r.Id))
                .Select(r => new InvestissementsPublics.Starter.Models.Account.RoleSelection
                {
                    RoleId = r.Id,
                    RoleName = r.Name ?? r.Id
                })
                .ToListAsync();

            // utilisateurs avec claim direct
            var userClaims = await _db.UserClaims
                                      .AsNoTracking()
                                      .Where(uc => uc.ClaimType == "privilege" && uc.ClaimValue == p.Name)
                                      .Select(uc => uc.UserId)
                                      .ToListAsync();

            vm.Users = await _userManager.Users
                .Where(u => userClaims.Contains(u.Id))
                .Select(u => new InvestissementsPublics.Starter.Models.Account.UserVm
                {
                    Id = u.Id,
                    DisplayName = u.UserName ?? u.Email ?? u.Id,
                    Email = u.Email
                })
                .ToListAsync();

            return View(vm);
        }


        // GET: Admin/Privileges/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Privileges
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            var rolesLinked = await _db.RolePrivileges.CountAsync(rp => rp.PrivilegeId == id);
            var usersLinked = await _db.UserClaims.CountAsync(uc => uc.ClaimType == "privilege" && uc.ClaimValue == p.Name);

            var vm = new PrivilegeDeleteVm
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                RolesLinked = rolesLinked,
                UsersLinked = usersLinked
            };

            return View(vm);
        }

        // POST: Admin/Privileges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _db.Privileges.FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            // supprimer roleprivileges
            var rps = _db.RolePrivileges.Where(rp => rp.PrivilegeId == id);
            _db.RolePrivileges.RemoveRange(rps);

            // supprimer claims "privilege" des utilisateurs
            var userClaims = _db.UserClaims.Where(uc => uc.ClaimType == "privilege" && uc.ClaimValue == p.Name);
            _db.UserClaims.RemoveRange(userClaims);

            // supprimer privilège
            _db.Privileges.Remove(p);

            await _db.SaveChangesAsync();

            TempData["Success"] = "Privilège supprimé.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Privileges/SearchUsers?q=term&page=1
        [HttpGet]
        [AllowAnonymous] // ou Authorize si tu veux restreindre qui peut appeler l'endpoint
        public async Task<IActionResult> SearchUsers(string q, int page = 1, int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new { items = new object[] { }, more = false });

            q = q.Trim();

            var query = _userManager.Users
                .Where(u => (u.UserName != null && u.UserName.Contains(q)) || (u.Email != null && u.Email.Contains(q)))
                .OrderBy(u => u.UserName);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
                          .Select(u => new { id = u.Id, text = (u.UserName ?? u.Email ?? u.Id) })
                          .ToListAsync();

            var more = (page * pageSize) < total;

            return Ok(new { items, more });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByIds([FromQuery] string[] ids)
        {
            var users = await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => new { id = u.Id, text = u.UserName ?? u.Email ?? u.Id })
                .ToListAsync();
            return Ok(users);
        }


    }
}
