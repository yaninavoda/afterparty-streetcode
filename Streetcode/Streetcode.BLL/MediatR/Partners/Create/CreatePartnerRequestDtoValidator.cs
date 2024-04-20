using FluentValidation;
using Streetcode.BLL.DTO.Partners.Create;

namespace Streetcode.BLL.MediatR.Partners.Create;

public class CreatePartnerRequestDtoValidator : AbstractValidator<CreatePartnerRequestDto>
{
    private const int MAXTITLELENGTH = 100;
    private const int MAXTARGETURLLENGTH = 200;
    private const int MAXDESCRIPTIONLENGTH = 450;

    public CreatePartnerRequestDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(MAXTITLELENGTH);

        RuleFor(dto => dto.LogoId)
            .GreaterThan(0);

        RuleFor(dto => dto.TargetUrl)
            .MaximumLength(MAXTARGETURLLENGTH);

        RuleFor(dto => dto.UrlTitle)
            .Must((dto, urlTitle) => urlTitle is null || dto.TargetUrl is not null || dto.TargetUrl != string.Empty);

        RuleFor(dto => dto.Description)
            .MaximumLength(MAXDESCRIPTIONLENGTH);
    }
}