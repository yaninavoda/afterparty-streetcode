using MediatR;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

using BLL.Dto.Sources;
using DAL.Entities.Sources;
using FluentResults;

public record CreateCategoryCommand(SourceLinkCategoryDto Category) : IRequest<Result<SourceLinkCategory>>;