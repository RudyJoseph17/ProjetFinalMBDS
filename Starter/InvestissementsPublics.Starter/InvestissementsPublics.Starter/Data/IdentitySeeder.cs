// InvestissementsPublics.Starter/Data/IdentitySeeder.cs
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
        /// <summary>
        /// Seed général : rôles, utilisateur admin, privileges, role→privilege et (optionnel) claim direct sur l'utilisateur.
        /// Appeler après app.Build() : await IdentitySeeder.SeedAsync(app.Services);
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // --- 1) Roles à créer (ajoute ici les rôles nécessaires)
            var roles = new[] { "Gestionnaire systeme", "Admin" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var err in roleResult.Errors)
                            Console.WriteLine($"Erreur création rôle '{roleName}': {err.Description}");
                    }
                    else
                    {
                        Console.WriteLine($"Rôle créé: {roleName}");
                    }
                }
            }

            // --- 2) Créer l'utilisateur admin si absent
            var adminUserName = "rudyJ";
            var adminEmail = "admin@site.com";
            var admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                    // Ajoute d'autres propriétés si nécessaire (Nom, Prenom, ...)
                };

                Console.WriteLine("🔧 Création de l'utilisateur admin...");

                var createResult = await userManager.CreateAsync(admin, "Admin@123");
                if (!createResult.Succeeded)
                {
                    foreach (var e in createResult.Errors)
                        Console.WriteLine($"Erreur création utilisateur admin: {e.Description}");
                }
                else
                {
                    // Assigner le rôle voulu (ici "Gestionnaire systeme")
                    var assignRoleResult = await userManager.AddToRoleAsync(admin, "Gestionnaire systeme");
                    if (!assignRoleResult.Succeeded)
                    {
                        foreach (var e in assignRoleResult.Errors)
                            Console.WriteLine($"Erreur assignation rôle admin: {e.Description}");
                    }
                    else
                    {
                        Console.WriteLine("✅ Utilisateur admin créé et assigné au rôle 'Gestionnaire systeme'.");
                    }
                }
            }
            else
            {
                Console.WriteLine("ℹ️ Utilisateur admin déjà existant");
            }

            // --- 3) Seed des privileges + sauvegarde (liste d'exemples)
            var privilegesToSeed = new[]
            {
                new { Name = "BanqueProjet.Projet.Create", Description = "Créer un projet BanqueProjet" },
                new { Name = "BanqueProjet.Projet.Edit", Description = "Modifier un projet BanqueProjet" },
                new { Name = "Programmation.Prevision.Approve", Description = "Approuver une prévision" }
            };

            // Utilisation d'une transaction pour cohérence (optionnel mais recommandé)
            using var tx = await db.Database.BeginTransactionAsync();
            try
            {
                foreach (var pInfo in privilegesToSeed)
                {
                    var p = await db.Privileges.FirstOrDefaultAsync(x => x.Name == pInfo.Name);
                    if (p == null)
                    {
                        p = new Privilege { Name = pInfo.Name, Description = pInfo.Description };
                        db.Privileges.Add(p);
                        Console.WriteLine($"Ajout privilege: {pInfo.Name}");
                        // SaveChanges after adding to get Id for relations
                        await db.SaveChangesAsync();
                    }
                }

                // --- 4) Lier certains privileges à un rôle (ex: "Gestionnaire systeme")
                // récupère le rôle cible
                var targetRoleName = "Gestionnaire systeme"; // change si tu veux "Admin"
                var targetRole = await roleManager.FindByNameAsync(targetRoleName);
                if (targetRole != null)
                {
                    var roleId = targetRole.Id;
                    foreach (var pInfo in privilegesToSeed)
                    {
                        var p = await db.Privileges.FirstOrDefaultAsync(x => x.Name == pInfo.Name);
                        if (p != null)
                        {
                            var exists = await db.RolePrivileges.AnyAsync(rp => rp.RoleId == roleId && rp.PrivilegeId == p.Id);
                            if (!exists)
                            {
                                db.RolePrivileges.Add(new RolePrivilege { RoleId = roleId, PrivilegeId = p.Id });
                                Console.WriteLine($"Lien RolePrivilege: role='{targetRoleName}' <- privilege='{p.Name}'");
                            }
                        }
                    }

                    await db.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"⚠️ Rôle cible '{targetRoleName}' introuvable, saut des liaisons RolePrivileges.");
                }

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du seed privileges: {ex.Message}");
                await tx.RollbackAsync();
            }

            // --- 5) Optionnel : ajouter des claims 'privilege' directement sur l'utilisateur admin
            // (utile si tu utilises la stratégie claims-on-login)
            if (admin != null)
            {
                // exemple : attribuer tous les privileges seedés à l'admin en claim direct
                foreach (var pInfo in privilegesToSeed)
                {
                    var hasClaim = (await userManager.GetClaimsAsync(admin)).Any(c => c.Type == "privilege" && c.Value == pInfo.Name);
                    if (!hasClaim)
                    {
                        var addClaimRes = await userManager.AddClaimAsync(admin, new Claim("privilege", pInfo.Name));
                        if (addClaimRes.Succeeded)
                            Console.WriteLine($"Claim ajouté à l'admin: privilege={pInfo.Name}");
                    }
                }
            }

            Console.WriteLine("Seed identity terminé.");
        }
    }
}
