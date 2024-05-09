﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Team;
using Streetcode.BLL.Entities.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAllMain
{
    public class GetAllMainTeamHandler : IRequestHandler<GetAllMainTeamQuery, Result<IEnumerable<TeamMemberDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllMainTeamHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TeamMemberDto>>> Handle(GetAllMainTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repositoryWrapper
                .TeamRepository
                .GetAllAsync(include: x => x.Where(x => x.IsMain).Include(x => x.Positions).Include(x => x.TeamMemberLinks));

            if (team is null)
            {
                string errorMsg = string.Format(ErrorMessages.EntitiesNotFound, nameof(TeamMember));
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TeamMemberDto>>(team));
        }
    }
}