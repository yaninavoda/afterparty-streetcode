using Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent
{
    public class StreetcodeCoordinateRepository : RepositoryBase<StreetcodeCoordinate>, IStreetcodeCoordinateRepository
    {
        public StreetcodeCoordinateRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}