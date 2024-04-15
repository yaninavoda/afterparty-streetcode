using FluentValidation;
namespace Streetcode.BLL.DTO.Account.Validation;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public const int MINPASSLENGTH = 6;
    public LoginUserDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .MinimumLength(MINPASSLENGTH)
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$");
    }
}
