using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Sources;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Dto.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Update;

public class UpdateCategoryContentHandler : IRequestHandler<UpdateCategoryContentCommand, Result<StreetcodeCategoryContentDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;
    public UpdateCategoryContentHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<StreetcodeCategoryContentDto>> Handle(UpdateCategoryContentCommand command, CancellationToken cancellationToken)
    {
        CategoryContentUpdateDto request = command.CategoryContentUpdateDto;

        StreetcodeCategoryContent content = _mapper.Map<StreetcodeCategoryContent>(request);

        _repositoryWrapper.StreetcodeCategoryContentRepository.Update(content);

        bool isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!isSuccess)
        {
            string errorMessage = UpdateFailedErrorMessage(request);

            _logger.LogError(command, errorMessage);

            return Result.Fail(errorMessage);
        }

        return Result.Ok(_mapper.Map<StreetcodeCategoryContentDto>(content));
    }

    private string UpdateFailedErrorMessage(CategoryContentUpdateDto request)
    {
        return string.Format(
            ErrorMessages.UpdateFailed,
            nameof(StreetcodeCategoryContent),
            request.StreetcodeId);
    }
}