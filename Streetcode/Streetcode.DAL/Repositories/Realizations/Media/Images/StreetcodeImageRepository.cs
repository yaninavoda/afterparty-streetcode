using Streetcode.BLL.Entities.Media.Images;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images
{
	public class StreetcodeImageRepository : RepositoryBase<StreetcodeImage>, IStreetcodeImageRepository
	{
		public StreetcodeImageRepository(StreetcodeDbContext context)
			: base(context)
		{
		}
	}
}
