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
    public class DepartementService : IDepartementService
    {
        private readonly SharedDbContext _db;
        private readonly ILogger<DepartementService> _logger;

        public DepartementService(SharedDbContext db, ILogger<DepartementService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AjouterAsync(DepartementDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            _logger.LogInformation("📦 JSON envoyé à AJOUTER_DEPARTEMENT_ET_LISTES_JSON : {Json}", json);

            var p = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };
            await _db.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_DEPARTEMENT_ET_LISTES_JSON(:p_json); END;",
                p
            );

            _logger.LogInformation("Procédure AJOUTER_DEPARTEMENT_ET_LISTES_JSON exécutée.");
        }

        public async Task<List<DepartementDto>> ObtenirTousAsync()
        {
            // Charger tous les départements plats
            var deps = await _db.ViewDepartementPlats.ToListAsync();
            var arrs = await _db.ViewArrondissementPlats.ToListAsync();
            var comms = await _db.ViewCommunePlats.ToListAsync();
            var secs = await _db.ViewSectionCommunalePlats.ToListAsync();

            var result = new List<DepartementDto>();

            foreach (var d in deps)
            {
                var dto = new DepartementDto
                {
                    IdDepartement = d.Iddepartement,
                    NomDepartement = d.Nomdepartement
                };

                var listArr = arrs
                    .Where(a => a.Iddepartement == d.Iddepartement)
                    .Select(a => new ArrondissementDto
                    {
                        IdArrondissement = a.Idarrondissement,
                        NomArrondissement = a.Nomarrondissement
                    }).ToList();

                foreach (var ar in listArr)
                {
                    var listComm = comms
                        .Where(c => c.Idarrondissement == ar.IdArrondissement)
                        .Select(c => new CommuneDto
                        {
                            IdCommune = c.Idcommune,
                            NomCommune = c.Nomcommune
                        }).ToList();

                    foreach (var co in listComm)
                    {
                        co.ListSections = secs
                            .Where(s => s.Idcommune == co.IdCommune)
                            .Select(s => new SectionCommunaleDto
                            {
                                IdSectionCommunale = s.Idsectioncommunale,
                                NomSectionCommunale = s.Nomsectioncommunale
                            }).ToList();
                    }

                    ar.ListCommunes = listComm;
                }

                dto.ListArrondissements = listArr;
                result.Add(dto);
            }

            return result;
        }

        public async Task<DepartementDto?> ObtenirParIdAsync(int IdDepartement)
        {
            var d = await _db.ViewDepartementPlats
                .FirstOrDefaultAsync(x => x.Iddepartement == IdDepartement);
            if (d == null) return null;

            var arrs = await _db.ViewArrondissementPlats
                .Where(a => a.Iddepartement == d.Iddepartement)
                .ToListAsync();

            var comms = await _db.ViewCommunePlats
                .ToListAsync();

            var secs = await _db.ViewSectionCommunalePlats
                .ToListAsync();

            var dto = new DepartementDto
            {
                IdDepartement = d.Iddepartement,
                NomDepartement = d.Nomdepartement
            };

            dto.ListArrondissements = arrs.Select(a =>
            {
                var arDto = new ArrondissementDto
                {
                    IdArrondissement = a.Idarrondissement,
                    NomArrondissement = a.Nomarrondissement
                };

                arDto.ListCommunes = comms
                    .Where(c => c.Idarrondissement == a.Idarrondissement)
                    .Select(c =>
                    {
                        var coDto = new CommuneDto
                        {
                            IdCommune = c.Idcommune,
                            NomCommune = c.Nomcommune
                        };
                        coDto.ListSections = secs
                            .Where(s => s.Idcommune == c.Idcommune)
                            .Select(s => new SectionCommunaleDto
                            {
                                IdSectionCommunale = s.Idsectioncommunale,
                                NomSectionCommunale = s.Nomsectioncommunale
                            }).ToList();
                        return coDto;
                    }).ToList();

                return arDto;
            }).ToList();

            return dto;
        }

        public async Task SupprimerAsync(int IdDepartement)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdDepartement };

            await _db.Database.ExecuteSqlRawAsync(
                "DELETE FROM DEPARTEMENT_O WHERE ID_DEPARTEMENT = :p_id",
                param
            );
        }

        public async Task MettreAJourAsync(DepartementDto departement)
        {
            var json = JsonConvert.SerializeObject(departement);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_DEPARTEMENT_ET_LISTES_JSON(:p_json); END;",
                param
            );
        }
    }
}
