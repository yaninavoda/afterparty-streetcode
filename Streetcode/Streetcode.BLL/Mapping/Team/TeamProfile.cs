using AutoMapper;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Entities.Team;

namespace Streetcode.BLL.Mapping.Team
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamMember, TeamMemberDto>().ReverseMap();
        }
    }
}
