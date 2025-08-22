using System;
using System.IO;
using DotNetEnv;
using InvestissementsPublics.Starter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvestissementsPublics.Starter.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 1) Charge le fichier .env à la racine du projet Starter
            //    (recalcule le path si nécessaire pour point to your .env)
            var basePath = Directory.GetCurrentDirectory();
            Env.Load(Path.Combine(basePath, ".env"));

            // 2) Récupère vos variables d’environnement
            var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
            var pwd = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
            var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
            var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
            var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

            if (string.IsNullOrEmpty(user)
             || string.IsNullOrEmpty(pwd)
             || string.IsNullOrEmpty(host)
             || string.IsNullOrEmpty(port)
             || string.IsNullOrEmpty(service))
            {
                throw new InvalidOperationException(
                  "Impossible de lire les variables d'environnement Oracle pour la migration EF.");
            }

            var connString = $"User Id={user};Password={pwd};" +
                             $"Data Source={host}:{port}/{service};Pooling=true;";

            // 3) Configure les options EF Core
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseOracle(connString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
