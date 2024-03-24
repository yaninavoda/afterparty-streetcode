using MediatR;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

using DAL.Entities.Sources;
using FluentResults;
using DAL.Repositories.Interfaces.Base;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces.Logging;
using BLL.Dto.Sources;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<SourceLinkCategory>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;
    public CreateCategoryHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<SourceLinkCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = _mapper.Map<SourceLinkCategory>(request.Category);

        if (newCategory is null)
        {
            string errorMsg = "Cannot convert null to category";
            _logger.LogError(request, errorMsg);

            return Result.Fail(new Error(errorMsg));
        }

        if (newCategory.ImageId == 0)
        {
            string errorMsg = "Cannot create category without image";
            _logger.LogError(request, errorMsg);

            return Result.Fail(new Error(errorMsg));
        }

        var entity = _repositoryWrapper.SourceCategoryRepository.Create(newCategory);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (resultIsSuccess)
        {
            return Result.Ok(entity);
        }
        else
        {
            string errorMsg = "Failed to create category";
            _logger.LogError(request, errorMsg);

            return Result.Fail(new Error(errorMsg));
        }
    }
}
