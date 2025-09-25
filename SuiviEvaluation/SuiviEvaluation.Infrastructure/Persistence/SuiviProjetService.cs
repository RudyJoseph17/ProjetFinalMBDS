using SuiviEvaluation.Application.Dtos;
using SuiviEvaluation.Application.Interfaces;
using Shared.Domain.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Services
{
    public class SuiviProjetService : ISuiviProjetService
    {
        // Simule une base de données en mémoire
        private readonly List<SuiviProjetDto> _projets = new();

        public SuiviProjetService()
        {
            // Exemple de données initiales pour test
            _projets.Add(new SuiviProjetDto
            {
                IdIdentificationProjet = "P001",
                NomProjet = "Projet Alpha",
                AutorisationsSurProjets = new List<AutorisationSurProjetDto>(),
                LivrablesRealisesProjets = new List<LivrablesRealisesProjetDto>(),
            });

            _projets.Add(new SuiviProjetDto
            {
                IdIdentificationProjet = "P002",
                NomProjet = "Projet Beta",
                AutorisationsSurProjets = new List<AutorisationSurProjetDto>(),
                LivrablesRealisesProjets = new List<LivrablesRealisesProjetDto>(),
            });
        }

        public Task AjouterAsync(SuiviProjetDto suiviProjetDto)
        {
            _projets.Add(suiviProjetDto);
            return Task.CompletedTask;
        }

        public Task MettreAJourAsync(SuiviProjetDto suiviProjetDto)
        {
            var existing = _projets.FirstOrDefault(p => p.IdIdentificationProjet == suiviProjetDto.IdIdentificationProjet);
            if (existing != null)
            {
                existing.NomProjet = suiviProjetDto.NomProjet;
                existing.LivrablesRealisesProjets = suiviProjetDto.LivrablesRealisesProjets ?? new List<LivrablesRealisesProjetDto>();
                existing.AutorisationsSurProjets = suiviProjetDto.AutorisationsSurProjets ?? new List<AutorisationSurProjetDto>();
                existing.DecaissementsSurProjets = suiviProjetDto.DecaissementsSurProjets ?? new List<DecaissementSurProjetDto>();
                existing.DepensesReellesSurProjets = suiviProjetDto.DepensesReellesSurProjets ?? new List<DepenseReelleSurProjetDto>();
            }
            return Task.CompletedTask;
        }

        public Task SupprimerAsync(string IdIdentificationProjet)
        {
            var projet = _projets.FirstOrDefault(p => p.IdIdentificationProjet == IdIdentificationProjet);
            if (projet != null)
            {
                _projets.Remove(projet);
            }
            return Task.CompletedTask;
        }

        public Task<List<SuiviProjetDto>> ObtenirTousAsync()
        {
            return Task.FromResult(_projets.ToList());
        }

        public Task<SuiviProjetDto?> ObtenirParIdAsync(string id)
        {
            var projet = _projets.FirstOrDefault(p => p.IdIdentificationProjet == id);
            return Task.FromResult(projet);
        }

        public Task<SuiviProjetDto> ObtenirParNomAsync(string nomProjet)
        {
            var projet = _projets.FirstOrDefault(p => p.NomProjet?.ToLower() == nomProjet?.ToLower());
            return Task.FromResult(projet ?? new SuiviProjetDto());
        }

        public Task<IEnumerable<object>> ObtenirActivitesParProjetAsync(string projectId)
        {
            var projet = _projets.FirstOrDefault(p => p.IdIdentificationProjet == projectId);
            if (projet == null)
                return Task.FromResult(Enumerable.Empty<object>());

            // Exemple d’activités simulées
            var activites = new List<SuiviActivite>
            {
                new SuiviActivite { IdActivites = 1, NomActivite = "Analyse initiale", Autorisations = new List<AutorisationSurProjetDto>() },
                new SuiviActivite { IdActivites = 2, NomActivite = "Mise en œuvre", Autorisations = new List<AutorisationSurProjetDto>() },
                new SuiviActivite { IdActivites = 3, NomActivite = "Rapport final", Autorisations = new List<AutorisationSurProjetDto>() }
            };

            return Task.FromResult(activites.Cast<object>());
        }
    }
}