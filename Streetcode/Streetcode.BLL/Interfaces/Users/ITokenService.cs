using System.Security.Claims;
using Streetcode.BLL.DTO.Account;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Interfaces.Users
{
    public interface ITokenService
    {
        AuthenticationResponseDto GenerateJWTToken(ApplicationUser user, List<Claim> claims);
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
        void CreateRefreshToken(ApplicationUser user, AuthenticationResponseDto response);
    }
}
