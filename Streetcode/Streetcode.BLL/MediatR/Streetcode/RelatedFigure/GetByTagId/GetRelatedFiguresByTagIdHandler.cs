﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.Entities.Streetcode.Types;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
  internal class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetRelatedFiguresByTagIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<RelatedFigureDto>>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: sc => sc.Status == BLL.Enums.StreetcodeStatus.Published &&
                  sc.Tags.Select(t => t.Id).Any(tag => tag == request.tagId),
                include: scl => scl
                    .Include(sc => sc.Images)
                    .Include(sc => sc.Tags));

            if (streetcodes is null)
            {
                string errorMsg = $"Cannot find any streetcode with corresponding tagid: {request.tagId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<RelatedFigureDto>>(streetcodes));
        }
    }
}
