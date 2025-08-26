using AutoMapper;
using Shared.Domain.Dtos;
using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Mapping
{
    public class SharedMappingProfile: Profile
    {
        public SharedMappingProfile()
        {
            // mapping simple — adapte les noms de propriétés si nécessaire
            CreateMap<ViewIdentificationProjetPlat, IdentificationProjetDto>()
                // exemples de mappings si les noms diffèrent :
                .ForMember(dest => dest.IdIdentificationProjet, opt => opt.MapFrom(src => src.IdIdentificationProjet))
                .ForMember(dest => dest.NomProjet, opt => opt.MapFrom(src => src.NomProjet));
            // ignore les props qui n'existent pas dans le DTO
            //.ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OViewActivite, ActiviteDto>()
                // exemples de mappings si les noms diffèrent :
                .ForMember(dest => dest.IdIdentificationProjet, opt => opt.MapFrom(src => src.IdIdentificationProjet))
                .ForMember(dest => dest.IdActivites, opt => opt.MapFrom(src => src.IdActivites));
        }
    }
}
