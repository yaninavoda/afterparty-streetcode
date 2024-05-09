using Streetcode.BLL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent
{
    public class TextRepository : RepositoryBase<Text>, ITextRepository
    {
        public TextRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}