using Streetcode.BLL.Entities.Timeline;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Timeline;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Timeline
{
    public class TimelineRepository : RepositoryBase<TimelineItem>, ITimelineRepository
    {
        public TimelineRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}