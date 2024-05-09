using AutoMapper;
using Streetcode.BLL.Dto.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.Entities.Partners;

namespace Streetcode.BLL.Mapping.Partners
{
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, PartnerDto>()
                .ForPath(dto => dto.TargetUrl.Title, conf => conf.MapFrom(ol => ol.UrlTitle))
                .ForPath(dto => dto.TargetUrl.Href, conf => conf.MapFrom(ol => ol.TargetUrl));
            CreateMap<Partner, CreatePartnerDto>().ReverseMap();
            CreateMap<Partner, PartnerShortDto>().ReverseMap();
            CreateMap<CreatePartnerRequestDto, Partner>();
            CreateMap<Partner, CreatePartnerResponseDto>();
        }
    }
}
