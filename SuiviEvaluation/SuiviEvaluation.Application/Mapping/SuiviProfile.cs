using AutoMapper;
using Shared.Domain.Dtos;
using SuiviEvaluation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEvaluation.Application.Mapping
{
    public class SuiviProfile: Profile
    {
        public SuiviProfile()
        {
            CreateMap<InformationsFinancieresProjetDto, SuiviInformationFinanciereDto>()
                .ReverseMap();

            CreateMap<LivrablesDuProjetDto, QuantiteLivreeParAnneeDto>()
                .ReverseMap();
        }
    }
}
