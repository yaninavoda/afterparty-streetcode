using FluentValidation;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Resources.Errors.ValidationErrors;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update;

public class UpdateFactDtoValidator : AbstractValidator<UpdateFactDto>
{
    private readonly int _maxTitleLength;
    private readonly int _maxFactContentLength;

    public UpdateFactDtoValidator()
    {
        _maxTitleLength = 68;
        _maxFactContentLength = 600;

        RuleFor(dto => dto.Id)
            .GreaterThan(0);

        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(_maxTitleLength);

        RuleFor(dto => dto.FactContent)
            .NotEmpty()
            .MaximumLength(_maxFactContentLength);

        RuleFor(dto => dto.ImageId)
            .GreaterThan(0);

        RuleFor(dto => dto.StreetcodeId)
            .GreaterThan(0);
    }
}