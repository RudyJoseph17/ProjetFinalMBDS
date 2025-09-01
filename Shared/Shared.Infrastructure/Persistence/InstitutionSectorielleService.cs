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
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.Persistence
{
    public class InstitutionSectorielleService : IInstitutionSectorielleService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<IInstitutionSectorielleService> _logger;

        public InstitutionSectorielleService(
            SharedDbContext dbContext,
            ILogger<InstitutionSectorielleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(InstitutionSectorielleDto institutionSectorielle)
        {
            var json = JsonConvert.SerializeObject(institutionSectorielle);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_INSTITUTION_SECTORIELLE_ET_LISTES_JSON : {Json}",
                json);

            var param = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                " BEGIN AJOUTER_INSTITUTION_SECTORIELLE_ET_LISTES_JSON(:p_json); END;",
                param);

            _logger.LogInformation(
                "Procédure AJOUTER_INSTITUTION_SECTORIELLE_ET_LISTES_JSON exécutée.");
        }

        public async Task MettreAJourAsync(InstitutionSectorielleDto institutionSectorielle)
        {
            var json = JsonConvert.SerializeObject(institutionSectorielle);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_INSTITUTION_SECTORIELLE_ET_LISTES_JSON(:p_json); END;",
                param
            );
        }

        public async Task<List<InstitutionSectorielleDto>> ObtenirTousAsync()
        {
            var insts = await _dbContext.ViewInstitutionSectoriellePlats
                .ToListAsync();

            var result = new List<InstitutionSectorielleDto>();

            foreach (var inst in insts)
            {
                var attributions = await _dbContext
                    .ViewAttributionsInstitutionPlats
                    .Where(a => a.Idinstitution == inst.Idinstitution)
                    .ToListAsync();

                var sections = await _dbContext
                    .ViewSectionInstitutionPlats
                    .Where(s => s.Idinstitution == inst.Idinstitution)
                    .ToListAsync();

                result.Add(new InstitutionSectorielleDto
                {
                    Idinstitution = inst.Idinstitution,
                    Nominstitution = inst.Nominstitution,
                    Sigleinstitution = inst.Sigleinstitution,
                    Missioninstitution = inst.Missioninstitution,
                    ListAttributions = attributions
                        .Select(a => new AttributionsInstitutionDto
                        {
                            Idattribution = a.Idattribution,
                            DescriptionAttribution = a.Descriptionattribution
                        })
                        .ToList(),
                    ListSections = sections
                        .Select(s => new SectionInstitutionDto
                        {
                            IdSection = s.Idsection,
                            NomSection = s.Nomsection,
                            SigleSection = s.Siglesection,
                            AdresseSection = s.Adressesection
                        })
                        .ToList()
                });
            }

            return result;
        }

        public async Task SupprimerAsync(int Idinstitution)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = Idinstitution };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM INSTITUTION_SECTORIELLE_O WHERE ID_INSTITUTION_SECTORIELLE = :p_id",
                param
            );
        }

        public async Task<InstitutionSectorielleDto?> ObtenirParIdAsync(int id)
        {
            var inst = await _dbContext.ViewInstitutionSectoriellePlats
                .FirstOrDefaultAsync(i => i.Idinstitution == id);

            if (inst == null)
                return null;

            var attributions = await _dbContext
                .ViewAttributionsInstitutionPlats
                .Where(a => a.Idinstitution == id)
                .ToListAsync();

            var sections = await _dbContext
                .ViewSectionInstitutionPlats
                .Where(s => s.Idinstitution == id)
                .ToListAsync();

            return new InstitutionSectorielleDto
            {
                Idinstitution = inst.Idinstitution,
                Nominstitution = inst.Nominstitution,
                Sigleinstitution = inst.Sigleinstitution,
                Missioninstitution = inst.Missioninstitution,
                ListAttributions = attributions
                    .Select(a => new AttributionsInstitutionDto
                    {
                        Idattribution = a.Idattribution,
                        DescriptionAttribution = a.Descriptionattribution
                    })
                    .ToList(),
                ListSections = sections
                    .Select(s => new SectionInstitutionDto
                    {
                        IdSection = s.Idsection,
                        NomSection = s.Nomsection,
                        SigleSection = s.Siglesection,
                        AdresseSection = s.Adressesection
                    })
                    .ToList()
            };
        }
    }
}

