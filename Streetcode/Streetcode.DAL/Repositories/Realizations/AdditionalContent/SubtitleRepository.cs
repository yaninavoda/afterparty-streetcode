using Streetcode.BLL.Entities.AdditionalContent;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent
{
    public class SubtitleRepository : RepositoryBase<Subtitle>, ISubtitleRepository
    {
        public SubtitleRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}