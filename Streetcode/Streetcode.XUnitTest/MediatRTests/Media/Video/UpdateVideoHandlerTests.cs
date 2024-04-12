using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.Update;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using AutoMapper;
using Streetcode.BLL.Dto.Media.Video;

using VideoEntity = Streetcode.DAL.Entities.Media.Video;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video.Update
{
    public class UpdateVideoHandlerTests
    {
        private const int FAILURERESULT = -1;
        private const int EXISTINGSTREETCODEID = 1;

        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public UpdateVideoHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_Should_ReturnFail_WhenSaveChangesAsyncWorkedIncorrect()
        {
            // Arrange
            var request = new UpdateVideoRequestDto(1, "Title", "Description", "https://www.youtube.com", EXISTINGSTREETCODEID);
            SetupMockRepository(request, FAILURERESULT);
            SetupMockMapper();
            var command = new UpdateVideoCommand(request);
            var handler = new UpdateVideoHandler(_mockRepositoryWrapper.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task Handle_Should_LogError_WhenSaveChangesAsyncFails()
        {
            // Arrange
            var request = new UpdateVideoRequestDto(1, "Title", "Description", "https://www.youtube.com", EXISTINGSTREETCODEID);
            SetupMockRepository(request, FAILURERESULT);
            SetupMockMapper();
            var command = new UpdateVideoCommand(request);
            var handler = new UpdateVideoHandler(_mockRepositoryWrapper.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _mockLogger.Verify(x => x.LogError(request, It.IsAny<string>()), Times.Once);
        }

        private void SetupMockRepository(UpdateVideoRequestDto request, int resultCase)
        {
            var videoEntity = new VideoEntity();
            _mockMapper.Setup(x => x.Map<VideoEntity>(request)).Returns(videoEntity);

            _mockRepositoryWrapper.Setup(x => x.VideoRepository.Update(videoEntity));
            _mockRepositoryWrapper.Setup(x => x.SaveChangesAsync()).ReturnsAsync(resultCase);
        }

        private void SetupMockMapper()
        {
            _mockMapper.Setup(x => x.Map<VideoDto>(It.IsAny<VideoEntity>()))
                .Returns(new VideoDto());
        }
    }
}