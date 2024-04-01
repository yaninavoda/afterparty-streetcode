using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<SourceLinkCategoryDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateCategoryHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<SourceLinkCategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);

        _repositoryWrapper.SourceCategoryRepository.Create(newCategory);

        await _repositoryWrapper.SaveChangesAsync();

        var streetcodeCategoryContent = new StreetcodeCategoryContent()
        {
            StreetcodeId = request.Category.StreetcodeId,
            SourceLinkCategoryId = newCategory.Id,
            Text = request.Category.Text
        };

        newCategory.StreetcodeCategoryContents.Add(streetcodeCategoryContent);

        var isSuccessful = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (isSuccessful)
        {
            var categoryDto = _mapper.Map<SourceLinkCategoryDto>(newCategory);

            return Result.Ok(categoryDto);
        }
        else
        {
            string errorMsg = "Failed to create category";
            _logger.LogError(request, errorMsg);

            return Result.Fail(new Error(errorMsg));
        }
    }
}
