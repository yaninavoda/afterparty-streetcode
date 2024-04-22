using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.BlobStorageService
{
    public sealed class AzureBlobService : IBlobService
    {
        public const string AzureStorageConnectionString = nameof(AzureStorageConnectionString);
        public const string AzureStorageContainerName = nameof(AzureStorageContainerName);

        public const string DefaultContainerName = "images";

        private readonly string _connectionString;
        private readonly string _containerName;

        private readonly ILoggerService _loggerService;
        private readonly IRepositoryWrapper? _repositoryWrapper;

        public AzureBlobService(IConfiguration configuration, ILoggerService loggerService, IRepositoryWrapper? repositoryWrapper = null)
        {
            _connectionString = configuration.GetConnectionString(AzureStorageConnectionString)
               ?? throw new InvalidOperationException($"Connection string '{AzureStorageConnectionString}' not found.");

            _containerName = configuration[AzureStorageContainerName] ?? DefaultContainerName;

            _loggerService = loggerService;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task CleanBlobStorageAsync(CancellationToken cancellationToken = default)
        {
            if (_repositoryWrapper == null)
            {
                return;
            }

            var existingImagesInDatabase = await _repositoryWrapper.ImageRepository.GetAllAsync();
            var existingAudiosInDatabase = await _repositoryWrapper.AudioRepository.GetAllAsync();

            var existingMedia = existingImagesInDatabase.Select(image => image.BlobName)
                .Union(existingAudiosInDatabase.Select(audio => audio.BlobName));

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                if (!existingMedia.Contains(blobItem.Name))
                {
                    _loggerService.LogInformation($"Deleting {blobItem.Name}...");
                    await containerClient.DeleteBlobAsync(blobItem.Name, cancellationToken: cancellationToken);
                }
            }
        }

        public string SaveFileInStorage(string base64, string name, string mimeType)
        {
            var bytes = Convert.FromBase64String(base64);
            var blobName = GenerateBlobName(name);

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(bytes))
            {
                blobClient.Upload(stream);
            }

            return blobName;
        }

        public async Task<string> SaveFileInStorageAsync(string base64, string name, string mimeType, CancellationToken cancellationToken = default)
        {
            var bytes = Convert.FromBase64String(base64);
            var blobName = GenerateBlobName(name);

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(bytes))
            {
                await blobClient.UploadAsync(stream, true, cancellationToken: cancellationToken);
            }

            return blobName;
        }

        public void DeleteFileInStorage(string name)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);
            blobClient.DeleteIfExists();
        }

        public async Task DeleteFileInStorageAsync(string name, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public byte[] FindFileInStorageAsBytes(string name)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            using (var stream = new MemoryStream())
            {
                blobClient.DownloadTo(stream);
                return stream.ToArray();
            }
        }

        public async Task<byte[]> FindFileInStorageAsBytesAsync(string name, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream, cancellationToken);
                return stream.ToArray();
            }
        }

        public string FindFileInStorageAsBase64(string name)
        {
            var bytes = FindFileInStorageAsBytes(name);
            return Convert.ToBase64String(bytes);
        }

        public MemoryStream FindFileInStorageAsMemoryStream(string name)
        {
            var bytes = FindFileInStorageAsBytes(name);
            return new MemoryStream(bytes);
        }

        public string UpdateFileInStorage(string previousBlobName, string base64Format, string newBlobName, string extension)
        {
            DeleteFileInStorage(previousBlobName);
            return SaveFileInStorage(base64Format, newBlobName, extension);
        }

        private static string GenerateBlobName(string fileName)
        {
            var guid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);
            return $"{guid}{extension}";
        }
    }
}
