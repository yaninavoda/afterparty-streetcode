using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.AdditionalContent.Tag;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle
{
    public record GetTagByTitleQuery(string Title) : IRequest<Result<TagDto>>;
}
