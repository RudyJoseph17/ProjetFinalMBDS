using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;

namespace BanqueProjet.Infrastructure.Data
{
    public class BanquePDbContextFactory : IDesignTimeDbContextFactory<BanquePDbContext>
    {
        public BanquePDbContext CreateDbContext(string[] args)
        {
            // Charger .env
            Env.Load();

            // Récupérer les variables individuelles
            var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
            var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
            var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
            var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
            var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(port) ||
                string.IsNullOrWhiteSpace(service))
            {
                throw new InvalidOperationException("Une ou plusieurs variables d'environnement Oracle sont manquantes dans le fichier .env");
            }

            // Construire la chaîne de connexion
            var conn = $"User Id={user};Password={password};Data Source={host}:{port}/{service};";

            var optionsBuilder = new DbContextOptionsBuilder<BanquePDbContext>();
            optionsBuilder.UseOracle(conn);

            return new BanquePDbContext(optionsBuilder.Options);
        }
    }
}
