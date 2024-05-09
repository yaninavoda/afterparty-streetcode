using Streetcode.BLL.Entities.Media;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Media;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media
{
    public class AudioRepository : RepositoryBase<Audio>, IAudioRepository
    {
        public AudioRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}