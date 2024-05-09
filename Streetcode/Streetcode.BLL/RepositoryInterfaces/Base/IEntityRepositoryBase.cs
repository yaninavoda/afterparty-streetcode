using Streetcode.BLL.Contracts;

namespace Streetcode.BLL.RepositoryInterfaces.Base
{
    public interface IEntityRepositoryBase<T> : IRepositoryBase<T>
        where T : class, IEntity
    {
    }
}