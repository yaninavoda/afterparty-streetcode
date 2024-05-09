﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId
{
    public class GetCategoryContentByStreetcodeIdHandler : IRequestHandler<GetCategoryContentByStreetcodeIdQuery, Result<StreetcodeCategoryContentDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetCategoryContentByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeCategoryContentDto>> Handle(GetCategoryContentByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            if((await _repositoryWrapper.StreetcodeRepository
                .GetFirstOrDefaultAsync(s => s.Id == request.streetcodeId)) == null)
            {
                string errorMsg = $"No such streetcode with id = {request.streetcodeId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var streetcodeContent = await _repositoryWrapper.StreetcodeCategoryContentRepository
                .GetFirstOrDefaultAsync(
                    sc => sc.StreetcodeId == request.streetcodeId && sc.SourceLinkCategoryId == request.categoryId);

            if (streetcodeContent == null)
            {
                string errorMsg = "The streetcode content is null";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<StreetcodeCategoryContentDto>(streetcodeContent));
        }
    }
}
