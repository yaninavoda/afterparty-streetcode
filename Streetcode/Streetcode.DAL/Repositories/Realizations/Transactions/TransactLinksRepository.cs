using Streetcode.BLL.Entities.Transactions;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Transactions;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Transactions
{
    public class TransactLinksRepository : RepositoryBase<TransactionLink>, ITransactLinksRepository
    {
        public TransactLinksRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}