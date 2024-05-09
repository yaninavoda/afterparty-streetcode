using Streetcode.BLL.Entities.Partners;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Partners;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Partners
{
    public class PartnersRepository : RepositoryBase<Partner>, IPartnersRepository
    {
        public PartnersRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}