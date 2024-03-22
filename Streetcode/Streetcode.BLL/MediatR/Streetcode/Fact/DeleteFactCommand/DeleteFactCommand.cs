using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.DeleteFactCommand;

public record DeleteFactCommand(int Id) : IRequest<Result<Unit>>;
