using Streetcode.BLL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent
{
    public class TermRepository : RepositoryBase<Term>, ITermRepository
    {
        public TermRepository(StreetcodeDbContext streetcodeDbContext)
            : base(streetcodeDbContext)
        {
        }
    }
}