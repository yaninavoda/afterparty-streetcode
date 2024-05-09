﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetById
{
    public class GetPartnerByIdHandler : IRequestHandler<GetPartnerByIdQuery, Result<PartnerDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetPartnerByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PartnerDto>> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
        {
            var partner = await _repositoryWrapper
                .PartnersRepository
                .GetSingleOrDefaultAsync(
                    predicate: p => p.Id == request.Id,
                    include: p => p
                        .Include(pl => pl.PartnerSourceLinks));

            if (partner is null)
            {
                string errorMsg = $"Cannot find any partner with corresponding id: {request.Id}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<PartnerDto>(partner));
        }
    }
}