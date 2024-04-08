using FluentValidation;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.DTO.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.Create
{
    public class CreateVideoRequestDtoValidator : AbstractValidator<CreateVideoRequestDto>
    {
        private const int MAXDESCRPTIONLENGTH = 400;

        public CreateVideoRequestDtoValidator()
        {
            RuleFor(dto => dto.Description)
                .MaximumLength(MAXDESCRPTIONLENGTH);

            RuleFor(dto => dto.Url)
                .Must(IsValidYoutubeUrl)
                .WithMessage("Only youtube.com links are accepted.");

            RuleFor(dto => dto.StreetcodeId)
                .GreaterThan(0);
        }

        private bool IsValidYoutubeUrl(string url)
        {
            return url.Contains("youtube.com");
        }
    }
}
