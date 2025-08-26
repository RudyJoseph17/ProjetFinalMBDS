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
        private readonly IMapper _mapper;

        public ActiviteBPService(
            IActiviteService activiteService,
            IMapper mapper)
        {
            _activiteService = activiteService;
            _mapper = mapper;
        }

        public async Task<List<ActiviteBPDto>> ObtenirTousAsync()
        {
            var sharedList = await _activiteService.ObtenirTousAsync();
            return _mapper.Map<List<ActiviteBPDto>>(sharedList);
        }

        public async Task<ActiviteBPDto?> ObtenirParIdAsync(byte id)
        {
            // ici id est un byte : on ne peut pas utiliser IsNullOrWhiteSpace
            var sharedList = await _activiteService.ObtenirTousAsync();
            if (sharedList == null || !sharedList.Any())
                return null;

            // comparer sur la propriété IdActivites (type byte)
            var match = sharedList.FirstOrDefault(p => p.IdActivites == id);
            return match is null ? null : _mapper.Map<ActiviteBPDto>(match);
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
