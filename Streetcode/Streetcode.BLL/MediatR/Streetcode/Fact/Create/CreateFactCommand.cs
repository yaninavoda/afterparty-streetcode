﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create
{
    public record CreateFactCommand(CreateFactDto CreateFact) : IRequest<Result<CreateFactDto>>
    {
    }
}
