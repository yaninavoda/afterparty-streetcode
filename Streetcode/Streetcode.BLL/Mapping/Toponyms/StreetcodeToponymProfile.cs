using AutoMapper;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Dto.Toponyms;
using Streetcode.BLL.DTO.StreetcodeToponym;
using Streetcode.BLL.Entities.Toponyms;
using Streetcode.BLL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Toponyms
{
    public class StreetcodeToponymProfile : Profile
    {
        public StreetcodeToponymProfile()
        {
            CreateMap<CreateStreetcodeToponymRequestDto, StreetcodeToponym>().ReverseMap();
        }
    }
}
