using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Dto.Account;
using Streetcode.BLL.DTO.Account;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.MediatR.Account.Login;
using Streetcode.BLL.MediatR.Account.Register;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.WebApi.Controllers.Account
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerRequest)
        {
            return HandleResult(await Mediator.Send(new RegisterUserCommand(registerRequest)));
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Login(LoginUserDto loginUserDto)
        {
            return HandleResult(await Mediator.Send(new LoginUserCommand(loginUserDto)));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }
    }
}
