using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Shared.Infrastructure.Persistence;
using AutoMapper;
using Shared.Domain.Interface;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class ActiviteBPService : IActiviteBPService
    {
        private readonly IActiviteService _activiteService;
        private readonly BanquePDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActiviteBPService(
            IActiviteService activiteService,
            BanquePDbContext dbContext,
            IMapper mapper)
        {
            _activiteService = activiteService;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ActiviteBPDto>> ObtenirTousAsync()
        {
            var entities = await _dbContext.OViewActivitesBP
                .AsNoTracking()
                .ToListAsync();

            // Mappez-les vers vos DTOs métier
            return entities.Select(e => new ActiviteBPDto
            {
                IdActivites = (byte)e.IdActivites,
                NumeroActivites = e.NumeroActivites,
                NomActivite = e.NomActivite,
                ResultatsAttendus = e.ResultatsAttendus,
                IdIdentificationProjet = e.IdIdentificationProjet
            })
            .ToList();
        }

        public async Task<ActiviteBPDto?> ObtenirParIdAsync(byte id)
        {
            var e = await _dbContext.OViewActivitesBP
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdActivites == id);

            if (e == null) return null;

            return new ActiviteBPDto
            {
                IdActivites = (byte)e.IdActivites,
                NumeroActivites = e.NumeroActivites,
                NomActivite = e.NomActivite,
                ResultatsAttendus = e.ResultatsAttendus,
                IdIdentificationProjet = e.IdIdentificationProjet
            };
        }

        public Task AjouterAsync(ActiviteBPDto activiteBP)
        {
            // TODO : implémenter la logique d'ajout en BDD
            throw new NotImplementedException();
        }

        public Task MettreAJourAsync(ActiviteBPDto activiteBP)
        {
            // TODO : implémenter la mise à jour des infos financières
            throw new NotImplementedException();
        }

        public Task SupprimerAsync(byte IdActivites)
        {
            // TODO : implémenter la suppression en BDD
            throw new NotImplementedException();
        }
    }
}
