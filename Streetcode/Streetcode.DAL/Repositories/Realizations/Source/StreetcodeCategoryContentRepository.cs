using Streetcode.BLL.Entities.Sources;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Source;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Source
{
    public class StreetcodeCategoryContentRepository : RepositoryBase<StreetcodeCategoryContent>, IStreetcodeCategoryContentRepository
    {
        public StreetcodeCategoryContentRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
