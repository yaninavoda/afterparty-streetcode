using FluentValidation;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create
{
    public class CreateStreetcodeRequestDtoValidator : AbstractValidator<CreateStreetcodeRequestDto>
    {
        public CreateStreetcodeRequestDtoValidator()
        {
            RuleFor(x => x.StreetcodeType)
                .IsInEnum()
                .WithMessage("Invalid StreetcodeType.");

            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Title must be less than 100 characters.");

            RuleFor(x => x.FirstName)
                .MaximumLength(50)
                .WithMessage("FirstName must be less than 50 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .WithMessage("LastName must be less than 50 characters.");

            RuleFor(x => x.Alias)
                .MaximumLength(33)
                .WithMessage("Alias must be less than 33 characters.");

            RuleFor(x => x.Teaser)
                .Must(BeValidTeaser)
                .WithMessage("Teaser must be less than 450 characters or 400 characters if it contains a paragraph.");

            RuleFor(x => x.TransliterationUrl)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("TransliterationUrl must be less than 100 characters.")
                .Matches(@"^[a-z0-9\-]+$")
                .WithMessage("Invalid URL. Only small latin alphabet letters, special symbol '-' and numbers are allowed.");
        }

        private bool BeValidTeaser(string teaser)
        {
            if (string.IsNullOrEmpty(teaser))
            {
                return true;
            }

            var limit = teaser.Contains("\n") ? 400 : 450;
            return teaser.Length <= limit;
        }
    }
}
