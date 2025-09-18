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
using Shared.Domain.Interface;
using BanqueProjet.Application.Mapping;
using AutoMapper;
using Shared.Infrastructure.Data;
using BanqueProjet.Application.Mapping;
using Shared.Infrastructure.Mapping;
using BanqueProjet.Infrastructure.Diagnostics;
using Serilog;





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
///
// ?? Configurer Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // log tout à partir de Debug
    .WriteTo.Console()
    .WriteTo.File(
        path: @"C:\Users\rudyj\MBDS\9-STAGE_ET_PROJET_FINAL\DERNIERS_SCRIPTS\BanqueProjet-.log",
        rollingInterval: RollingInterval.Day, // un fichier par jour
        retainedFileCountLimit: 10,           // garder 10 jours d’historique
        shared: true
    )
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Scan et enregistrement des profiles AutoMapper
// Enregistrer le DbContext EF Core
builder.Services.AddDbContext<SharedDbContext>(opts =>
    opts.UseOracle(connectionString)
);

// Enregistrer AutoMapper (tous vos profiles)
builder.Services.AddAutoMapper(
    typeof(SharedMappingProfile).Assembly,
    typeof(ProjetProfile).Assembly
);
builder.Services.AddAutoMapper(typeof(ProjetProfile).Assembly);
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
builder.Services.AddScoped<IGrilleDdpProjetService, GrilleDdpProjetService>();
builder.Services.AddDbContext<SharedDbContext>((sp, options) =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(new OracleCommandInterceptor());
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // durée de vie
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // durée de vie de la session
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});


builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // ? Log dans la console de debug

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{
    var mapper = scope.ServiceProvider.GetRequiredService<AutoMapper.IMapper>();
    try
    {
        mapper.ConfigurationProvider.AssertConfigurationIsValid(); // lance une exception si config invalide
    }
    catch (AutoMapperConfigurationException ex)
    {
        // log détaillé et rethrow pour voir l'erreur au démarrage
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "AutoMapper configuration invalid: {Message}", ex.Message);
        throw;
    }
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


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
