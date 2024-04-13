using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Account;

public class RegisterUserDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
