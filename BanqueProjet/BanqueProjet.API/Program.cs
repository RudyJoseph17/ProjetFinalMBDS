using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BanqueProjet.Infrastructure; // Assure-toi que ce namespace contient ApplicationDbContext
using BanqueProjet.Infrastructure.Data;
using DotNetEnv;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Persistence;




// Charger les variables d'environnement depuis le fichier .env
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

// Lire la chaîne de connexion depuis la variable d’environnement

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion Oracle n'est pas définie.");
}

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Utiliser la chaîne de connexion du fichier .env
builder.Services.AddDbContext<BanquePDbContext>(options =>
    options.UseOracle(connectionString));




builder.Services.AddScoped<IIdentificationProjetService, IdentificationProjetService>();
//builder.Services.AddScoped<IDdpCadreLogiqueService, DdpCadreLogiqueService>();
//builder.Services.AddScoped<IAspectsInstirutionnelService, AspectsInstirutionnelService>();
//builder.Services.AddScoped<IEchelonTerritorialeService, EchelonTerritorialeService>();
//builder.Services.AddScoped<IEmploisCreesService, EmploisCreesService>();
//builder.Services.AddScoped<IDureeProjetService, DureeProjetService>();
//builder.Services.AddScoped<IGestionDeProjetService, GestionDeProjetService>();
//builder.Services.AddScoped<ILocalisationGeographiqueProjService, LocalisationGeographiqueProjService>();
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

// Middleware
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// ?? Mappez vos controllers pour qu’ils soient exposés
app.MapControllers();

// Endpoint de test
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
