using AutoMapper;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Entities.Team;

namespace Streetcode.BLL.Mapping.Team
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Positions, PositionDto>().ReverseMap();
        }
    }
}
