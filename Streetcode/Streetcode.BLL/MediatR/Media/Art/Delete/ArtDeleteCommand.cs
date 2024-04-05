using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Art.Delete;

public record ArtDeleteCommand(int Id) : IRequest<Result<Unit>>;