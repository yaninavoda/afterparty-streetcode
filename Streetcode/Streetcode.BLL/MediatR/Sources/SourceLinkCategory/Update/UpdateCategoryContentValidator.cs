using FluentValidation;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Update;

public class UpdateCategoryContentValidator : AbstractValidator<CategoryContentUpdateDto>
{
    private readonly int _maxTextLength;

    public UpdateCategoryContentValidator()
    {
        _maxTextLength = 4000;

        RuleFor(dto => dto.Text)
            .NotEmpty()
            .MaximumLength(_maxTextLength);

        RuleFor(dto => dto.SourceLinkCategoryId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(dto => dto.StreetcodeId)
            .GreaterThan(0);
    }
}
