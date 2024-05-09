﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Video;
using Streetcode.BLL.Entities.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId
{
    public class GetVideoByStreetcodeIdHandler : IRequestHandler<GetVideoByStreetcodeIdQuery, Result<VideoDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetVideoByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<VideoDto>> Handle(GetVideoByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var video = await _repositoryWrapper.VideoRepository
                .GetFirstOrDefaultAsync(video => video.StreetcodeId == request.StreetcodeId);
            if(video == null)
            {
                StreetcodeContent? streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(x => x.Id == request.StreetcodeId);
                if (streetcode is null)
                {
                    string errorMsg = $"Streetcode with id: {request.StreetcodeId} doesn`t exist";
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }

            NullResult<VideoDto> result = new NullResult<VideoDto>();
            result.WithValue(_mapper.Map<VideoDto>(video));
            return result;
        }
    }
}