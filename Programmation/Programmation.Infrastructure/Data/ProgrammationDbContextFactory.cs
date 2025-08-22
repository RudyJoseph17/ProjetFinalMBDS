using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace Programmation.Infrastructure.Data
{
    public class ProgrammationDbContextFactory : IDesignTimeDbContextFactory<ProgrammationDbContext>
    {
        public ProgrammationDbContext CreateDbContext(string[] args)
        {
            // 1) Charger automatiquement le fichier .env
            Env.Load();

            // 2) Récupérer la variable d'environnement ConnectionStrings_ProjetConnection
            var conn = Environment.GetEnvironmentVariable("ProjetConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ProjetConnection n'est pas défini. Vérifiez votre .env.");

            // 3) Construire les options EF Core avec UseOracle
            var optionsBuilder = new DbContextOptionsBuilder<ProgrammationDbContext>();
            optionsBuilder.UseOracle(conn);

            return new ProgrammationDbContext(optionsBuilder.Options);
        }
    }
}
