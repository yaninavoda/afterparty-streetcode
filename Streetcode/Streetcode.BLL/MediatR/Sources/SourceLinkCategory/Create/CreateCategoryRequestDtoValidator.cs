using FluentValidation;
using Streetcode.BLL.Dto.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

public class CreateCategoryRequestDtoValidator : AbstractValidator<CreateCategoryRequestDto>
{
    private readonly int _maxTitleLength;
    private readonly int _maxTextLength;

    public CreateCategoryRequestDtoValidator()
    {
        _maxTitleLength = 100;
        _maxTextLength = 4000;

        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(_maxTitleLength);

        RuleFor(dto => dto.Text)
            .MaximumLength(_maxTextLength);

        RuleFor(dto => dto.ImageId)
            .GreaterThan(0);

        RuleFor(dto => dto.StreetcodeId)
            .GreaterThan(0);
    }
}
