using AutoMapper;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Dto.Streetcode.Types;
using Streetcode.BLL.Entities.Streetcode;
using Streetcode.BLL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode.Types
{
    public class EventStreetcodeProfile : Profile
    {
        public EventStreetcodeProfile()
        {
            CreateMap<EventStreetcode, EventStreetcodeDto>()
                .IncludeBase<StreetcodeContent, StreetcodeDto>().ReverseMap();
      }
    }
}
