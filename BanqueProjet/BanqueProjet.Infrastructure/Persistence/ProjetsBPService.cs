using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using Shared.Domain.Interface;

namespace BanqueProjet.Infrastructure.Persistence
{
    public class ProjetsBPService : IProjetsBPService
    {
        private readonly IIdentificationProjetService _identificationService;
        private readonly IMapper _mapper;

        public ProjetsBPService(
            IIdentificationProjetService identificationService,
            IMapper mapper)
        {
            _identificationService = identificationService;
            _mapper = mapper;
        }

        public async Task<List<ProjetsBPDto>> ObtenirTousAsync()
        {
            var sharedList = await _identificationService.ObtenirTousAsync();
            return _mapper.Map<List<ProjetsBPDto>>(sharedList);
        }

        public async Task<ProjetsBPDto?> ObtenirParIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var sharedList = await _identificationService.ObtenirTousAsync();

            // comparaison string ↔ string
            var match = sharedList.FirstOrDefault(p => p.IdIdentificationProjet == id);

            return match is null
                ? null
                : _mapper.Map<ProjetsBPDto>(match);
        }

        public Task AjouterAsync(ProjetsBPDto projetsBPD)
        {
            // TODO : implémenter la logique d'ajout en BDD
            throw new NotImplementedException();
        }

        public Task MettreAJourAsync(ProjetsBPDto projetsBPD)
        {
            // TODO : implémenter la mise à jour des infos financières
            throw new NotImplementedException();
        }

        public Task SupprimerAsync(string IdIdentificationProjet)
        {
            // TODO : implémenter la suppression en BDD
            throw new NotImplementedException();
        }
    }
}
