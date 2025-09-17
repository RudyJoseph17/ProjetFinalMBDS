using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Authorization;
using InvestissementsPublics.Starter.ApplicationUsers;

namespace InvestissementsPublics.Starter.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var db = sp.GetRequiredService<ApplicationDbContext>();

            var roles = new[] { "Gestionnaire systeme", "Admin" };
            var privileges = new[]
            {
                new { Name = "BanqueProjet.Projet.Create", Description = "Créer un projet BanqueProjet" },
                new { Name = "BanqueProjet.Projet.Edit", Description = "Modifier un projet BanqueProjet" },
                new { Name = "Programmation.Prevision.Approve", Description = "Approuver une prévision" }
            };

            // --- 1) Créer les rôles si manquants
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var res = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!res.Succeeded)
                        Console.WriteLine($"Erreur création rôle '{roleName}': {string.Join(", ", res.Errors.Select(e => e.Description))}");
                    else
                        Console.WriteLine($"Rôle créé: {roleName}");
                }
            }

            // --- 2) Créer admin si absent
            var adminUserName = "rudyJ";
            var adminEmail = "admin@site.com";
            var admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = adminUserName, Email = adminEmail, EmailConfirmed = true };
                var createRes = await userManager.CreateAsync(admin, "Admin@123");
                if (!createRes.Succeeded)
                    Console.WriteLine($"Erreur création admin: {string.Join(", ", createRes.Errors.Select(e => e.Description))}");
                else
                    Console.WriteLine("✅ Utilisateur admin créé");
            }

            // --- 3) Seed privileges + RolePrivileges dans une transaction
            using var tx = await db.Database.BeginTransactionAsync();
            try
            {
                foreach (var p in privileges)
                {
                    var privilege = await db.Privileges.FirstOrDefaultAsync(x => x.Name == p.Name);
                    if (privilege == null)
                    {
                        privilege = new Privilege { Name = p.Name, Description = p.Description };
                        db.Privileges.Add(privilege);
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Privilege ajouté: {p.Name}");
                    }

                    // Lier chaque privilege au rôle "Gestionnaire systeme" si pas déjà lié
                    var role = await roleManager.FindByNameAsync("Gestionnaire systeme");
                    if (role != null)
                    {
                        var exists = await db.RolePrivileges.CountAsync(rp => rp.RoleId == role.Id && rp.PrivilegeId == privilege.Id);
                        if (exists == 0)
                        {
                            db.RolePrivileges.Add(new RolePrivilege { RoleId = role.Id, PrivilegeId = privilege.Id });
                            Console.WriteLine($"Lien RolePrivilege: role='Gestionnaire systeme' <- privilege='{privilege.Name}'");
                        }
                    }

                    // Ajouter claim à admin si pas présent
                    if (admin != null)
                    {
                        var hasClaim = (await userManager.GetClaimsAsync(admin)).Any(c => c.Type == "privilege" && c.Value == privilege.Name);
                        if (!hasClaim)
                        {
                            var addClaimRes = await userManager.AddClaimAsync(admin, new Claim("privilege", privilege.Name));
                            if (addClaimRes.Succeeded)
                                Console.WriteLine($"Claim ajouté à admin: {privilege.Name}");
                        }
                    }
                }

                await db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du seed: {ex.Message}");
                await tx.RollbackAsync();
            }

            // --- 4) Assurer que admin est membre du rôle Admin
            if (admin != null && !await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var res = await userManager.AddToRoleAsync(admin, "Admin");
                if (res.Succeeded)
                    Console.WriteLine("✅ Admin ajouté au rôle 'Admin'");
                else
                    Console.WriteLine($"Erreur ajout admin au rôle 'Admin': {string.Join(", ", res.Errors.Select(e => e.Description))}");
            }

            Console.WriteLine("Seed identity terminé.");
        }
    }
}
