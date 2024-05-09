using Microsoft.AspNetCore.Identity;

namespace Streetcode.BLL.Entities.Users
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }

        public ApplicationRole()
        {
        }
    }
}
