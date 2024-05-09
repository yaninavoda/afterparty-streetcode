using Streetcode.BLL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Toponyms;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Toponyms
{
	public class StreetcodeToponymRepository : RepositoryBase<StreetcodeToponym>, IStreetcodeToponymRepository
	{
		public StreetcodeToponymRepository(StreetcodeDbContext context)
			: base(context)
		{
		}
	}
}
