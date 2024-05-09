using Streetcode.BLL.Entities.News;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Newss;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Newss
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
