﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllMainPage
{
    public class GetAllStreetcodesMainPageHandler : IRequestHandler<GetAllStreetcodesMainPageQuery,
        Result<IEnumerable<StreetcodeMainPageDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllStreetcodesMainPageHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StreetcodeMainPageDto>>> Handle(GetAllStreetcodesMainPageQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == BLL.Enums.StreetcodeStatus.Published,
                include: src => src.Include(item => item.Text).Include(item => item.Images));

            if (streetcodes != null)
            {
                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeMainPageDto>>(streetcodes));
            }

            const string errorMsg = "No streetcodes exist now";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
