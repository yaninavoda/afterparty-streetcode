using FluentValidation;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.MediatR.Media.Art.Create;

namespace Streetcode.BLL.MediatR.Media.Art.CreateMultiple;

public class CreateArtsValidator : AbstractValidator<CreateArtsRequestDto>
{
    public CreateArtsValidator()
    {
        RuleForEach(dto => dto.Arts).SetValidator(new CreateArtRequestDtoValidator());
    }
}
