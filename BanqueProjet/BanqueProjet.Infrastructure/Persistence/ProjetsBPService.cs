using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data;
using Microsoft.CodeAnalysis;
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
    public class ProjetsBPService : IProjetsBPService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly ILogger<ProjetsBPService> _logger;
        private readonly JsonSerializerSettings _jsonSettings;

        public ProjetsBPService(BanquePDbContext dbContext, ILogger<ProjetsBPService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;


            // 🔑 Ici on impose snake_case pour TOUTES les clés JSON envoyées à Oracle
            //_jsonSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new DefaultContractResolver
            //    {
            //        NamingStrategy = new SnakeCaseNamingStrategy
            //        {
            //            ProcessDictionaryKeys = true,
            //            OverrideSpecifiedNames = false
            //        }
            //    },
            //    NullValueHandling = NullValueHandling.Include
            //};
        }

        public async Task AjouterAsync(ProjetsBPDto projetsBPD)
        {
            if (string.IsNullOrWhiteSpace(projetsBPD.IdIdentificationProjet))
            {
                projetsBPD.IdIdentificationProjet = IdGenerator.GenererIdPour(projetsBPD.IdIdentificationProjet);
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()  // conserve strictement la casse C#
                },
                NullValueHandling = NullValueHandling.Ignore,

                // 👉 Force toutes les dates au format YYYY-MM-DD
                DateFormatString = "yyyy-MM-dd"
            };

            var json = JsonConvert.SerializeObject(projetsBPD, settings);

            Console.WriteLine("JSON envoyé à Oracle :");
            Console.WriteLine(json);  // ou ILogger.LogDebug(json)

            _logger.LogInformation("=== JSON envoyé à Oracle (Ajouter) ===\n{Json}", json);

            await ExecuteProcedureAsync("AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(ProjetsBPDto projetsBPD)
        {
            var json = JsonConvert.SerializeObject(projetsBPD, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            _logger.LogInformation("=== JSON envoyé à Oracle (Mise à jour) ===\n{Json}", json);

            await ExecuteProcedureAsync("MAJ_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(string idIdentificationProjet)
        {
            var wrapper = new { ID_IDENTIFICATION_PROJET = idIdentificationProjet };

            var json = JsonConvert.SerializeObject(wrapper, _jsonSettings);

            _logger.LogInformation("=== JSON envoyé à Oracle (Suppression) ===\n{Json}", json);

            await ExecuteProcedureAsync("SUPPRIMER_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        private async Task ExecuteProcedureAsync(string procedureName, string json)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur executing {Procedure}. json (truncated): {Json}",
                    procedureName, json.Substring(0, Math.Min(json.Length, 500)));
                throw;
            }
        }

        public async Task<List<ProjetsBPDto>> ObtenirTousAsync()
        {
            var rows = await _dbContext.ViewIdentificationProjetPlats
                .AsNoTracking()
                .ToListAsync();

            return rows
                .GroupBy(r => r.IdIdentificationProjet)
                .Select(g => new ProjetsBPDto
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
        }

        public async Task<ProjetsBPDto?> ObtenirParIdAsync(string id)
        {
            var rows = await _dbContext.ViewIdentificationProjetPlats
                .Where(r => r.IdIdentificationProjet == id)
                .AsNoTracking()
                .ToListAsync();

            if (!rows.Any())
                return null;

            var first = rows.First();

            return new ProjetsBPDto
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
