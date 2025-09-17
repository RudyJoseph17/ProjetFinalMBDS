using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.Dtos;
using Shared.Domain.Helpers;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Persistence
{
    public class IdentificationProjetService : IIdentificationProjetService
    {
        private readonly SharedDbContext _dbContext;
        private readonly ILogger<IdentificationProjetService> _logger;

        public IdentificationProjetService(
            SharedDbContext dbContext,
            ILogger<IdentificationProjetService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Méthode bas-niveau : exécute la procédure PL/SQL en lui passant le JSON fourni (CLOB).
        /// Cette méthode permet aux modules (ex. BanqueProjet) d'envoyer un payload complet incluant
        /// leurs DTOs spécifiques sans que Shared.Infrastructure dépende d'eux.
        /// </summary>
        public async Task AjouterProjetEtListesJsonAsync(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));

            _logger.LogDebug("Envoi JSON brut à la procédure : {Len} chars", json.Length);

            var param = new OracleParameter("p_json", OracleDbType.Clob)
            {
                Direction = ParameterDirection.Input,
                Value = json
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON(:p_json); END;",
                param);
        }

        /// <summary>
        /// Confort : construit un payload JSON pour les parties partagées du DTO (top-level + listes partagées)
        /// et appelle la procédure. Ne gère pas les DTOs spécifiques à BanqueProjet.
        /// </summary>
        public async Task AjouterAsync(IdentificationProjetDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Génération de l'ID côté backend si non fourni
            if (string.IsNullOrWhiteSpace(dto.IdIdentificationProjet))
            {
                dto.IdIdentificationProjet = IdGenerator.GenererIdPour("IdIdentificationProjet");
                _logger.LogDebug("IdIdentificationProjet généré: {Id}", dto.IdIdentificationProjet);
            }

            // Construire le payload en respectant les clés attendues par la procédure (ta procédure d'origine)
            var payload = new
            {
                ID_IDENTIFICATION_PROJET = dto.IdIdentificationProjet,
                NOM_PROJET = dto.NomProjet,
                TYPE_DE_PROJET = dto.TypeDeProjet,
                SECTEUR_D_ACTIVITES = dto.SecteurDActivites,
                SOUS_SECTEUR_D_ACTIVITES = dto.SousSecteurDActivites,
                MINISTERE = dto.Ministere,
                NOM_DIRECTEUR_DE_PROJET = dto.NomDirecteurDeProjet,
                TELEPHONE_DIRECTEUR_DE_PROJET = dto.TelephoneDirecteurDeProjet,
                COURRIEL_DIRECTEUR_DE_PROJET = dto.CourrielDirecteurDeProjet,
                SECTION = dto.Section,
                CODE_PIP = dto.CodePip,
                CODE_BAILLEUR = dto.CodeBailleur,
                JUSTIFICATION_PROJET = dto.JustificationProjet,
                ETUDE_PREFAISABILITE = dto.EtudePrefaisabilite,
                ETUDE_FAISABILITE = dto.EtudeFaisabilite,
                OBJECTIF_GENERAL_PROJET = dto.ObjectifGeneralProjet,
                DUREE_PROJET = dto.DureeProjet,
                POPULATION_VISEE = dto.PopulationVisee,
                COUT_TOTAL_PROJET = dto.CoutTotalProjet,
                ECHELON_TERRITORIAL = dto.EchelonTerritorial,
                PROGRAMME = dto.Programme,
                SOUS_PROGRAMME = dto.SousProgramme,
                DATE_INSCRIPTION = dto.DateInscription?.ToString("yyyy-MM-dd"),
                DATE_MISE_A_JOUR = dto.DateMiseAJour?.ToString("yyyy-MM-dd"),

                // listes **partagées** (présentes dans 3 modules) :
                // ActiviteBPDto, DefinitionLivrablesDuProjetDto, InformationsFinancieresDto
                listActvt = dto.Activites?.Select(act => new
                {
                    ID_ACTIVITES = act.IdActivites,
                    NUMERO_ACTIVITES = act.NumeroActivites,
                    NOM_ACTIVITE = act.NomActivite,
                    Resultats_attendus = act.ResultatsAttendus,
                    listinfofinancieres = act.InformationsFinancieres?.Select(info => new
                    {
                        ID_INFORMATIONS_FINANCIERES = info.IdInformationsFinancieres,
                        EXERCICE_FISCAL_DEBUT = info.ExerciceFiscalDebut,
                        EXERCICE_FISCAL_FIN = info.ExerciceFiscalFin,
                        SOURCES_FINANCEMENT = info.SourcesFinancement,
                        ARTICLE = info.Article,
                        ALINEA = info.Alinea,
                        MONTANT_PREVU = info.MontantPrevu,
                        MOIS_PREVISION = info.MoisPrevision,
                        MONTANT_AUTORISATION = info.MontantAutorisation,
                        MOIS_AUTORISATION = info.MoisAutorisation,
                        MONTANT_DECAISSEMENT = info.MontantDecaissement,
                        MOIS_DECAISSEMENT = info.MoisDecaissement,
                        MONTANT_DEPENSE = info.MontantDepense,
                        MOIS_DEPENSE = info.MoisDepense
                    })
                }),

                listlivrprojet = dto.LivrablesProjets?.Select(l => new
                {
                    ID_LIVRABLES_PROJET = l.IdLivrablesProjet,
                    DEFINITION_LIVRABLES_DU_PROJET = l.DefinitionLivrablesDuProjet,
                    QUANTITE_A_LIVRER = l.QuantiteALivrer,
                    QUANTITE_LIVREE = l.QuantiteLivree,
                    VALEUR_LIVREE = l.ValeurLivree
                })
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() }
            };

            var json = JsonConvert.SerializeObject(payload, settings);
            _logger.LogInformation("📦 JSON (partagé) envoyé à AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON : {Len} chars", json.Length);

            await AjouterProjetEtListesJsonAsync(json);
        }

        /// <summary>
        /// Mettre à jour via DTO partagé (id requis). La procédure est idempotente.
        /// </summary>
        public async Task MettreAJourAsync(IdentificationProjetDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.IdIdentificationProjet))
                throw new ArgumentException("IdIdentificationProjet doit être renseigné pour la mise à jour.", nameof(dto));

            await AjouterAsync(dto); // la proc supprime si l'id existe puis insert
        }

        /// <summary>
        /// Supprime l'enregistrement (on appelle la proc en fournissant un JSON minimal contenant l'ID).
        /// Modules peuvent aussi appeler AjouterProjetEtListesJsonAsync directement avec leur payload delete.
        /// </summary>
        public async Task SupprimerAsync(string idIdentificationProjet)
        {
            if (string.IsNullOrWhiteSpace(idIdentificationProjet)) throw new ArgumentNullException(nameof(idIdentificationProjet));

            var payload = new { ID_IDENTIFICATION_PROJET = idIdentificationProjet };
            var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            _logger.LogInformation("🗑️ Demande suppression pour Id={Id}", idIdentificationProjet);
            await AjouterProjetEtListesJsonAsync(json);
        }

        public async Task<List<IdentificationProjetDto>> ObtenirTousAsync()
        {
            var vues = await _dbContext.ViewIdentificationProjetPlats
                .AsNoTracking()
                .ToListAsync();

            // Mapping minimal (adapter selon la shape de la view)
            return vues.Select(v => new IdentificationProjetDto
            {
                IdIdentificationProjet = v.IdIdentificationProjet,
                NomProjet = v.NomProjet,
                // mappe les autres champs si nécessaire
            }).ToList();
        }

        public async Task<IdentificationProjetDto?> ObtenirParIdAsync(string idIdentificationProjet)
        {
            var v = await _dbContext.ViewIdentificationProjetPlats
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdIdentificationProjet == idIdentificationProjet);

            if (v == null) return null;

            return new IdentificationProjetDto
            {
                IdIdentificationProjet = v.IdIdentificationProjet,
                NomProjet = v.NomProjet,
                // mappe les autres champs si nécessaire
            };
        }
    }
}
