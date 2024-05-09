using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Team;

namespace Streetcode.BLL.MediatR.Team.GetAllMain
{
    public record GetAllMainTeamQuery : IRequest<Result<IEnumerable<TeamMemberDto>>>;
}
