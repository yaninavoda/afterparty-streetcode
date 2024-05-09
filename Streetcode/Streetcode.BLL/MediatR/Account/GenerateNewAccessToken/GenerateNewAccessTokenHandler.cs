using System.Security.Claims;
using MediatR;
using Streetcode.BLL.DTO.Account;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.RepositoryInterfaces.Base;
using Streetcode.BLL.Entities.Users;

namespace Streetcode.BLL.MediatR.Account.GenerateNewAccessToken
{
    public sealed class GenerateNewAccessTokenHandler : IRequestHandler<GenerateNewAccessTokenCommand, Result<AuthenticationResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public GenerateNewAccessTokenHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILoggerService logger, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<AuthenticationResponseDto>> Handle(GenerateNewAccessTokenCommand command, CancellationToken cancellationToken)
        {
            var request = command.TokenModel;

            string accessToken = request.Token;

            string refreshTokenFromRequest = request.RefreshToken;

            ClaimsPrincipal principals = _tokenService.GetPrincipalFromJwtToken(accessToken)!;

            if (principals is null)
            {
                return InvalidJwtToken(accessToken);
            }

            string email = principals.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            // find refresh token in db
            var savedRefreshToken = await _repositoryWrapper.RefreshTokenRepository.GetFirstOrDefaultAsync(
                rt => rt.RefreshToken == refreshTokenFromRequest
                && rt.ApplicationUserId == user.Id);

            if (user is null
                || savedRefreshToken is null
                || savedRefreshToken.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            {
                return InvalidRefreshToken(refreshTokenFromRequest);
            }

            var claims = await _tokenService.GetUserClaimsAsync(user);

            var response = _tokenService.GenerateJWTToken(user, claims);

            _tokenService.CreateRefreshToken(user, response);

            if (await _repositoryWrapper.SaveChangesAsync() <= 0)
            {
                return FailedToSaveRefreshTokenError(response);
            }

            return Result.Ok(response);
        }

        private Result<AuthenticationResponseDto> FailedToSaveRefreshTokenError(AuthenticationResponseDto response)
        {
            var errorMessage = "Failed to save refresh token.";

            _logger.LogError(response, errorMessage);

            return Result.Fail(errorMessage);
        }

        private Result<AuthenticationResponseDto> InvalidRefreshToken(string refreshToken)
        {
            var errorMessage = "Invalid Refresh Token";

            _logger.LogError(refreshToken, errorMessage);

            return Result.Fail(errorMessage);
        }

        private Result<AuthenticationResponseDto> InvalidJwtToken(string jwtToken)
        {
            var errorMessage = "Invalid Jwt Token";

            _logger.LogError(jwtToken, errorMessage);

            return Result.Fail(errorMessage);
        }
    }
}
