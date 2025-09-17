using AutoMapper;
using Programmation.Application.Dtos;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Application.Mapping
{
    public class ProgrammationProfile: Profile
    {
        public ProgrammationProfile()
        {
            CreateMap<InformationsFinancieresProjetDto, InformationsFinancieresProgrammeesProjetDto>()
                .ReverseMap();

            CreateMap<LivrablesDuProjetDto, LivrablesProgrameProjetDto>()
                .ReverseMap();

            CreateMap<IdentificationProjetDto, ProgrammationProjetDto>()
               .ReverseMap();
        }
    }
}
