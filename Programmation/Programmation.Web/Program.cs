using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Programmation.Infrastructure.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

//// Charger les variables d'environnement depuis .env
//DotNetEnv.Env.Load();

// Test de debug
//Console.WriteLine($"DEBUG ? ORACLE_DB_USER={Environment.GetEnvironmentVariable("ProjetConnection")}");
//Console.WriteLine($"DEBUG ? ORACLE_DB_PASSWORD={(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProjetConnection")) ? "[NULL]" : "[RECU]")}");


// Lire les variables
//var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
//var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
//var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
//var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
//var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

//// Construire la chaîne de connexion Oracle
//var connectionString = $"User Id={user};Password={password};Data Source={host}:{port}/{service};Pooling=true;";

//// Ajouter le DbContext Oracle
//builder.Services.AddDbContext<ProjetDbContext>(options =>
//    options.UseOracle(connectionString));

//var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
//logger.LogInformation("Connection string used: {conn}", connectionString);


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.Run();
