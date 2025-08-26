using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using BanqueProjet.Infrastructure.Data;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Persistence;
using Shared.Domain.Interface;
using Shared.Domain.ApplicationUsers;
using BanqueProjet.Application.Mapping; // si ProjetProfile est ici
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Persistence;
using InvestissementsPublics.Starter.Data;
using InvestissementsPublics.Starter.ApplicationUsers;
using Shared.Infrastructure.Mapping;       // si SharedMappingProfile est ici
// ajoute d'autres usings de profiles si besoin

var builder = WebApplication.CreateBuilder(args);

// Charger .env
DotNetEnv.Env.Load();

// Lire variables env et construire la connection string
var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

var connectionString = $"User Id={user};Password={password};Data Source={host}:{port}/{service};Pooling=true;";
Console.WriteLine("🔧 Connection string utilisée : " + connectionString);

// MVC + ApplicationParts + Razor runtime compilation
builder.Services
    .AddControllersWithViews()
    .AddApplicationPart(typeof(BanqueProjet.Web.Areas.BanqueProjet.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(Programmation.Web.Areas.Programmation.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(TableauxDeBord.Web.Areas.TableauxDeBord.Controllers.HomeController).Assembly)
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

// DbContexts (un seul enregistrement par DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseOracle(connectionString));

builder.Services.AddDbContext<SharedDbContext>(opts =>
    opts.UseOracle(connectionString, b => b.MigrationsAssembly("Shared.Infrastructure")));

builder.Services.AddDbContext<Programmation.Infrastructure.Data.ProgrammationDbContext>(opts =>
    opts.UseOracle(connectionString, b => b.MigrationsAssembly("Programmation.Infrastructure")));

builder.Services.AddDbContext<BanqueProjet.Infrastructure.Data.BanquePDbContext>(opts =>
    opts.UseOracle(connectionString, b => b.MigrationsAssembly("BanqueProjet.Infrastructure")));

builder.Services.AddDbContext<SuiviEvaluation.Infrastructure.Data.EvaluationDbContext>(opts =>
    opts.UseOracle(connectionString, b => b.MigrationsAssembly("SuiviEvaluation.Infrastructure")));

// Identity (sur ApplicationDbContext)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// AutoMapper : enregistrement UNIQUE, avant d'enregistrer les services qui utilisent IMapper.
// On indique ici les types marquant les assemblies contenant les Profiles (plus fiable que AppDomain scan si assemblies sont séparées).
builder.Services.AddAutoMapper(
    typeof(SharedMappingProfile), // remplace par le type réel du profile si diffèrent
    typeof(ProjetProfile)         // idem
);

// Enregistrement des services (pas de doublons)
builder.Services.AddScoped<IIdentificationProjetService, IdentificationProjetService>();
builder.Services.AddScoped<IProjetsBPService, ProjetsBPService>();
builder.Services.AddScoped<IActiviteService, ActiviteService>();
builder.Services.AddScoped<IActiviteBPService, ActiviteBPService>();
builder.Services.AddScoped<IDdpCadreLogiqueService, DdpCadreLogiqueService>();
builder.Services.AddScoped<IAspectsJuridiquesService, AspectsJuridiquesService>();
builder.Services.AddScoped<ILocalisationGeographiqueProjService, LocalisationGeographiqueProjService>();
builder.Services.AddScoped<IActivitesAnnuellesService, ActivitesAnnuellesService>();
builder.Services.AddScoped<IBailleursDeFondService, BailleursDeFondService>();
builder.Services.AddScoped<IPartiesPrenantesService, PartiesPrenantesService>();
builder.Services.AddScoped<ICoutAnnuelDuProjetService, CoutAnnuelDuProjetService>();
builder.Services.AddScoped<IEffetsDuProjetService, EffetsDuProjetService>();
builder.Services.AddScoped<IImpactsDuProjetService, ImpactsDuProjetService>();
builder.Services.AddScoped<IIndicateursDeResultatService, IndicateursDeResultatService>();
builder.Services.AddScoped<IInformationsFinancieresService, InformationsFinancieresService>();
builder.Services.AddScoped<IDefinitionLivrablesDuProjetService, DefinitionLivrablesDuProjetService>();
builder.Services.AddScoped<IObjectifsSpecifiquesService, ObjectifsSpecifiquesService>();


// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Seed (création des rôles/users) - crée un scope et utilise GetRequiredService
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
}

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
