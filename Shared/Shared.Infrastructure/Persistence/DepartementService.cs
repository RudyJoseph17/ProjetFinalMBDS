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
    public class DepartementService : IDepartementService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<DepartementService> _logger;

        public DepartementService(
            SharedDbContext dbContext,
            ILogger<DepartementService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(DepartementDto departement)
        {
            // comptage des niveaux pour le log
            var arrCount = departement.ListArrondissements?.Count ?? 0;
            var comCount = departement.ListArrondissements?
                .Sum(a => a.ListCommunes?.Count ?? 0) ?? 0;
            var secCount = departement.ListArrondissements?
                .Sum(a => a.ListCommunes?
                    .Sum(c => c.ListSections?.Count ?? 0) ?? 0) ?? 0;

            _logger.LogInformation(
                "DTO counts — Arrondissements: {A}, Communes: {C}, Sections: {S}",
                arrCount, comCount, secCount);

            foreach (var arr in departement.ListArrondissements)
            {
                arr.ParentDepartement = departement.IdDepartement;

                foreach (var com in arr.ListCommunes)
                {
                    com.ParentArr = arr.IdArrondissement;

                    foreach (var sec in com.ListSections)
                    {
                        sec.ParentCommune = com.IdCommune;
                    }
                }
            }


            // sérialisation JSON
            var json = JsonConvert.SerializeObject(departement);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_DEPARTEMENT_ET_LISTES_JSON : {Json}",
                json);

            // appel de la procédure Oracle
            var pJson = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_DEPARTEMENT_ET_LISTES_JSON(:p_json); END;",
                pJson);

            _logger.LogInformation(
                "Procédure AJOUTER_DEPARTEMENT_ET_LISTES_JSON exécutée.");
        }

        public async Task MettreAJourAsync(DepartementDto departement)
        {
            var json = JsonConvert.SerializeObject(departement);
            var pJson = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_DEPARTEMENT_ET_LISTES_JSON(:p_json); END;",
                pJson);

            _logger.LogInformation(
                "Procédure AJOUTER_DEPARTEMENT_ET_LISTES_JSON exécutée pour mise à jour.");
        }

        public async Task<List<DepartementDto>> ObtenirTousAsync()
        {
            var deps = await _dbContext.ViewDepartementPlats.ToListAsync();
            var result = new List<DepartementDto>();

            foreach (var d in deps)
            {
                // arrondissements du département
                var arrs = await _dbContext.ViewArrondissementPlats
                    .Where(a => a.Iddepartement == d.Iddepartement)
                    .ToListAsync();

                var listArr = new List<ArrondissementDto>();

                foreach (var a in arrs)
                {
                    // communes de l'arrondissement
                    var comms = await _dbContext.ViewCommunePlats
                        .Where(c => c.Idarrondissement == a.Idarrondissement)
                        .ToListAsync();

                    var listComm = new List<CommuneDto>();

                    foreach (var c in comms)
                    {
                        // sections communales de la commune
                        var secs = await _dbContext.ViewSectionCommunalePlats
                            .Where(s => s.Idcommune == c.Idcommune)
                            .ToListAsync();

                        listComm.Add(new CommuneDto
                        {
                            IdCommune = c.Idcommune,
                            NomCommune = c.Nomcommune,
                            ListSections = secs
                                .Select(s => new SectionCommunaleDto
                                {
                                    IdSectionCommunale = s.Idsectioncommunale,
                                    NomSectionCommunale = s.Nomsectioncommunale
                                })
                                .ToList()
                        });
                    }

                    listArr.Add(new ArrondissementDto
                    {
                        IdArrondissement = a.Idarrondissement,
                        NomArrondissement = a.Nomarrondissement,
                        ListCommunes = listComm
                    });
                }

                result.Add(new DepartementDto
                {
                    IdDepartement = d.Iddepartement,
                    NomDepartement = d.Nomdepartement,
                    ListArrondissements = listArr
                });
            }

            return result;
        }

        public async Task<DepartementDto?> ObtenirParIdAsync(int id)
        {
            var d = await _dbContext.ViewDepartementPlats
                .FirstOrDefaultAsync(x => x.Iddepartement == id);

            if (d == null)
                return null;

            var dto = new DepartementDto
            {
                IdDepartement = d.Iddepartement,
                NomDepartement = d.Nomdepartement,
                ListArrondissements = new List<ArrondissementDto>()
            };

            var arrs = await _dbContext.ViewArrondissementPlats
                .Where(a => a.Iddepartement == id)
                .ToListAsync();

            foreach (var a in arrs)
            {
                var comms = await _dbContext.ViewCommunePlats
                    .Where(c => c.Idarrondissement == a.Idarrondissement)
                    .ToListAsync();

                var listComm = new List<CommuneDto>();

                foreach (var c in comms)
                {
                    var secs = await _dbContext.ViewSectionCommunalePlats
                        .Where(s => s.Idcommune == c.Idcommune)
                        .ToListAsync();

                    listComm.Add(new CommuneDto
                    {
                        IdCommune = c.Idcommune,
                        NomCommune = c.Nomcommune,
                        ListSections = secs
                            .Select(s => new SectionCommunaleDto
                            {
                                IdSectionCommunale = s.Idsectioncommunale,
                                NomSectionCommunale = s.Nomsectioncommunale
                            })
                            .ToList()
                    });
                }

                dto.ListArrondissements.Add(new ArrondissementDto
                {
                    IdArrondissement = a.Idarrondissement,
                    NomArrondissement = a.Nomarrondissement,
                    ListCommunes = listComm
                });
            }

            return dto;
        }

        public async Task SupprimerAsync(int idDepartement)
        {
            var pId = new OracleParameter("p_id", OracleDbType.Int32)
            {
                Value = idDepartement
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM DEPARTEMENT_O WHERE ID_DEPARTEMENT = :p_id",
                pId);

            _logger.LogInformation(
                "Département {Id} supprimé de DEPARTEMENT_O", idDepartement);
        }
    }
}
