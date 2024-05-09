using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllMainPage
{
    public record GetAllStreetcodesMainPageQuery : IRequest<Result<IEnumerable<StreetcodeMainPageDto>>>;
}
