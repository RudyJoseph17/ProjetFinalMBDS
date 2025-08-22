using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class IdentificationProjetService : IIdentificationProjetService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<IdentificationProjetService> _logger;

        public IdentificationProjetService(BanquePDbContext dbContext, ILogger<IdentificationProjetService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AjouterAsync(IdentificationProjetDto projet)
        {
            if (string.IsNullOrWhiteSpace(projet.IdIdentificationProjet))
            {
                projet.IdIdentificationProjet = IdGenerator.GenererIdPour("IdIdentificationProjet");
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()  // conserve strictement la casse C#
                },
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(projet, settings);

            Console.WriteLine("JSON envoyé à Oracle :");
            Console.WriteLine(json);  // ou ILogger.LogDebug(json)

            await ExecuteProcedureAsync("AJOUTER_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(IdentificationProjetDto projet)
        {
            var json = JsonConvert.SerializeObject(projet, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            await ExecuteProcedureAsync("MAJ_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(string ID_IDENTIFICATION_PROJET)
        {
            var wrapper = new { ID_IDENTIFICATION_PROJET = ID_IDENTIFICATION_PROJET };
            var json = JsonConvert.SerializeObject(wrapper);
            await ExecuteProcedureAsync("SUPPRIMER_PROJET_ET_LISTES_JSON", json);
        }

        private async Task ExecuteProcedureAsync(string procedureName, string json)
        {
            _logger.LogInformation("📦 JSON envoyé à {Procedure} : {Json}", procedureName, json);

            await using var conn = _dbContext.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();

            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.CreateParameter();
            param.ParameterName = "p_json";
            param.DbType = DbType.String;
            param.Value = json;
            cmd.Parameters.Add(param);

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<IdentificationProjetDto>> ObtenirTousAsync()
        {
            var rows = await _dbContext.ViewIdentificationProjetPlats
                .AsNoTracking()
                .ToListAsync();

            var projets = rows
                .GroupBy(r => r.IdIdentificationProjet)
                .Select(g => new IdentificationProjetDto
                {
                    IdIdentificationProjet = g.Key,
                    NomProjet = g.First().NomProjet,
                    Ministere = g.First().Ministere,
                    Section = g.First().Section,
                    CodePip = g.First().CodePip,
                    CodeBailleur = g.First().CodeBailleur,
                    JustificationProjet = g.First().JustificationProjet,
                    EtudePrefaisabilite = g.First().EtudePrefaisabilite,
                    EtudeFaisabilite = g.First().EtudeFaisabilite,
                    PopulationVisee = g.First().PopulationVisee,
                    Programme = g.First().Programme,
                    SousProgramme = g.First().SousProgramme,
                    DateInscription = g.First().DateInscription,
                    DateMiseAJour = g.First().DateMiseAJour,
                })
                .ToList();

            return projets;
        }

        public async Task<IdentificationProjetDto?> ObtenirParIdAsync(string id)
        {
            var rows = await _dbContext.ViewIdentificationProjetPlats
                .Where(r => r.IdIdentificationProjet == id)
                .AsNoTracking()
                .ToListAsync();

            if (!rows.Any())
                return null;

            var g = rows;
            var first = g.First();

            return new IdentificationProjetDto
            {
                IdIdentificationProjet = first.IdIdentificationProjet,
                NomProjet = first.NomProjet,
                Ministere = first.Ministere,
                Section = first.Section,
                CodePip = first.CodePip,
                CodeBailleur = first.CodeBailleur,
                JustificationProjet = first.JustificationProjet,
                EtudePrefaisabilite = first.EtudePrefaisabilite,
                EtudeFaisabilite = first.EtudeFaisabilite,
                PopulationVisee = first.PopulationVisee,
                Programme = first.Programme,
                SousProgramme = first.SousProgramme,
                DateInscription = first.DateInscription,
                DateMiseAJour = first.DateMiseAJour
            };
        }
    }
}
