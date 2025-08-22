using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using InvestissementsPublics.Starter.ApplicationUsers;
using InvestissementsPublics.Starter.Models;

namespace InvestissementsPublics.Starter.Data
{
    public class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Gestionnaire systeme" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var err in roleResult.Errors)
                            Console.WriteLine($"Erreur création rôle: {err.Description}");
                    }
                }
            }

            var user = await userManager.FindByNameAsync("Gestionnaire systeme");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "rudyJ",
                    Email = "admin@site.com",
                    EmailConfirmed = true,
                    //Nom = "Admin",
                    //Prenom = "Systeme",
                    //Institution = "DSEIP"
                };

                Console.WriteLine("🔧 Création de l'utilisateur admin...");

                Console.WriteLine($"🧪 UserName: {user.UserName}");
                Console.WriteLine($"🧪 Email: {user.Email}");


                var result = await userManager.CreateAsync(user, "Admin@123");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Erreur création utilisateur: {error.Description}");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "Gestionnaire systeme");
                    Console.WriteLine("✅ Utilisateur admin créé avec succès");
                }
            }
            else
            {
                Console.WriteLine("ℹ️ Utilisateur admin déjà existant");
            }

            var existing = await userManager.FindByNameAsync("admin");
            if (existing != null)
            {
                await userManager.DeleteAsync(existing);
            }

        }

    }
}
