using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Dto.Account;
using Streetcode.DAL.Entities.Users;
using UserRole = Streetcode.DAL.Entities.Users.UserRole;

namespace Streetcode.WebApi.Controllers.Account
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterUserDto registerDto)
        {
            await IsEmailAlreadyInUse(registerDto.Email!);

            // Create user
            ApplicationUser user = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName!,
                LastName = registerDto.LastName!,
            };

            IdentityResult creatingUserResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!creatingUserResult.Succeeded)
            {
                return FailedToRegister(creatingUserResult);
            }

            // sign-in
            await _signInManager.SignInAsync(user, isPersistent: false);

            // add user role
            IdentityResult addingRoleResult = await _userManager.AddToRoleAsync(user, UserRole.USER);

            if (!addingRoleResult.Succeeded)
            {
                return FailedToAssignRole(addingRoleResult);
            }

            return Ok(user);
        }

        private ActionResult<ApplicationUser> FailedToAssignRole(IdentityResult addingRoleResult)
        {
            string errorMessage = string.Join(
                Environment.NewLine,
                addingRoleResult.Errors.Select(e => e.Description));

            return Problem(errorMessage);
        }

        private ActionResult<ApplicationUser> FailedToRegister(IdentityResult creatingUserResult)
        {
            string errorMessage = string.Join(
                Environment.NewLine,
                creatingUserResult.Errors.Select(e => e.Description));

            return Problem(errorMessage);
        }

        private async Task<IActionResult> IsEmailAlreadyInUse(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            return user is null ?
                Ok(true) :
                Ok(false);
        }
    }
}
