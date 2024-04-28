using Streetcode.DAL.Entities.AdditionalContent.Jwt;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent;

public class RefreshTokenRepository : RepositoryBase<RefreshTokenEntity>, IRefreshTokenRepository
{
    public RefreshTokenRepository(StreetcodeDbContext context)
        : base(context)
    {
    }
}
