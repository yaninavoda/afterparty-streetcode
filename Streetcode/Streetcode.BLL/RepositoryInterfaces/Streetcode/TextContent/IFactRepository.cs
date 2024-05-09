using Streetcode.BLL.Entities.Streetcode.TextContent;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.RepositoryInterfaces.Streetcode.TextContent
{
    public interface IFactRepository : IRepositoryBase<Fact>
    {
        Task<int> GetMaxNumberAsync(int streetcodeId);
    }
}