﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.AdditionalContent.Tag;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
    public record CreateTagQuery(CreateTagDto tag) : IRequest<Result<TagDto>>;
}
