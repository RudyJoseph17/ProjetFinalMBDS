using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuiviEvaluation.Infrastructure.Data;
using DotNetEnv;
using SuiviEvaluation.Application.Interfaces;




var builder = WebApplication.CreateBuilder(args);

// 1) Charger les variables d'environnement depuis .env
//DotNetEnv.Env.Load();

//// 2) Lire les variables
//var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
//var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
//var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
//var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
//var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

//// 3) Construire la chaîne de connexion Oracle
//var connectionString =
//    $"User Id={user};Password={password};Data Source={host}:{port}/{service};Pooling=true;";
//Console.WriteLine("?? Connection string utilisée : " + connectionString);


// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IEvolutionTemporelleService, EvolutionTemporelleService>();
//builder.Services.AddScoped<IFluxFinancierService, FluxFinancierService>();
//builder.Services.AddScoped<IQuantiteLivreParAnneeService, QuantiteLivreParAnneeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Bilan}/{action=Index}/{id?}"
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
