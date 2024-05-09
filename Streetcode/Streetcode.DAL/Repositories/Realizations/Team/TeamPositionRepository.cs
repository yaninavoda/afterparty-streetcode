using Streetcode.BLL.Entities.Partners;
using Streetcode.BLL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Partners;
using Streetcode.BLL.RepositoryInterfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class TeamPositionRepository : RepositoryBase<TeamMemberPositions>, ITeamPositionRepository
    {
        public TeamPositionRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
