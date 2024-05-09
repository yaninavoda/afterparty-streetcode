﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoriesByStreetcodeId
{
    public record GetCategoriesByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<SourceLinkCategoryDto>>>;
}