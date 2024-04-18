using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Streetcode.BLL.DTO.Account;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Interfaces.Users
{
    public interface ITokenService
    {
        public AuthenticationResponseDto GenerateJWTToken(ApplicationUser user);

        // claims principal represents user details
        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
