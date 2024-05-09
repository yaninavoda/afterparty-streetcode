using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.Entities.AdditionalContent.Jwt;

namespace Streetcode.BLL.Entities.Users
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    }
}
