using DotNetEnv;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Infrastructure.Data
{
    public class EvaluationDbContextFactory : IDesignTimeDbContextFactory<EvaluationDbContext>
    {
        public EvaluationDbContext CreateDbContext(string[] args)
        {
            // 1) Charger automatiquement le fichier .env
            Env.Load();

            // 2) Récupérer la variable d'environnement ConnectionStrings_ProjetConnection
            var conn = Environment.GetEnvironmentVariable("ProjetConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ConnectionStrings_ProjetConnection n'est pas défini. Vérifiez votre .env.");

            // 3) Construire les options EF Core avec UseOracle
            var optionsBuilder = new DbContextOptionsBuilder<EvaluationDbContext>();
            optionsBuilder.UseOracle(conn);

            return new EvaluationDbContext(optionsBuilder.Options);
        }
    }
}
