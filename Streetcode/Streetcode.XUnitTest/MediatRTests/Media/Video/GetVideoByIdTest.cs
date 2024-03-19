﻿using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.BLL.MediatR.Media.Video.GetById;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video
{
    public class GetVideoByIdTest
    {
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IMapper> _mockMapper;

        public GetVideoByIdTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsOkResult_WhenIdExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsVideo(id);
            MockMapperSetup(id);

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_RepositoryCallGetFirstOrDefaultAsyncOnlyOnce_WhenVideoExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsVideo(id);
            MockMapperSetup(id);

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(
                repo =>
                repo.VideoRepository.GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Video, bool>>>(),
                   It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Video>,
                   IIncludableQueryable<Streetcode.DAL.Entities.Media.Video, object>>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_MapperCallMapOnlyOnce_WhenVideoExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsVideo(id);
            MockMapperSetup(id);

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            _mockMapper.Verify(
                mapper => mapper.Map<VideoDTO>(It.IsAny<Streetcode.DAL.Entities.Media.Video>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsVideoWithCorrectId_WhenVideoExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsVideo(id);
            MockMapperSetup(id);

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnVideoDto_WhenVideoExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsVideo(id);
            MockMapperSetup(id);

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<VideoDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnFail_WhenVideoIsNotFound(int id)
        {
            // Arrange
            MockRepositorySetupReturnsNull();

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ShouldLogCorrectErrorMessage_WhenVideoIsNotFound(int id)
        {
            // Arrange
            MockRepositorySetupReturnsNull();

            var handler = new GetVideoByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            var expectedMessage = $"Cannot find a video with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetVideoByIdQuery(id), CancellationToken.None);
            var actualMessage = result.Errors.First().Message;

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        private void MockMapperSetup(int id)
        {
            _mockMapper.Setup(x => x
                .Map<VideoDTO>(It.IsAny<Streetcode.DAL.Entities.Media.Video>()))
                .Returns(new VideoDTO { Id = id });
        }

        private void MockRepositorySetupReturnsVideo(int id)
        {
            _mockRepositoryWrapper.Setup(x => x.VideoRepository
                .GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Video, bool>>>(),
                   It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Video>,
                   IIncludableQueryable<Streetcode.DAL.Entities.Media.Video, object>>>()))
                .ReturnsAsync(new Streetcode.DAL.Entities.Media.Video { Id = id });
        }

        private void MockRepositorySetupReturnsNull()
        {
            _mockRepositoryWrapper.Setup(x => x.VideoRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Video, bool>>>(),
                    It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Video>,
                IIncludableQueryable<Streetcode.DAL.Entities.Media.Video, object>>>()))
                .ReturnsAsync((IEnumerable<Streetcode.DAL.Entities.Media.Video>?)null);
        }
    }

}
