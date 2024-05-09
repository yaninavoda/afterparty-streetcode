using Streetcode.BLL.Entities.Media.Images;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.RepositoryInterfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}