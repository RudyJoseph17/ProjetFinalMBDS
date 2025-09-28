using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Infrastructure.Data; // BanquePDbContext
using BanqueProjet.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class ActiviteBPService : IActiviteBPService
    {
        private readonly BanquePDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ActiviteBPService> _logger;

        public ActiviteBPService(BanquePDbContext dbContext, IMapper mapper, ILogger<ActiviteBPService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        // lectures existantes réutilisées (en supposant mapping manuel ou AutoMapper)
        public async Task<List<ActiviteBPDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.OViewActivitesBP
                .AsNoTracking()
                .ToListAsync();

            return entities.Select(e => new ActiviteBPDto
            {
                IdActivites = e.IdActivites,
                NumeroActivites = e.NumeroActivites,
                NomActivite = e.NomActivite,
                ResultatsAttendus = e.ResultatsAttendus,
                IdIdentificationProjet = e.IdIdentificationProjet
            }).ToList();
        }

        public async Task<ActiviteBPDto?> ObtenirParIdAsync(int id)
        {
            var e = await _dbContext.OViewActivitesBP
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdActivites == id);

            if (e == null) return null;

            return new ActiviteBPDto
            {
                IdActivites = e.IdActivites,
                NumeroActivites = e.NumeroActivites,
                NomActivite = e.NomActivite,
                ResultatsAttendus = e.ResultatsAttendus,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        // CREATE
        public async Task AjouterAsync(ActiviteBPDto activiteBP)
        {
            if (activiteBP == null) throw new ArgumentNullException(nameof(activiteBP));

            // Mapper DTO -> Entity (exemple générique)
            var entity = new OViewActiviteBP
            {
                IdActivites = activiteBP.IdActivites == 0 ? 0 : activiteBP.IdActivites,
                NumeroActivites = (byte?)activiteBP.NumeroActivites,
                NomActivite = activiteBP.NomActivite,
                ResultatsAttendus = activiteBP.ResultatsAttendus,
                IdIdentificationProjet = activiteBP.IdIdentificationProjet
            };

            await _dbContext.OViewActivitesBP.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            // Enregistrer enfants (ActivitesAnnuelles, InformationsFinancieres) si présents

            if (activiteBP.InformationsFinancieres?.Any() == true)
            {
                foreach (var inf in activiteBP.InformationsFinancieres)
                {
                    var fin = new OViewIformationsFinancierePrevision
                    {
                        IdInformationsFinancieres = inf.IdInformationsFinancieres,
                        ExerciceFiscalDebut = inf.ExerciceFiscalDebut,
                        ExerciceFiscalFin = inf.ExerciceFiscalFin,
                        SourcesFinancement = inf.SourcesFinancement,
                        Article = inf.Article,
                        Alinea = inf.Alinea,
                        MontantPrevu = inf.MontantPrevu,
                        IdActivites = entity.IdActivites
                    };
                    _dbContext.OViewIformationsFinancierePrevisions.Add(fin);
                }
                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Activité ajoutée Id={Id}", entity.IdActivites);
        }

        // UPDATE
        public async Task MettreAJourAsync(ActiviteBPDto activiteBP)
        {
            if (activiteBP == null) throw new ArgumentNullException(nameof(activiteBP));

            var existing = await _dbContext.OViewActivitesBP
               .Include(a => a.OViewIformationsFinancierePrevisions)
                            .FirstOrDefaultAsync(a => a.IdActivites == activiteBP.IdActivites);

            if (existing == null)
            {
                // Option : créer si absent
                await AjouterAsync(activiteBP);
                return;
            }

            // Mettre à jour champs simples
            existing.NumeroActivites = (byte?)activiteBP.NumeroActivites;
            existing.NomActivite = activiteBP.NomActivite;
            existing.ResultatsAttendus = activiteBP.ResultatsAttendus;
            existing.IdIdentificationProjet = activiteBP.IdIdentificationProjet;

            // Synchroniser InformationsFinancieres : replace all (exemple)
            if (activiteBP.InformationsFinancieres != null)
            {
                var toRemoveFin = existing.OViewIformationsFinancierePrevisions
             .Where(ei => !activiteBP.InformationsFinancieres
                 .Any(f => f.IdInformationsFinancieres == ei.IdInformationsFinancieres))
             .ToList();
                _dbContext.OViewIformationsFinancierePrevisions.RemoveRange(toRemoveFin);

                foreach (var finDto in activiteBP.InformationsFinancieres)
                {
                    var fin = existing.OViewIformationsFinancierePrevisions
                      .FirstOrDefault(x => x.IdInformationsFinancieres == finDto.IdInformationsFinancieres);
                    if (fin == null)
                    {
                        fin = new OViewIformationsFinancierePrevision
                        {
                            ExerciceFiscalDebut = finDto.ExerciceFiscalDebut,
                            ExerciceFiscalFin = finDto.ExerciceFiscalFin,
                            SourcesFinancement = finDto.SourcesFinancement,
                            Article = finDto.Article,
                            Alinea = finDto.Alinea,
                            MontantPrevu = finDto.MontantPrevu,
                            IdActivites = existing.IdActivites
                        };
                        _dbContext.OViewIformationsFinancierePrevisions.Add(fin);
                    }
                    else
                    {
                        fin.ExerciceFiscalDebut = finDto.ExerciceFiscalDebut;
                        fin.ExerciceFiscalFin = finDto.ExerciceFiscalFin;
                        fin.SourcesFinancement = finDto.SourcesFinancement;
                        fin.Article = finDto.Article;
                        fin.Alinea = finDto.Alinea;
                        fin.MontantPrevu = finDto.MontantPrevu;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Activité mise à jour Id={Id}", existing.IdActivites);
        }

        // DELETE
        public async Task SupprimerAsync(int idActivites)
        {
            var existing = await _dbContext.OViewActivitesBP
                .Include(a => a.OViewIformationsFinancierePrevision)
                .FirstOrDefaultAsync(a => a.IdActivites == idActivites);

            if (existing == null)
            {
                _logger.LogWarning("Tentative de suppression d'une activité inexistante Id={Id}", idActivites);
                return;
            }

            // supprimer les enfants puis la parent (si FK ON DELETE CASCADE absent)
            _dbContext.OViewIformationsFinancierePrevisions.RemoveRange((OViewIformationsFinancierePrevision)existing.OViewIformationsFinancierePrevision);
            _dbContext.OViewActivitesBP.Remove(existing);

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Activité supprimée Id={Id}", idActivites);
        }
    }
}