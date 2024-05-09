using AutoMapper;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Entities.Team;

namespace Streetcode.BLL.Mapping.Team
{
    public class TeamLinkProfile : Profile
    {
        public TeamLinkProfile()
        {
            CreateMap<TeamMemberLink, TeamMemberLinkDto>().ReverseMap();
        }
    }
}
