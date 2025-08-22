using DotNetEnv;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauxDeBord.Infrastructure.Data
{
    public class ProjetDbContextFactory : IDesignTimeDbContextFactory<ProjetDbContext>
    {
        public ProjetDbContext CreateDbContext(string[] args)
        {
            // 1) Charger automatiquement le fichier .env
            Env.Load();

            // 2) Récupérer la variable d'environnement ConnectionStrings_ProjetConnection
            var conn = Environment.GetEnvironmentVariable("ConnectionStrings_ProjetConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ConnectionStrings_ProjetConnection n'est pas défini. Vérifiez votre .env.");

            // 3) Construire les options EF Core avec UseOracle
            var optionsBuilder = new DbContextOptionsBuilder<ProjetDbContext>();
            optionsBuilder.UseOracle(conn);

            return new ProjetDbContext(optionsBuilder.Options);
        }
    }
}
