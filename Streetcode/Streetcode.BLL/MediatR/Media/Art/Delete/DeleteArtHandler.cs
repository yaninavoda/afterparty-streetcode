using MediatR;
using FluentResults;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

using ArtEntity = Streetcode.DAL.Entities.Media.Images.Art;

namespace Streetcode.BLL.MediatR.Media.Art.Delete;

public class DeleteArtHandler : IRequestHandler<DeleteArtCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    public DeleteArtHandler(IRepositoryWrapper repository, ILoggerService logger)
    {
        _repositoryWrapper = repository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteArtCommand request, CancellationToken cancellationToken)
    {
        int id = request.Id;

        var art = await _repositoryWrapper.ArtRepository.GetFirstOrDefaultAsync(x => x.Id == id);

        if (art is null)
        {
            return ArtNotFoundError(id);
        }

        _repositoryWrapper.ArtRepository.Delete(art);

        bool isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!isSuccess)
        {
            return DeleteFailed(id);
        }

        return Result.Ok(Unit.Value);
    }

    private Result<Unit> ArtNotFoundError(int id)
    {
        string errorMessage = string.Format(
            ErrorMessages.EntityByIdNotFound,
            nameof(ArtEntity),
            id);

        _logger.LogError(id, errorMessage);

        return Result.Fail(errorMessage);
    }

    private Result<Unit> DeleteFailed(int id)
    {
        string errorMessage = string.Format(
            ErrorMessages.DeleteFailed,
            nameof(ArtEntity),
            id);

        _logger.LogError(id, errorMessage);

        return Result.Fail(errorMessage);
    }
}
