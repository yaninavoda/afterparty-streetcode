using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Entities.Team;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.Create
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionQuery, Result<PositionDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public CreatePositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<PositionDto>> Handle(CreatePositionQuery request, CancellationToken cancellationToken)
        {
            var newPosition = _repository.PositionRepository.Create(new Positions()
            {
                Position = request.position.Position
            });

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }

            return Result.Ok(_mapper.Map<PositionDto>(newPosition));
        }
    }
}