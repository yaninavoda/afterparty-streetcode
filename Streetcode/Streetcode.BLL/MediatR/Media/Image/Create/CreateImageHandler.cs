﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using ImageEntity = Streetcode.BLL.Entities.Media.Images.Image;

namespace Streetcode.BLL.MediatR.Media.Image.Create
{
    public class CreateImageHandler : IRequestHandler<CreateImageCommand, Result<ImageDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public CreateImageHandler(
            IBlobService blobService,
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger)
        {
            _blobService = blobService;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ImageDto>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            string hashBlobStorageName = _blobService.SaveFileInStorage(
                request.Image.BaseFormat,
                request.Image.Title,
                request.Image.Extension!);

            var image = _mapper.Map<ImageEntity>(request.Image);

            image.BlobName = hashBlobStorageName;

            _repositoryWrapper.ImageRepository.Create(image);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            var createdImage = _mapper.Map<ImageDto>(image);

            createdImage.Base64 = _blobService.FindFileInStorageAsBase64(createdImage.BlobName!);

            if(resultIsSuccess)
            {
                return Result.Ok(createdImage);
            }
            else
            {
                string errorMsg = string.Format(
                    ErrorMessages.CreateFailed,
                    typeof(ImageEntity).Name);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
