using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Persistence
{
    public class SecteurActiviteService: ISecteurActiviteService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<SecteurActiviteService> _logger;

        public SecteurActiviteService(SharedDbContext dbContext, ILogger<SecteurActiviteService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(SecteurActiviteDto secteurActivite)
        {
            var json = JsonConvert.SerializeObject(secteurActivite);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON : {Json}",
                json);

            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON(:p_json); END;",
                param);

            _logger.LogInformation(
                "Procédure AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON exécutée.");
        }

        public async Task<List<SecteurActiviteDto>> ObtenirTousAsync()
        {
            var secteurs = await _dbContext.ViewSecteurActivitePlats.ToListAsync();
            var sousSecteurs = await _dbContext.ViewSousSecteurActivitePlats.ToListAsync();

            return secteurs.Select(s => new SecteurActiviteDto
            {
                IdSecteurActivite = s.Idsecteur,
                NomSecteurActivite = s.Nomsecteur,
                ListSousSecteurActivite = sousSecteurs
                    .Where(ss => ss.Idsecteur == s.Idsecteur)
                    .Select(ss => new SousSecteurActiviteDto
                    {
                        IdSousSecteurActivite = ss.Idsoussecteur,
                        NomSousSecteurActivite = ss.Nomsoussecteur
                    })
                    .ToList()
            }).ToList();
        }

        public async Task<SecteurActiviteDto?> ObtenirParIdAsync(int id)
        {
            var secteur = await _dbContext.ViewSecteurActivitePlats
                .FirstOrDefaultAsync(s => s.Idsecteur == id);
            if (secteur == null) return null;

            var sousSecteurs = await _dbContext.ViewSousSecteurActivitePlats
                .Where(ss => ss.Idsecteur == id)
                .ToListAsync();

            return new SecteurActiviteDto
            {
                IdSecteurActivite = secteur.Idsecteur,
                NomSecteurActivite = secteur.Nomsecteur,
                ListSousSecteurActivite = sousSecteurs
                    .Select(ss => new SousSecteurActiviteDto
                    {
                        IdSousSecteurActivite = ss.Idsoussecteur,
                        NomSousSecteurActivite = ss.Nomsoussecteur
                    })
                    .ToList()
            };
        }

        public async Task SupprimerAsync(int IdSecteurActivite)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdSecteurActivite };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM SECTEUR_ACTIVITE_O WHERE ID_SECTEUR_ACTIVITE = :p_id",
                param
            );
        }

        public async Task MettreAJourAsync(SecteurActiviteDto secteurActivite)
        {
            var json = JsonConvert.SerializeObject(secteurActivite);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON(:p_json); END;",
                param
            );
        }
    }
}
