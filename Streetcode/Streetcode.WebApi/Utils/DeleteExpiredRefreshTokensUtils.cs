using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.WebApi.Utils;

public class DeleteExpiredRefreshTokensUtils
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public DeleteExpiredRefreshTokensUtils(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task DeleteExpiredRefreshTokens()
    {
        var refreshTokens = await _repositoryWrapper.RefreshTokenRepository
            .GetAllAsync(rt => rt.RefreshTokenExpirationDateTime <= DateTime.UtcNow);

        if (refreshTokens is not null)
        {
            _repositoryWrapper.RefreshTokenRepository.DeleteRange(refreshTokens);
            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
