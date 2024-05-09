﻿using AutoMapper;
using Streetcode.BLL.Dto.Partners;
using Streetcode.BLL.Dto.Partners.Create;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners
{
    public class PartnerSourceLinkProfile : Profile
    {
        public PartnerSourceLinkProfile()
        {
            CreateMap<PartnerSourceLink, PartnerSourceLinkDto>()
                .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
            CreateMap<PartnerSourceLink, CreatePartnerSourceLinkDto>().ReverseMap();
            CreateMap<CreatePartnerSourceLinkRequestDto, PartnerSourceLink>();
            CreateMap<PartnerSourceLink, CreatePartnerSourceLinkResponseDto>();
        }
    }
}
