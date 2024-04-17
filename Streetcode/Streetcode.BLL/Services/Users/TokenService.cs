using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.DAL.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Streetcode.DAL.Entities.AdditionalContent.Jwt;

namespace Streetcode.BLL.Services.Users;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration;
    public TokenService(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public JwtSecurityToken GenerateJWTToken(ApplicationUser user)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.ExpirationMinutes));

        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user id)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Date and time of token generation
            new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()), // Email
            new Claim(ClaimTypes.Name, user.UserName.ToString()), // Name of the user
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        return tokenGenerator;
    }

    public JwtSecurityToken RefreshToken(string token)
    {
        throw new NotImplementedException();
    }
}
