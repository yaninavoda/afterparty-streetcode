﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAll
{
    public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<IEnumerable<PartnerDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllPartnersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PartnerDto>>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
        {
            var partners = await _repositoryWrapper
                .PartnersRepository
                .GetAllAsync(
                    include: p => p
                        .Include(pl => pl.PartnerSourceLinks)
                        .Include(p => p.Streetcodes));

            if (partners is null)
            {
                const string errorMsg = $"Cannot find any partners";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PartnerDto>>(partners));
        }
    }
}
