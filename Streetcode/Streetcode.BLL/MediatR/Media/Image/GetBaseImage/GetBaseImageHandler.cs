using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using ImageEntity = Streetcode.BLL.Entities.Media.Images.Image;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseImage
{
    public class GetBaseImageHandler : IRequestHandler<GetBaseImageQuery, Result<MemoryStream>>
    {
        private readonly IBlobService _blobStorage;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetBaseImageHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _blobStorage = blobService;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<MemoryStream>> Handle(GetBaseImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

            if (image is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByIdNotFound,
                    typeof(ImageEntity).Name,
                    request.Id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return _blobStorage.FindFileInStorageAsMemoryStream(image.BlobName);
        }
    }
}
