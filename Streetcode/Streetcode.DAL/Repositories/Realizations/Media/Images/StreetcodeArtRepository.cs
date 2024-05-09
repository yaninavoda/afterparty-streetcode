using Streetcode.BLL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images
{
    public class StreetcodeArtRepository : RepositoryBase<StreetcodeArt>, IStreetcodeArtRepository
    {
        public StreetcodeArtRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
