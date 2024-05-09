using AutoMapper;
using Streetcode.BLL.Dto.Toponyms;
using Streetcode.BLL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Toponyms
{
    public class ToponymProfile : Profile
    {
        public ToponymProfile()
        {
            CreateMap<Toponym, ToponymDto>().ReverseMap();
    	}
    }
}
