﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<SourceLinkCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public GetCategoryByIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<Result<SourceLinkCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var srcCategories = await _repositoryWrapper
                .SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    predicate: sc => sc.Id == request.Id,
                    include: scl => scl
                        .Include(sc => sc.StreetcodeCategoryContents)
                        .Include(sc => sc.Image)!);

            if (srcCategories is null)
            {
                string errorMsg = $"Cannot find any srcCategory by the corresponding id: {request.Id}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var mappedSrcCategories = _mapper.Map<SourceLinkCategoryDto>(srcCategories);

            mappedSrcCategories.Image.Base64 = _blobService.FindFileInStorageAsBase64(mappedSrcCategories.Image.BlobName);

            return Result.Ok(mappedSrcCategories);
        }
    }
}