﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using AudioEntity = Streetcode.BLL.Entities.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.GetById
{
    public class GetAudioByIdHandler : IRequestHandler<GetAudioByIdQuery, Result<AudioDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public GetAudioByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<Result<AudioDto>> Handle(GetAudioByIdQuery request, CancellationToken cancellationToken)
        {
            var audio = await _repositoryWrapper.AudioRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (audio is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByIdNotFound,
                    typeof(AudioEntity).Name,
                    request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var audioDto = _mapper.Map<AudioDto>(audio);

            audioDto.Base64 = _blobService.FindFileInStorageAsBase64(audioDto.BlobName);

            return Result.Ok(audioDto);
        }
    }
}