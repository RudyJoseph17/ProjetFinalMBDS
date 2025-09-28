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
    // 1) Génération de l’ID si nécessaire
    if (string.IsNullOrWhiteSpace(projetsBPD.IdIdentificationProjet))
    {
        projetsBPD.IdIdentificationProjet =
            IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
    }

    // 2) Préparation de la sérialisation JSON
    var settings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new DefaultNamingStrategy()
        },
        NullValueHandling = NullValueHandling.Ignore,
        DateFormatString = "yyyy-MM-dd"
    };

    string json   = JsonConvert.SerializeObject(projetsBPD, settings);
    int    status;
    string report;

    // 3) Boucle de retry tant que la BDD signale un doublon
    do
    {
        (status, report) = await ExecuteProcedureWithStatusAsync(
            "AJOUTER_IDENTIFICATION_PROJET_JSON",
            json);

        if (status == 1)
        {
            // doublon détecté : régénération de l’ID et nouvelle sérialisation
            _logger.LogWarning(
                "Doublon détecté pour ID={Id}, régénération et nouvel essai",
                projetsBPD.IdIdentificationProjet);

            projetsBPD.IdIdentificationProjet =
                IdGenerator.GenererIdPour(nameof(ProjetsBPDto.IdIdentificationProjet));
            json = JsonConvert.SerializeObject(projetsBPD, settings);
        }
        else if (status < 0)
        {
            // erreur fatale remontée avec le report
            throw new InvalidOperationException(
                $"Erreur Oracle lors de l’insertion : {report}");
        }

    } while (status == 1);

    _logger.LogInformation("Projet ajouté avec succès. Report Oracle :\n{Report}", report);
}


// Méthode utilitaire retournant (status, report) depuis la proc PL/SQL



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
            await using var conn = _dbContext.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            // 1) p_action IN VARCHAR2
            var pAction = cmd.CreateParameter();
            pAction.ParameterName = "p_action";
            pAction.DbType = DbType.String;
            pAction.Value = "INSERT";         // ou "UPDATE" selon le contexte
            cmd.Parameters.Add(pAction);

            // 2) p_json IN CLOB
            var pJson = cmd.CreateParameter();
            pJson.ParameterName = "p_json";
            pJson.DbType = DbType.String;      // ou DbType.AnsiString
            pJson.Value = json;
            cmd.Parameters.Add(pJson);

            // 3) p_report OUT CLOB
            var pReport = cmd.CreateParameter();
            pReport.ParameterName = "p_report";
            pReport.DbType = DbType.String;    // ODP.NET détecte automatiquement le CLOB
            pReport.Direction = ParameterDirection.Output;
            // taille arbitraire pour laisser sortir un long CLOB
            pReport.Size = 32768;
            cmd.Parameters.Add(pReport);

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            // Optionnel : récupérer et logger le rapport Oracle
            var reportText = pReport.Value as string;
            _logger.LogDebug("Report from Oracle: {Report}", reportText);
        }

        private async Task<(int Status, string Report)> ExecuteProcedureWithStatusAsync(
    string procedureName,
    string json)
        {
            // 1) Récupère la connexion gérée par EF
            var conn = _dbContext.Database.GetDbConnection();
            var mustClose = conn.State != ConnectionState.Open;

            // 2) Ouvre si nécessaire
            if (mustClose)
                await conn.OpenAsync();

            try
            {
                // 3) Crée et configure le DbCommand
                using var cmd = conn.CreateCommand();
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // p_action IN
                var pAction = cmd.CreateParameter();
                pAction.ParameterName = "p_action";
                pAction.DbType = DbType.String;
                pAction.Direction = ParameterDirection.Input;
                pAction.Value = "INSERT";
                cmd.Parameters.Add(pAction);

                // p_json IN
                var pJson = cmd.CreateParameter();
                pJson.ParameterName = "p_json";
                pJson.DbType = DbType.String;
                pJson.Direction = ParameterDirection.Input;
                pJson.Value = json;
                cmd.Parameters.Add(pJson);

                // p_report OUT
                var pReport = cmd.CreateParameter();
                pReport.ParameterName = "p_report";
                pReport.DbType = DbType.String;
                pReport.Direction = ParameterDirection.Output;
                pReport.Size = 32768;
                cmd.Parameters.Add(pReport);

                // p_status OUT
                var pStatus = cmd.CreateParameter();
                pStatus.ParameterName = "p_status";
                pStatus.DbType = DbType.Int32;
                pStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pStatus);

                // 4) Exécute la proc
                await cmd.ExecuteNonQueryAsync();

                // 5) Récupère les valeurs
                var status = Convert.ToInt32(pStatus.Value);
                var report = pReport.Value as string ?? string.Empty;
                return (status, report);
            }
            finally
            {
                // 6) Ferme la connexion si on l'avait ouverte
                if (mustClose)
                    await conn.CloseAsync();
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

        public async Task<List<ProjetsBPDto>> GetProjetsSommaireAsync()
        {
            // réutilise la logique existante
            return await ObtenirTousAsync();
        }
    }
}
