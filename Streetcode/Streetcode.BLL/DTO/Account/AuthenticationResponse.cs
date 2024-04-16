namespace Streetcode.BLL.DTO.Account;

public class AuthenticationResponse
{
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Token { get; set; } = string.Empty;
}
