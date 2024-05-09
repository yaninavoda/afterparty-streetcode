using AutoMapper;
using Streetcode.BLL.Dto.Timeline;
using Streetcode.BLL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline
{
    public class HistoricalContextProfile : Profile
    {
        public HistoricalContextProfile()
        {
            CreateMap<HistoricalContext, HistoricalContextDto>().ReverseMap();
        }
    }
}
