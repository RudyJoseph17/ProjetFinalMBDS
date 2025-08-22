using Microsoft.EntityFrameworkCore;
using Shared.Domain.Helpers;

namespace Shared.Infrastructure.Persistence
{
    /// <summary>
    /// Classe de base pour générer des IDs via séquences Oracle et partager la logique d'accès.
    /// </summary>
    public abstract class BaseService
    {
        public readonly DbContext _dbContext;

        public BaseService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Récupère la prochaine valeur de la séquence Oracle spécifiée.
        /// </summary>
        /// <param name="sequenceName">Nom de la séquence (ex: SEQ_PROJET)</param>
        /// <returns>Valeur entière suivante de la séquence</returns>
        public async Task<int> NextValAsync(string sequenceName)
        {
            // Récupérer l'objet DbConnection via l'extension
            var conn = _dbContext.Database.GetDbConnection();

            // Ouvrir si nécessaire
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            // Créer et exécuter la commande
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

    }
}

