using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Programmation.Infrastructure.Data;
using Programmation.Infrastructure;
using DotNetEnv;                // <— import pour DotNetEnv



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


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Utiliser la chaîne de connexion du fichier .env
builder.Services.AddDbContext<ProgrammationDbContext>(options =>
    options.UseOracle(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();*/

app.Run();

/*record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/
