﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using ImageEntity = Streetcode.BLL.Entities.Media.Images.Image;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll
{
    public class GetAllImagesHandler : IRequestHandler<GetAllImagesQuery, Result<IEnumerable<ImageDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;

        public GetAllImagesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ImageDto>>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            var images = await _repositoryWrapper.ImageRepository.GetAllAsync();

            if (images is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntitiesNotFound,
                    typeof(ImageEntity).Name);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var imageDtos = _mapper.Map<IEnumerable<ImageDto>>(images);

            foreach (var image in imageDtos)
            {
                try
                {
                    image.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
                }
                catch (Azure.RequestFailedException ex)
                {
                    if (ex.ErrorCode == "BlobNotFound")
                    {
                        continue;
                    }

                    throw;
                }
            }

            return Result.Ok(imageDtos.Where(x => !string.IsNullOrEmpty(x.Base64)).AsEnumerable());
        }
    }
}