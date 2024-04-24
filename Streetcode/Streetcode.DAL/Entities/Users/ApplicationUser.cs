using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.AdditionalContent.Jwt;

namespace Streetcode.DAL.Entities.Users;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
