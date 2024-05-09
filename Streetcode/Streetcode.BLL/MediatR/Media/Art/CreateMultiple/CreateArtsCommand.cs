using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.CreateMultiple;

public record CreateArtsCommand(CreateArtsRequestDto Arts)
    : IRequest<Result<IEnumerable<CreateArtResponseDto>>>;