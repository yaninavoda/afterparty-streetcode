using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent
{
    public class FactRepository : RepositoryBase<Fact>, IFactRepository
    {
        private readonly StreetcodeDbContext _context;
        public FactRepository(StreetcodeDbContext streetcodeDbContext)
            : base(streetcodeDbContext)
        {
            _context = streetcodeDbContext;
        }

        public Task<int> GetMaxNumberAsync(int streetcodeId)
        {
            return _context.Facts
                .Where(f => f.StreetcodeId == streetcodeId)
                .MaxAsync(f => f.Number);
        }
    }
}