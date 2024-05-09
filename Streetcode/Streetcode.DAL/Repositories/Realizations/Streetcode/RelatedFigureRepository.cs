using Streetcode.BLL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode
{
    internal class RelatedFigureRepository : RepositoryBase<RelatedFigure>, IRelatedFigureRepository
    {
        public RelatedFigureRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}