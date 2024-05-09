﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Team;

namespace Streetcode.BLL.MediatR.Team.Position.Create
{
    public record CreatePositionQuery(PositionDto position) : IRequest<Result<PositionDto>>;
}
