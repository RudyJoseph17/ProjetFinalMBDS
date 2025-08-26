using AutoMapper;
using BanqueProjet.Application.Dtos;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Infrastructure.Mapping
{
    public class ProjetProfile: Profile
    {
        public ProjetProfile()
        {
            CreateMap<IdentificationProjetDto, ProjetsBPDto>()
                .ReverseMap();

            CreateMap<ActiviteDto, ActiviteBPDto>()
                .ReverseMap();

        }
    }
}
