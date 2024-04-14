﻿using System.Linq.Expressions;
using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Video.Delete;
using Streetcode.BLL.MediatR.Video.Delete;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using VideoEntity = Streetcode.DAL.Entities.Media.Video;

namespace Streetcode.XUnitTest.MediatRTests.Media.Video
{
    public class DeleteVideoHandlerTests
    {
        private const int SUCCESSFULSAVE = 1;
        private const int FAILEDSAVE = -1;

        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public DeleteVideoHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ShouldReturnOkResult_IfCommandHasValidInput()
        {
            // Arrange
            var request = GetValidVideoRecordRequest();
            SetupMock(SUCCESSFULSAVE);
            var handler = DeleteHandler();
            var command = new DeleteVideoCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnResultOfCorrectType_IfInputIsValid()
        {
            // Arrange
            var request = GetValidVideoRecordRequest();
            var expectedType = typeof(Result<DeleteVideoResponseDto>);
            SetupMock(SUCCESSFULSAVE);
            var handler = DeleteHandler();
            var command = new DeleteVideoCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.Should().BeOfType(expectedType);
        }

        [Fact]
        public async Task Handle_ShouldReturnResultFail_IfSavingOperationFailed()
        {
            // Arrange
            var request = GetValidVideoRecordRequest();
            SetupMock(FAILEDSAVE);
            var handler = DeleteHandler();
            var command = new DeleteVideoCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnErrorDeleteFailed_IfSavingOperationFailed()
        {
            // Arrange
            var request = GetValidVideoRecordRequest();
            SetupMock(FAILEDSAVE);
            var handler = DeleteHandler();
            var command = new DeleteVideoCommand(request);

            var expectedErrorMessage = string.Format(
            ErrorMessages.DeleteFailed,
            typeof(VideoEntity).Name,
            request.Id);

            // Act
            var result = await handler.Handle(command, _cancellationToken);
            var actualErrorMessage = result.Errors[0].Message;

            // Assert
            Assert.Equal(expectedErrorMessage, actualErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldCallSaveChangesAsyncOnce_IfInputIsValid()
        {
            // Arrange
            var request = GetValidVideoRecordRequest();
            SetupMock(SUCCESSFULSAVE);
            var handler = DeleteHandler();
            var command = new DeleteVideoCommand(request);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockRepositoryWrapper.Verify(x => x.SaveChangesAsync(), Times.Exactly(1));
        }

        private DeleteVideoHandler DeleteHandler()
        {
            return new DeleteVideoHandler(
                _mockRepositoryWrapper.Object,
                _mockLogger.Object);
        }

        private void SetupMock(int saveChangesAsyncResult)
        {
            var video = new VideoEntity { Id = 1 };

            _mockRepositoryWrapper
                .Setup(repo => repo.VideoRepository.GetFirstOrDefaultAsync(
                    AnyEntityPredicate<VideoEntity>(),
                    AnyEntityInclude<VideoEntity>()))
                .ReturnsAsync(video);

            _mockRepositoryWrapper
                .Setup(repo => repo.VideoRepository.Delete(video));

            _mockRepositoryWrapper.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(saveChangesAsyncResult);
        }

        private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
        {
            return It.IsAny<Expression<Func<TEntity, bool>>>();
        }

        private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
        {
            return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
        }

        private static DeleteVideoRequestDto GetValidVideoRecordRequest()
        {
            return new(Id: 1);
        }
    }
}
