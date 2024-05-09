﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using AudioEntity = Streetcode.BLL.Entities.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio
{
    public class GetBaseAudioHandler : IRequestHandler<GetBaseAudioQuery, Result<MemoryStream>>
    {
        private readonly IBlobService _blobStorage;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetBaseAudioHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _blobStorage = blobService;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<MemoryStream>> Handle(GetBaseAudioQuery request, CancellationToken cancellationToken)
        {
            var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

            if (audio is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByIdNotFound,
                    typeof(AudioEntity).Name,
                    request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return _blobStorage.FindFileInStorageAsMemoryStream(audio.BlobName);
        }
    }
}
