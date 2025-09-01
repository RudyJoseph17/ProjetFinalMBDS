using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Shared.Infrastructure.Persistence
{
    public class BailleurDeFondsService : IBailleurDeFondsService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<BailleurDeFondsService> _logger;

        public BailleurDeFondsService(SharedDbContext dbContext, ILogger<BailleurDeFondsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Appelle la procédure PL/SQL AJOUTER_BAILLEUR_DE_FONDS_JSON
        /// </summary>
        public async Task AjouterAsync(BailleurDeFondsDto bailleur)
        {
            var json = JsonConvert.SerializeObject(bailleur);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_BAILLEUR_DE_FONDS_JSON : {Json}", json);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };
            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_BAILLEUR_DE_FONDS_JSON(:p_json); END;",
                param
            );
            _logger.LogInformation("Procédure AJOUTER_BAILLEUR_DE_FONDS_JSON exécutée avec succès.");
        }

        /// <summary>
        /// Pour la mise à jour, on repasse aussi par la procédure JSON.
        /// </summary>
        public async Task MettreAJourAsync(BailleurDeFondsDto bailleur)
        {
            var json = JsonConvert.SerializeObject(bailleur);
    var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_BAILLEUR_DE_FONDS_JSON(:p_json); END;",
                param
            );
        }

        /// <summary>
        /// Suppression via procédure ou simple DELETE
        /// </summary>
        public async Task SupprimerAsync(int idBailleur)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = idBailleur };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM BAILLEURS_DE_FONDS_O WHERE ID_BAILLEURS_DE_FONDS = :p_id",
                param
            );
        }

        /// <summary>
        /// Lecture via la vue
        /// </summary>
        public async Task<List<BailleurDeFondsDto>> ObtenirTousAsync()
        {
            return await _dbContext.ViewBailleursDeFondsPlats
                .Select(v => new BailleurDeFondsDto
                {
                    IdBailleur = v.IdBailleur,
                    NomBailleur = v.NomBailleur,
                    TypeBailleur = v.TypeBailleur
                })
                .ToListAsync();
        }

        public async Task<BailleurDeFondsDto?> ObtenirParIdAsync(int id)
        {
            var entity = await _dbContext.ViewBailleursDeFondsPlats
                .FirstOrDefaultAsync(v => v.IdBailleur == id);

            if (entity == null) return null;

            return new BailleurDeFondsDto
            {
                IdBailleur = entity.IdBailleur,
                NomBailleur = entity.NomBailleur,
                TypeBailleur = entity.TypeBailleur
            };
        }
    }
}
