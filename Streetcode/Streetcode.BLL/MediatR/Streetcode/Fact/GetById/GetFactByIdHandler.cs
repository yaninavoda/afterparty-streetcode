﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById
{
    public class GetFactByIdHandler : IRequestHandler<GetFactByIdQuery, Result<FactDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetFactByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<FactDto>> Handle(GetFactByIdQuery request, CancellationToken cancellationToken)
        {
            var fact = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (fact is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByIdNotFound,
                    nameof(Fact),
                    request.Id);

                _logger.LogError(request, errorMsg);

                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<FactDto>(fact));
        }
    }
}