﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using ImageEntity = Streetcode.BLL.Entities.Media.Images.Image;

namespace Streetcode.BLL.MediatR.Media.Image.GetById
{
    public class GetImageByIdHandler : IRequestHandler<GetImageByIdQuery, Result<ImageDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public GetImageByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<Result<ImageDto>> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(
                f => f.Id == request.Id,
                include: q => q.Include(i => i.ImageDetails) !);

            if (image is null)
            {
                string errorMsg = string.Format(
                   ErrorMessages.EntityByIdNotFound,
                   typeof(ImageEntity).Name,
                   request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var imageDto = _mapper.Map<ImageDto>(image);
            if(imageDto.BlobName != null)
            {
                imageDto.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
            }

            return Result.Ok(imageDto);
        }
    }
}