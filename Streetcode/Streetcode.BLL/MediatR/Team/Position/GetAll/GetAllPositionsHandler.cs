﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<IEnumerable<PositionDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllPositionsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PositionDto>>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            var positions = await _repositoryWrapper
                .PositionRepository
                .GetAllAsync();

            if (positions is null)
            {
                const string errorMsg = $"Cannot find any positions";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PositionDto>>(positions));
        }
    }
}
