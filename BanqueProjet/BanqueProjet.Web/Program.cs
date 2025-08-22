using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BanqueProjet.Infrastructure.Data;
using DotNetEnv;
using BanqueProjet.Application.Interfaces;
using Oracle.ManagedDataAccess.Types;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.ApplicationUsers;
////using Shared.Infrastructure.Notifications;
using Shared.Infrastructure.Persistence;
using BanqueProjet.Infrastructure.Persistence;


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
Console.WriteLine("?? Connection string utilisée : " + connectionString);


// Add services to the container.
builder.Services.AddControllersWithViews();


//OracleUdt.RegisterTypeMapping(
//    schemaName: "JOSEPHRUDY",
//    packageName: "PKG_TYPES",
//    typeName: "NUM_TABLE",
//    udtFactory: typeof(BanqueProjet.Infrastructure.OracleMapping.NumTableFactory)
//);



// Configure the HTTP request pipeline.


// Email sender (votre implémentation SMTP/SendGrid)
////builder.Services.AddTransient<Shared.Domain.ApplicationUsers.IEmailSender, SmtpEmailSender>();

// Notification
////builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IIdentificationProjetService, IdentificationProjetService>();
builder.Services.AddScoped<IDdpCadreLogiqueService, DdpCadreLogiqueService>();
//builder.Services.AddScoped<IAspectsInstirutionnelService, AspectsInstirutionnelService>();
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
//builder.Services.AddScoped<IAspectsLegauxService, AspectsLegauxService>();
//builder.Services.AddScoped<ICoutAnnuelService, CoutAnnuelService>();
//builder.Services.AddScoped<ICoutTotalProjetService, CoutTotalProjetService>();
//builder.Services.AddScoped<IAspectsInstitutionnelleService, AspectsInstitutionnelleService>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // ? Log dans la console de debug

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.MapDefaultControllerRoute();

app.Run();
