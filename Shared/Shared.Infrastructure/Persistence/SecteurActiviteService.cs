using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Persistence
{
    public class SecteurActiviteService : ISecteurActiviteService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<SecteurActiviteService> _logger;

        public SecteurActiviteService(
            SharedDbContext dbContext,
            ILogger<SecteurActiviteService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(SecteurActiviteDto secteur)
        {
            // 1) Nettoyage / filtrage
            var listeOrigine = secteur.ListSousSecteurActivite;
            var listeFiltree = (listeOrigine ?? new List<SousSecteurActiviteDto>())
                // on enlève les null et ceux sans nom
                .Where(ss => ss != null && !string.IsNullOrWhiteSpace(ss.NomSousSecteurActivite))
                .ToList();

            // réaffectation pour sérialisation
            secteur.ListSousSecteurActivite = listeFiltree;

            // 2) Logging
            _logger.LogInformation("DTO count — Sous‐secteurs: {Count}", listeFiltree.Count);

            // 3) Propagation de l'ID parent
            foreach (var ss in listeFiltree)
            {
                ss.IdSecteurActivite = secteur.IdSecteurActivite;
            }

            // 4) Sérialisation & appel PL/SQL
            var json = JsonConvert.SerializeObject(secteur);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON : {Json}",
                json);

            var p_json = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_SECTEUR_ACTIVITE_ET_SOUS_SECTEURS_JSON(:p_json); END;",
                p_json);

            _logger.LogInformation("Procédure exécutée avec succès.");
        }


        public async Task MettreAJourAsync(SecteurActiviteDto secteur)
        {
            // Same proc is idempotent => reuse AjouterAsync
            await AjouterAsync(secteur);
        }

        public async Task<List<SecteurActiviteDto>> ObtenirTousAsync()
        {
            var secteurs = await _dbContext.ViewSecteurActivitePlats
                .AsNoTracking()
                .ToListAsync();
            var sousSecteurs = await _dbContext.ViewSousSecteurActivitePlats
                .AsNoTracking()
                .ToListAsync();

            var result = new List<SecteurActiviteDto>(secteurs.Count);
            foreach (var s in secteurs)
            {
                var listSs = sousSecteurs
                    .Where(ss => ss.Idsecteur == s.Idsecteur)
                    .Select(ss => new SousSecteurActiviteDto
                    {
                        IdSousSecteurActivite = ss.Idsoussecteur,
                        NomSousSecteurActivite = ss.Nomsoussecteur,
                        IdSecteurActivite = ss.Idsecteur
                    })
                    .ToList();

                result.Add(new SecteurActiviteDto
                {
                    IdSecteurActivite = s.Idsecteur,
                    NomSecteurActivite = s.Nomsecteur,
                    ListSousSecteurActivite = listSs
                });
            }

            return result;
        }

        public async Task<SecteurActiviteDto?> ObtenirParIdAsync(int id)
        {
            var s = await _dbContext.ViewSecteurActivitePlats
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Idsecteur == id);
            if (s == null) return null;

            var sous = await _dbContext.ViewSousSecteurActivitePlats
                .AsNoTracking()
                .Where(ss => ss.Idsecteur == id)
                .ToListAsync();

            return new SecteurActiviteDto
            {
                IdSecteurActivite = s.Idsecteur,
                NomSecteurActivite = s.Nomsecteur,
                ListSousSecteurActivite = sous
                    .Select(ss => new SousSecteurActiviteDto
                    {
                        IdSousSecteurActivite = ss.Idsoussecteur,
                        NomSousSecteurActivite = ss.Nomsoussecteur,
                        IdSecteurActivite = ss.Idsecteur
                    })
                    .ToList()
            };
        }

        public async Task SupprimerAsync(int idSecteur)
        {
            // Delete children then parent
            var pId = new OracleParameter("p_id", OracleDbType.Int32)
            {
                Value = idSecteur
            };

            var plsql = @"
BEGIN
  DELETE FROM SOUS_SECTEUR_ACTIVITE_O
   WHERE refSECTEUR_ACTIVITE = (
     SELECT REF(s)
       FROM SECTEUR_ACTIVITE_O s
      WHERE s.ID_SECTEUR_ACTIVITE = :p_id
   );
  DELETE FROM SECTEUR_ACTIVITE_O
   WHERE ID_SECTEUR_ACTIVITE = :p_id;
  COMMIT;
END;";

            await _dbContext.Database.ExecuteSqlRawAsync(plsql, pId);
            _logger.LogInformation(
                "Secteur {Id} et ses sous-secteurs supprimés.", idSecteur);
        }
    }
}
