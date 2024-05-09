using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Streetcode;
using Streetcode.BLL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode
{
    public class StreetcodeRepository : RepositoryBase<StreetcodeContent>, IStreetcodeRepository
    {
        public StreetcodeRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
