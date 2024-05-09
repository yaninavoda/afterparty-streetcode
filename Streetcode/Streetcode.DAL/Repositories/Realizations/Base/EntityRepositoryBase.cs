using Streetcode.BLL.Contracts;
using Streetcode.BLL.RepositoryInterfaces.Base;
using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Realizations.Base;

public class EntityRepositoryBase<T> : RepositoryBase<T>, IEntityRepositoryBase<T>
    where T : class, IEntity
{
    public EntityRepositoryBase(StreetcodeDbContext context)
        : base(context)
    {
    }
}