using Streetcode.BLL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class PositionRepository : RepositoryBase<Positions>, IPositionRepository
    {
        public PositionRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
