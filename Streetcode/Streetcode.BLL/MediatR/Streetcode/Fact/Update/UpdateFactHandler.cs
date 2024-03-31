using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Media.Images;
using AutoMapper;

using FactEntity = Streetcode.DAL.Entities.Streetcode.TextContent.Fact;
namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update
{
    public class UpdateFactHandler : IRequestHandler<UpdateFactCommand, Result<UpdateFactDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public UpdateFactHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<UpdateFactDto>> Handle(UpdateFactCommand query, CancellationToken cancellationToken)
        {
            var request = query.UpdateRequest;

            var fact = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);

            var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(i => i.Id == request.ImageId);

            if (fact is null)
            {
                FactNotFoundError(request);
            }

            if (image is null)
            {
                ImageNotFoundError(request);
            }

            fact.ImageId = request.ImageId;

            fact.Title = request.Title;

            fact.FactContent = request.FactContent;

            _repositoryWrapper.FactRepository.Update(fact);

            var isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!isSuccess)
            {
                UpdateFailed(request);
            }

            return Result.Ok(_mapper.Map<UpdateFactDto>(fact));
        }

        private Result<UpdateFactDto> FactNotFoundError(UpdateFactDto request)
        {
            string errorMsg = string.Format(
                ErrorMessages.EntityByIdNotFound,
                nameof(Fact),
                request.Id);
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }

        private Result<UpdateFactDto> ImageNotFoundError(UpdateFactDto request)
        {
            string errorMsg = string.Format(
                ErrorMessages.EntityByIdNotFound,
                nameof(Image),
                request.ImageId);
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }

        private Result<UpdateFactDto> UpdateFailed(UpdateFactDto request)
        {
            string errorMsg = string.Format(
                ErrorMessages.UpdateFailed,
                nameof(Fact),
                request.ImageId);
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
