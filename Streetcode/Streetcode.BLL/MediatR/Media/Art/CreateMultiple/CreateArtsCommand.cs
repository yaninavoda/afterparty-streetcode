using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.CreateMultiple;

public record CreateArtsCommand(IEnumerable<CreateArtRequestDto> Arts)
    : IRequest<Result<IEnumerable<CreateArtResponseDto>>>;