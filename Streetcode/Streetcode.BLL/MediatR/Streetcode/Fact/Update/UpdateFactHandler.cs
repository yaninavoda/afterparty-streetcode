using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Repositories.Interfaces.Base;
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

        public async Task<Result<UpdateFactDto>> Handle(UpdateFactCommand command, CancellationToken cancellationToken)
        {
            UpdateFactDto request = command.UpdateRequest;

            FactEntity factEntity = _mapper.Map<FactEntity>(request);

            _repositoryWrapper.FactRepository.Update(factEntity);

            bool isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!isSuccess)
            {
                string errorMessage = UpdateFailedErrorMessage(request);

                _logger.LogError(request, errorMessage);

                return Result.Fail(errorMessage);
            }

            return Result.Ok(_mapper.Map<FactEntity, UpdateFactDto>(factEntity));
        }

        private string UpdateFailedErrorMessage(UpdateFactDto request)
        {
            return string.Format(
                ErrorMessages.UpdateFailed,
                nameof(FactEntity),
                request.Id);
        }
    }
}
