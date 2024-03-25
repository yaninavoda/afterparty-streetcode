using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.Dto.Streetcode.TextContent;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

using FactEntity = Streetcode.DAL.Entities.Streetcode.TextContent.Fact;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create
{
    public class CreateFactHandler : IRequestHandler<CreateFactCommand, Result<CreateFactDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateFactHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CreateFactDto>> Handle(CreateFactCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            if (!await IsImageExistAsync(request.ImageId))
            {
                return ImageNotFoundError(request);
            }

            if (!await IsStreetcodeExistAsync(request.StreetcodeId))
            {
                return StreetcodeNotFoundError(request);
            }

            int latestFactNumber = await GetLatestFactNumberAsync();

            var factToCreate = _mapper.Map<CreateFactDto, FactEntity>(request);
            factToCreate.Number = latestFactNumber + 1;

            var fact = _repositoryWrapper.FactRepository.Create(factToCreate);

            bool resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return FailedToCreateFactError(request);
            }

            return Result.Ok(_mapper.Map<FactEntity, CreateFactDto>(fact));
        }

        private async Task<bool> IsImageExistAsync(int imageId)
        {
            var image = await _repositoryWrapper.ImageRepository
                .GetFirstOrDefaultAsync(i => i.Id == imageId);

            return image is not null;
        }

        private Result<CreateFactDto> ImageNotFoundError(CreateFactDto request)
        {
            string errorMsg = $"Cannot find an image with corresponding id: {request.ImageId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }

        private async Task<bool> IsStreetcodeExistAsync(int streetcodeId)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetFirstOrDefaultAsync(s => s.Id == streetcodeId);

            return streetcode is not null;
        }

        private Result<CreateFactDto> StreetcodeNotFoundError(CreateFactDto request)
        {
            string errorMsg = $"Cannot find an streetcode with corresponding id: {request.StreetcodeId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }

        private Task<int> GetLatestFactNumberAsync()
        {
            return _repositoryWrapper.FactRepository.GetMaxNumberAsync();
        }

        private Result<CreateFactDto> FailedToCreateFactError(CreateFactDto request)
        {
            const string errorMsg = "Cannot create fact";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
