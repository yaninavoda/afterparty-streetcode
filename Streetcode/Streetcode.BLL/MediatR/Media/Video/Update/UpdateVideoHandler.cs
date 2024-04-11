using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Video;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Repositories.Interfaces.Base;

using VideoEntity = Streetcode.DAL.Entities.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.Update
{
    public class UpdateVideoHandler : IRequestHandler<UpdateVideoCommand, Result<VideoDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public UpdateVideoHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<VideoDto>> Handle(UpdateVideoCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var video = _mapper.Map<UpdateVideoRequestDto, VideoEntity>(request);

            _repositoryWrapper.VideoRepository.Update(video);

            bool resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                string errorMessage = string.Format(
                ErrorMessages.UpdateFailed,
                typeof(VideoEntity).Name,
                request.StreetcodeId);

                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var response = new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Description = video.Description,
                Url = video.Url,
                StreetcodeId = video.StreetcodeId,
            };

            return Result.Ok(response);
        }
    }
}
