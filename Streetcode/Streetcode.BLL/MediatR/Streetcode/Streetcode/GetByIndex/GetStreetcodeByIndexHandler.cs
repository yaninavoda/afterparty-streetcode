﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex
{
    public class GetStreetcodeByIndexHandler : IRequestHandler<GetStreetcodeByIndexQuery, Result<StreetcodeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetStreetcodeByIndexHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeDto>> Handle(GetStreetcodeByIndexQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
                predicate: st => st.Index == request.Index,
                include: source => source.Include(l => l.Tags));

            if (streetcode is null)
            {
                string errorMsg = $"Cannot find any streetcode with corresponding index: {request.Index}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<StreetcodeDto>(streetcode));
        }
    }
}