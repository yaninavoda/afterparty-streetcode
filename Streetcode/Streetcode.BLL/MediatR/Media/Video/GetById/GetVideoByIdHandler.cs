﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using VideoEntity = Streetcode.BLL.Entities.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.GetById
{
    public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, Result<VideoDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetVideoByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<VideoDto>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
        {
            var video = await _repositoryWrapper.VideoRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (video is null)
            {
                string errorMsg = string.Format(
                   ErrorMessages.EntityByIdNotFound,
                   typeof(VideoEntity).Name,
                   request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<VideoDto>(video));
        }
    }
}