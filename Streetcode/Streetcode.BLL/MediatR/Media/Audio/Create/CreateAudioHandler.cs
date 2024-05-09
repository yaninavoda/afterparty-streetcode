using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using AudioEntity = Streetcode.BLL.Entities.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.Create
{
    public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, Result<AudioDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public CreateAudioHandler(
            IBlobService blobService,
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger)
        {
            _blobService = blobService;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<AudioDto>> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
        {
            string hashBlobStorageName = _blobService.SaveFileInStorage(
                request.Audio.BaseFormat,
                request.Audio.Title,
                request.Audio.Extension);

            var audio = _mapper.Map<AudioEntity>(request.Audio);

            audio.BlobName = hashBlobStorageName;

            _repositoryWrapper.AudioRepository.Create(audio);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            var createdAudio = _mapper.Map<AudioDto>(audio);

            if(resultIsSuccess)
            {
                return Result.Ok(createdAudio);
            }
            else
            {
                string errorMsg = string.Format(
                    ErrorMessages.CreateFailed,
                    typeof(AudioEntity).Name);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
