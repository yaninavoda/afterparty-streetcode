using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Streetcode.BLL.Interfaces.Users;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.Entities.AdditionalContent.Jwt;
using Streetcode.BLL.DTO.Account;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.RepositoryInterfaces.Base;
using Streetcode.BLL.Entities.Users;

namespace Streetcode.BLL.Services.Users
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly RefreshTokenConfiguration _refreshTokenConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TokenService(
            JwtConfiguration jwtConfiguration,
            RefreshTokenConfiguration refreshTokenConfiguration,
            UserManager<ApplicationUser> userManager,
            IRepositoryWrapper repositoryWrapper)
        {
            _jwtConfiguration = jwtConfiguration;
            _refreshTokenConfiguration = refreshTokenConfiguration;
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
        }

        public AuthenticationResponseDto GenerateJWTToken(ApplicationUser user, List<Claim> claims)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.ExpirationMinutes));

            SymmetricSecurityKey securityKey = new (Encoding.UTF8.GetBytes(_jwtConfiguration.Key!));

            SigningCredentials signingCredentials = new (securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new (
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new ();

            var token = jwtSecurityTokenHandler.WriteToken(tokenGenerator);

            AuthenticationResponseDto response = new ()
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddDays(_refreshTokenConfiguration.ExpirationDays)
            };

            return response;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            List<Claim> claims = new ()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user id)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Date and time of token generation
                new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()), // Email
                new Claim(ClaimTypes.Name, user.UserName.ToString()), // Name of the user
                new Claim(ClaimTypes.Email, user.Email), // Name of the user
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole)); // Role of the user
            }

            return claims;
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!)),
                ValidateLifetime = false,
            };

            JwtSecurityTokenHandler tokenValidationHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claims = tokenValidationHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return claims;
        }

        public void CreateRefreshToken(ApplicationUser user, AuthenticationResponseDto response)
        {
            var refreshToken = new RefreshTokenEntity
            {
                ApplicationUserId = user.Id,
                RefreshToken = response.RefreshToken!,
                RefreshTokenExpirationDateTime = response.RefreshTokenExpirationDateTime,
            };

            _repositoryWrapper.RefreshTokenRepository.Create(refreshToken);
        }

        private static string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];

            var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
