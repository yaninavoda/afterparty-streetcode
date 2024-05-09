using Streetcode.BLL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class TeamLinkRepository : RepositoryBase<TeamMemberLink>, ITeamLinkRepository
    {
        public TeamLinkRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
