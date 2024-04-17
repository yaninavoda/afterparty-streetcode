using FluentValidation;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create
{
    public class CreateStreetcodeRequestDtoValidator : AbstractValidator<CreateStreetcodeRequestDto>
    {
        private const int MINIMUMLENGTH = 1;
        private const int MAXTITLELENGTH = 100;
        private const int MAXFIRSTNAMELENGTH = 50;
        private const int MAXLASTNAMELENGTH = 50;
        private const int MAXALIASLENGTH = 33;
        private const int MAXTEASERLENGTH = 450;
        private const int MAXPARAGRAPHTEASERLENGTH = 400;
        private const int MAXTRANSLITERATIONURLLENGTH = 100;

        public CreateStreetcodeRequestDtoValidator()
        {
            RuleFor(x => x.StreetcodeType)
                .IsInEnum()
                .WithMessage("Invalid StreetcodeType.");

            RuleFor(x => x.Title)
                .MinimumLength(MINIMUMLENGTH)
                .MaximumLength(MAXTITLELENGTH);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(MINIMUMLENGTH)
                .MaximumLength(MAXFIRSTNAMELENGTH);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(MINIMUMLENGTH)
                .MaximumLength(MAXLASTNAMELENGTH);

            RuleFor(x => x.Alias)
                .NotEmpty()
                .MinimumLength(MINIMUMLENGTH)
                .MaximumLength(MAXALIASLENGTH);

            RuleFor(x => x.Teaser)
               .Must(teaser => string.IsNullOrEmpty(teaser) || IsValidTeaser(teaser))
               .WithMessage($"Teaser must be less than {MAXTEASERLENGTH} characters or {MAXPARAGRAPHTEASERLENGTH} characters if it contains a paragraph.");

            RuleFor(x => x.TransliterationUrl)
                .NotEmpty()
                .MaximumLength(MAXTRANSLITERATIONURLLENGTH)
                .Matches(@"^[a-z0-9\-]+$");
        }

        private bool IsValidTeaser(string teaser)
        {
            var limit = teaser.Contains("\n") ? MAXPARAGRAPHTEASERLENGTH : MAXTEASERLENGTH;
            return teaser.Length <= limit;
        }
    }
}
