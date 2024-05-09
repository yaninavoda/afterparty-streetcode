using Streetcode.BLL.Entities.Analytics;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Analytics;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Analytics
{
    public class StatisticRecordsRepository : RepositoryBase<StatisticRecord>, IStatisticRecordRepository
    {
        public StatisticRecordsRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
