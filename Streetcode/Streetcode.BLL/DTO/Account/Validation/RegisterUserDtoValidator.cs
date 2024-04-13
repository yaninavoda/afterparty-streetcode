using FluentValidation;
using Streetcode.BLL.Dto.Account;

namespace Streetcode.BLL.MediatR.Account.Register;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public const int MAXNAMELENGTH = 50;
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .MaximumLength(MAXNAMELENGTH);

        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .MaximumLength(MAXNAMELENGTH);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
