﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.RepositoryInterfaces.Base;
using Streetcode.BLL.Resources.Errors;
using ArtEntity = Streetcode.BLL.Entities.Media.Images.Art;

namespace Streetcode.BLL.MediatR.Media.Art.GetAll
{
    public class GetAllArtsHandler : IRequestHandler<GetAllArtsQuery, Result<IEnumerable<ArtDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllArtsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ArtDto>>> Handle(GetAllArtsQuery request, CancellationToken cancellationToken)
        {
            var arts = await _repositoryWrapper.ArtRepository.GetAllAsync();

            if (arts is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntitiesNotFound,
                    typeof(ArtEntity).Name);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<ArtDto>>(arts));
        }
    }
}