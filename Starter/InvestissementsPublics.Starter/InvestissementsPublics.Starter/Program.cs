using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using InvestissementsPublics.Starter.Data;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Programmation.Infrastructure.Data;
using BanqueProjet.Infrastructure.Data;
using SuiviEvaluation.Infrastructure.Data;
using SuiviEvaluation.Application.Interfaces;
using BanqueProjet.Application.Interfaces;
using Shared.Domain.ApplicationUsers;
////using Shared.Infrastructure.Notifications;
using Shared.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.UI.Services;
using Shared.Infrastructure.Data;
using BanqueProjet.Infrastructure.Persistence;
using InvestissementsPublics.Starter.ApplicationUsers;
using InvestissementsPublics.Starter.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Charger les variables d'environnement depuis .env
DotNetEnv.Env.Load();

// 2) Lire les variables
var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

// 3) Construire la chaîne de connexion Oracle
var connectionString =
    $"User Id={user};Password={password};Data Source={host}:{port}/{service};Pooling=true;";
Console.WriteLine("🔧 Connection string utilisée : " + connectionString);

// 4) Enregistrer les services MVC et les ApplicationParts
builder.Services
    .AddControllersWithViews()
    .AddApplicationPart(typeof(BanqueProjet.Web.Areas.BanqueProjet.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(Programmation.Web.Areas.Programmation.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(SuiviEvaluation.Web.Areas.SuiviEvaluation.Controllers.HomeController).Assembly)
    .AddApplicationPart(typeof(TableauxDeBord.Web.Areas.TableauxDeBord.Controllers.HomeController).Assembly)
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

//5) Enregistrer ApplicationDbContext(Identity)
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseOracle(connectionString));

// 6) **Enregistrer ProjetDbContext** en réutilisant `connectionString`
//    (au lieu de GetConnectionString, qui était null)

// …

// 5) Enregistrer le contexte Identity : ApplicationDbContext
//builder.Services.AddDbContext<ApplicationDbContext>(opts =>
//    opts.UseOracle(
//        connectionString,
//        b => b.MigrationsAssembly("InvestissementsPublics.Starter")
//    )
//);
// 6) Configurer Identity sur ce contexte
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();



builder.Services.
AddDbContext<Programmation.Infrastructure.Data.ProgrammationDbContext>(opts =>
    opts.UseOracle(
        connectionString,
        b => b.MigrationsAssembly("Programmation.Infrastructure"))
);

builder.Services.AddDbContext<BanqueProjet.Infrastructure.Data.BanquePDbContext>(opts =>
    opts.UseOracle(
        connectionString,
        b => b.MigrationsAssembly("BanqueProjet.Infrastructure"))
);

builder.Services.AddDbContext<SuiviEvaluation.Infrastructure.Data.EvaluationDbContext>(opts =>
    opts.UseOracle(
        connectionString,
        b => b.MigrationsAssembly("SuiviEvaluation.Infrastructure"))
);

builder.Services.AddDbContext<Shared.Infrastructure.Data.SharedDbContext>(opts =>
    opts.UseOracle(
        connectionString,
        b => b.MigrationsAssembly("Shared.Infrastructure"))
);


// 7) Configurer Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 8) Cookie settings
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.LogoutPath = "/Account/Logout";
    opts.ExpireTimeSpan = TimeSpan.FromHours(2);
});

// Email sender (votre implémentation SMTP/SendGrid)
builder.Services.AddTransient<Shared.Domain.ApplicationUsers.IEmailSender, SmtpEmailSender>();


// Notification
////builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IIdentificationProjetService, IdentificationProjetService>();
builder.Services.AddScoped<IDdpCadreLogiqueService, DdpCadreLogiqueService>();
builder.Services.AddScoped<IAspectsJuridiquesService, AspectsJuridiquesService>();
//builder.Services.AddScoped<IEchelonTerritorialeService, EchelonTerritorialeService>();
//builder.Services.AddScoped<IEmploisCreesService, EmploisCreesService>();
//builder.Services.AddScoped<IDureeProjetService, DureeProjetService>();
builder.Services.AddScoped<IGestionDeProjetService, GestionDeProjetService>();
builder.Services.AddScoped<ILocalisationGeographiqueProjService, LocalisationGeographiqueProjService>();
//builder.Services.AddScoped<ISuiviEtControleService, SuiviEtControleService>();
//builder.Services.AddScoped<IGrilleDdpAviService, GrilleDdpAviService>();
//builder.Services.AddScoped<IGrilleDdpIdentificationProjetService, GrilleDdpIdentificationProjetService>();
//builder.Services.AddScoped<IGrilleDdpAspectsLegauxService, GrilleDdpAspectsLegauxService>();
//builder.Services.AddScoped<IGrilleDdpResumeProjetService, GrilleDdpResumeProjetService>();
//builder.Services.AddScoped<IGrilleDdpEtudesPrefaisabiliteService, GrilleDdpEtudesPrefaisabiliteService>();
//builder.Services.AddScoped<IGrilleDdpStrategieGestionProjetService, GrilleDdpStrategieGestionProjetService>();
//builder.Services.AddScoped<IGrilleDdpCommentairesGenerauxService, GrilleDdpCommentairesGenerauxService>();
//builder.Services.AddScoped<IGrilleDdpCalendrierFinancierProjetService, GrilleDdpCalendrierFinancierProjetService>();
//builder.Services.AddScoped<IGrilleDdpCalendrierExecutionService, GrilleDdpCalendrierExecutionService>();
//builder.Services.AddScoped<IPrevisionService, PrevisionService>();
//builder.Services.AddScoped<IActiviteService, ActiviteService>();
//builder.Services.AddScoped<ICoutAnnuelService, CoutAnnuelService>();


var app = builder.Build();

// 9) Seed des rôles / users
using var scope = app.Services.CreateScope();
await IdentitySeeder.SeedAsync(
    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());

// 10) Pipeline HTTP
app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 11) Routing avec Areas
app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
