using InvestissementsPublics.Starter.ApplicationUsers;
using InvestissementsPublics.Starter.Models;
using InvestissementsPublics.Starter.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Domain.ApplicationUsers;
using System.Linq;
using System.Threading.Tasks;

namespace InvestissementsPublics.Starter.Controllers
{
    [Authorize(Roles = "Gestionnaire systeme")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userMgr = userManager;
            _roleMgr = roleManager;
        }

        public async Task<IActionResult> Utilisateurs()
        {
            var users = _userMgr.Users.ToList();
            var vm = new List<UtilisateurListeViewModel>();
            foreach (var u in users)
            {
                var roles = await _userMgr.GetRolesAsync(u);
                vm.Add(new UtilisateurListeViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName!,
                    Email = u.Email!,
                    Roles = roles.ToList()
                });
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreerUtilisateur()
        {
            var vm = new UtilisateurViewModel();
            RemplirDropdownRoles(vm, null);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreerUtilisateur(UtilisateurViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            var user = new ApplicationUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                EmailConfirmed = true
            };

            var create = await _userMgr.CreateAsync(user, vm.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors)
                    ModelState.AddModelError("", e.Description);

                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            await _userMgr.AddToRoleAsync(user, vm.SelectedRole);
            return RedirectToAction(nameof(Utilisateurs));
        }

        [HttpGet]
        public async Task<IActionResult> ModifierUtilisateur(string id)
        {
            var user = await _userMgr.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRole = (await _userMgr.GetRolesAsync(user)).FirstOrDefault() ?? "";
            var vm = new UtilisateurViewModel
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                SelectedRole = currentRole
            };

            RemplirDropdownRoles(vm, currentRole);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifierUtilisateur(UtilisateurViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            var user = await _userMgr.FindByIdAsync(vm.Id);
            if (user == null)
            {
                ModelState.AddModelError("", "Utilisateur non trouvé.");
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            if (string.IsNullOrWhiteSpace(vm.SelectedRole))
            {
                ModelState.AddModelError("SelectedRole", "Le rôle doit être sélectionné.");
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            var roleExists = await _roleMgr.RoleExistsAsync(vm.SelectedRole);
            if (!roleExists)
            {
                ModelState.AddModelError("SelectedRole", $"Le rôle '{vm.SelectedRole}' n'existe pas.");
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            user.UserName = vm.UserName;
            user.Email = vm.Email;
            var upd = await _userMgr.UpdateAsync(user);
            if (!upd.Succeeded)
            {
                foreach (var e in upd.Errors)
                    ModelState.AddModelError("", e.Description);
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            // 2) Réinitialiser le mot de passe
            //    On génère d'abord un token "one-time"
            var resetToken = await _userMgr.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userMgr.ResetPasswordAsync(user, resetToken, vm.Password);
            if (!resetResult.Succeeded)
            {
                foreach (var e in resetResult.Errors)
                    ModelState.AddModelError("", e.Description);
                RemplirDropdownRoles(vm, vm.SelectedRole);
                return View(vm);
            }

            // 3) Mettre à jour le rôle (exemple avec rôle unique)
            var oldRoles = await _userMgr.GetRolesAsync(user);
            if (oldRoles.Any())
                await _userMgr.RemoveFromRolesAsync(user, oldRoles);
            await _userMgr.AddToRoleAsync(user, vm.SelectedRole);

            return RedirectToAction(nameof(Utilisateurs));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerUtilisateur(string id)
        {
            var user = await _userMgr.FindByIdAsync(id);
            if (user != null)
                await _userMgr.DeleteAsync(user);
            return RedirectToAction(nameof(Utilisateurs));
        }

        private void RemplirDropdownRoles(UtilisateurViewModel vm, string? selectedRole)
        {
            var allRoleNames = _roleMgr.Roles
                                       .Select(r => r.Name!)
                                       .ToList();

            vm.AvailableRoles = allRoleNames
                .Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name,
                    Selected = name == selectedRole
                })
                .ToList();
        }
    }
}
