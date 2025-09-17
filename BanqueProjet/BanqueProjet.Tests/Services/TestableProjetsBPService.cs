using BanqueProjet.Application.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Shared.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanqueProjet.Application.Interfaces;

namespace BanqueProjet.Tests.Services
{
    public class TestableProjetsBPService : IProjetsBPService
    {
        private readonly ILogger<TestableProjetsBPService> _logger;

        public string? LastProcedureName { get; private set; }
        public string? LastJsonSent { get; private set; }

        public TestableProjetsBPService(ILogger<TestableProjetsBPService> logger)
        {
            _logger = logger;
        }

        public async Task AjouterAsync(ProjetsBPDto projetsBPD)
        {
            if (string.IsNullOrEmpty(projetsBPD.IdIdentificationProjet))
            {
                projetsBPD.IdIdentificationProjet = IdGenerator.GenererIdPour(nameof(projetsBPD.IdIdentificationProjet));
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(projetsBPD, settings);

            _logger.LogInformation("=== JSON envoyé (testable) ===\n{Json}", json);

            await ExecuteProcedureAsync("AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        public async Task MettreAJourAsync(ProjetsBPDto projetsBPD)
        {
            if (string.IsNullOrEmpty(projetsBPD.IdIdentificationProjet))
            {
                projetsBPD.IdIdentificationProjet = IdGenerator.GenererIdPour(nameof(projetsBPD.IdIdentificationProjet));
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(projetsBPD, settings);
            await ExecuteProcedureAsync("MAJ_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        public async Task SupprimerAsync(string ID_IDENTIFICATION_PROJET)
        {
            var wrapper = new { ID_IDENTIFICATION_PROJET = ID_IDENTIFICATION_PROJET };
            var json = JsonConvert.SerializeObject(wrapper);
            await ExecuteProcedureAsync("SUPPRIMER_IDENTIFICATION_PROJET_ET_LISTES_JSON", json);
        }

        protected virtual Task ExecuteProcedureAsync(string procedureName, string json)
        {
            LastProcedureName = procedureName;
            LastJsonSent = json;
            return Task.CompletedTask;
        }

        public Task<List<ProjetsBPDto>> ObtenirTousAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjetsBPDto?> ObtenirParIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
