using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.Persistence
{
    public class ProgrammeService: IProgrammeService
    {
         private readonly SharedDbContext _dbContext;
        private readonly ILogger<ProgrammeService> _logger;

        public ProgrammeService(
            SharedDbContext dbContext,
            ILogger<ProgrammeService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(ProgrammeDto programme)
        {
            var json = JsonConvert.SerializeObject(programme);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_PROGRAMME_ET_SOUS_PROGRAMMES_JSON : {Json}",
                json);

            var param = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_PROGRAMME_ET_SOUS_PROGRAMMES_JSON(:p_json); END;",
                param);

            _logger.LogInformation(
                "Procédure AJOUTER_PROGRAMME_ET_SOUS_PROGRAMMES_JSON exécutée.");
        }

        public async Task<List<ProgrammeDto>> ObtenirTousAsync()
        {
            var progs = await _dbContext.ViewProgrammePlats.ToListAsync();
            var sous = await _dbContext.ViewSousProgrammePlats.ToListAsync();

            var result = progs.Select(p => new ProgrammeDto
            {
                Idprogramme = p.Idprogramme,
                Nomprogramme = p.Nomprogramme,
                ListSousProgramme = sous
                    .Where(s => s.Idprogramme == p.Idprogramme)
                    .Select(s => new SousProgrammeDto
                    {
                        Idsousprogramme = s.Idsousprogramme,
                        Nomsousprogramme = s.Nomsousprogramme
                    })
                    .ToList()
            }).ToList();

            return result;
        }

        public async Task<ProgrammeDto?> ObtenirParIdAsync(int id)
        {
            var p = await _dbContext.ViewProgrammePlats
                .FirstOrDefaultAsync(x => x.Idprogramme == id);
            if (p == null) return null;

            var sous = await _dbContext.ViewSousProgrammePlats
                .Where(s => s.Idprogramme == p.Idprogramme)
                .ToListAsync();

            return new ProgrammeDto
            {
                Idprogramme = p.Idprogramme,
                Nomprogramme = p.Nomprogramme,
                ListSousProgramme = sous
                    .Select(s => new SousProgrammeDto
                    {
                        Idsousprogramme = s.Idsousprogramme,
                        Nomsousprogramme = s.Nomsousprogramme
                    })
                    .ToList()
            };
        }

        public async Task SupprimerAsync(int Idprogramme)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = Idprogramme};

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM PROGRAMME_O WHERE ID_PROGRAMME = :p_id",
                param
            );
        }

        public async Task MettreAJourAsync(ProgrammeDto programme)
        {
            var json = JsonConvert.SerializeObject(programme);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_PROGRAMME_ET_SOUS_PROGRAMMES_JSON(:p_json); END;",
                param
            );
        }
    }
}
