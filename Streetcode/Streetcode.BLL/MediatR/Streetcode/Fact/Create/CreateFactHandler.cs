using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.Dto.Streetcode.TextContent;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

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

            if (!await IsImageExist(request.ImageId))
            {
                return ImageNotFoundError(request);
            }

            if (!await IsStreetcodeExist(request.StreetcodeId))
            {
                return StreetcodeNotFoundError(request);
            }

            int latestFactNumber = await GetLatestFactNumber();

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

        private async Task<bool> IsImageExist(int imageId)
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

        private async Task<bool> IsStreetcodeExist(int streetcodeId)
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

        private async Task<int> GetLatestFactNumber()
        {
            var latestFact = await _repositoryWrapper.FactRepository
                .GetAllAsync(
                    predicate: null,
                    include: null);

            var sortedFacts = latestFact.OrderByDescending(f => f.Number);

            return sortedFacts.FirstOrDefault()?.Number ?? 0;
        }

        private Result<CreateFactDto> FailedToCreateFactError(CreateFactDto request)
        {
            const string errorMsg = "Cannot create fact";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
