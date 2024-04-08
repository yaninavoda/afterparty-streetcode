using MediatR;
using FluentResults;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Media.Art;

using StreetcodeArtEntity = Streetcode.DAL.Entities.Streetcode.StreetcodeArt;

namespace Streetcode.BLL.MediatR.Media.StreetcodeArt.Delete;

public class DeleteStreetcodeArtHandler : IRequestHandler<DeleteStreetcodeArtCommand, Result<DeleteStreetcodeArtResponeDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    public DeleteStreetcodeArtHandler(IRepositoryWrapper repository, ILoggerService logger)
    {
        _repositoryWrapper = repository;
        _logger = logger;
    }

    public async Task<Result<DeleteStreetcodeArtResponeDto>> Handle(DeleteStreetcodeArtCommand command, CancellationToken cancellationToken)
    {
        var request = command.DeleteStreetcodeArtRequestDto;

        var streetcodeArt = await _repositoryWrapper.StreetcodeArtRepository.GetFirstOrDefaultAsync(x => x.ArtId == request.ArtId
        && x.StreetcodeId == request.StreetcodeId);

        if (streetcodeArt is null)
        {
            return StreetcodeArtNotFoundError(request.StreetcodeId);
        }

        _repositoryWrapper.StreetcodeArtRepository.Delete(streetcodeArt);

        bool isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!isSuccess)
        {
            return DeleteFailed(request.StreetcodeId);
        }

        var response = new DeleteStreetcodeArtResponeDto(true);

        return Result.Ok(response);
    }

    private Result<DeleteStreetcodeArtResponeDto> StreetcodeArtNotFoundError(int id)
    {
        string errorMessage = string.Format(
            ErrorMessages.EntityByIdNotFound,
            nameof(StreetcodeArtEntity),
            id);

        _logger.LogError(id, errorMessage);

        return Result.Fail(errorMessage);
    }

    private Result<DeleteStreetcodeArtResponeDto> DeleteFailed(int id)
    {
        string errorMessage = string.Format(
            ErrorMessages.DeleteFailed,
            nameof(StreetcodeArtEntity),
            id);

        _logger.LogError(id, errorMessage);

        return Result.Fail(errorMessage);
    }
}
