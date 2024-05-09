﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using AudioEntity = Streetcode.BLL.Entities.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.Delete
{
    public class DeleteAudioHandler : IRequestHandler<DeleteAudioCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public DeleteAudioHandler(IRepositoryWrapper repositoryWrapper, IBlobService blobService, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteAudioCommand request, CancellationToken cancellationToken)
        {
            var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

            if (audio is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByCategoryIdNotFound,
                    typeof(AudioEntity).Name,
                    request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.AudioRepository.Delete(audio);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (resultIsSuccess)
            {
                _blobService.DeleteFileInStorage(audio.BlobName);
            }

            if (resultIsSuccess)
            {
                _logger?.LogInformation($"DeleteAudioCommand handled successfully");
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = string.Format(
                    ErrorMessages.DeleteFailed,
                    typeof(AudioEntity).Name);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
