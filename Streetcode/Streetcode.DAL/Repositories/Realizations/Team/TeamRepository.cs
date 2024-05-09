using Streetcode.BLL.Entities.AdditionalContent;
using Streetcode.BLL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.AdditionalContent;
using Streetcode.BLL.RepositoryInterfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class TeamRepository : RepositoryBase<TeamMember>, ITeamRepository
    {
        public TeamRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
