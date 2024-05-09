﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Dto.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Сreate
{
    public record CreateRelatedFigureCommand(int ObserverId, int TargetId) : IRequest<Result<Unit>>;
}
