using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

using AudioEntity = Streetcode.DAL.Entities.Media.Audio;
using ImageEntity = Streetcode.DAL.Entities.Media.Images.Image;
using StreetcodeEntity = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.StreetcodeTests
{
    public class CreateStreetcodeHandlerTests
    {
        private const int SUCCESSFULSAVE = 1;
        private const int FAILEDSAVE = -1;

        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public CreateStreetcodeHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ShouldReturnOkResult_IfCommandHasValidInput()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, SUCCESSFULSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnResultOfCorrectType_IfInputIsValid()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            var expectedType = typeof(Result<CreateStreetcodeResponseDto>);
            SetupMock(request, SUCCESSFULSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.Should().BeOfType(expectedType);
        }

        [Fact]
        public async Task Handle_ShouldReturnResultFail_IfSavingOperationFailed()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, FAILEDSAVE);

            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldCallSaveChangesAsyncOnce_IfInputIsValid()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, SUCCESSFULSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockRepositoryWrapper.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAudioValidationFails()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, FAILEDSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUrlValidationFails()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, FAILEDSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenImageValidationFails()
        {
            // Arrange
            var request = GetValidCreateStreetcodeRequestDto();
            SetupMock(request, FAILEDSAVE);
            var handler = CreateHandler();
            var command = new CreateStreetcodeCommand(request);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        private CreateStreetcodeHandler CreateHandler()
        {
            return new CreateStreetcodeHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        private void SetupMock(CreateStreetcodeRequestDto request, int saveChangesAsyncResult)
        {
             var image = new ImageEntity { Id = 1 };
             var audio = new AudioEntity { Id = 1 };

             var streetcode = new StreetcodeEntity { Id = 1 };

             _mockRepositoryWrapper.Setup(r => r.StreetcodeRepository
               .Create(It.IsAny<StreetcodeEntity>())).Returns(streetcode);

             _mockMapper.Setup(m => m.Map<StreetcodeEntity>(request)).Returns(streetcode);

             _mockRepositoryWrapper.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(saveChangesAsyncResult);

             _mockRepositoryWrapper.Setup(r => r.AudioRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<AudioEntity, bool>>>(),
                null))
                .ReturnsAsync(audio);

             _mockRepositoryWrapper.Setup(r => r.ImageRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<ImageEntity, bool>>>(),
                null))
                .ReturnsAsync(image);

             _mockRepositoryWrapper
                .Setup(r => r.StreetcodeRepository.GetSingleOrDefaultAsync(
                    AnyEntityPredicate<StreetcodeEntity>(),
                    AnyEntityInclude<StreetcodeEntity>()))
                .ReturnsAsync(streetcode);

             _mockRepositoryWrapper.Setup(r => r.StreetcodeRepository.FindAll(
                 It.IsAny<Expression<Func<StreetcodeEntity, bool>>>()))
                .Returns(new List<StreetcodeEntity>().AsQueryable());

             _mockRepositoryWrapper.Setup(x => x.BeginTransaction())
                .Returns(new System.Transactions.TransactionScope());
        }

        private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
        {
            return It.IsAny<Expression<Func<TEntity, bool>>>();
        }

        private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
        {
            return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
        }

        private static StreetcodeEntity GetStreetcodeItem()
        {
            return new StreetcodeEntity { Id = 1, Index = 1, Teaser = "Teaser" };
        }

        private static CreateStreetcodeRequestDto GetValidCreateStreetcodeRequestDto()
        {
            return new CreateStreetcodeRequestDto
            {
                StreetcodeType = StreetcodeType.Person,
                Title = "Title",
                FirstName = "FirstName",
                LastName = "LastName",
                EventStartOrPersonBirthDate = DateTime.Now,
                TransliterationUrl = "validurl",
                TagIds = Enumerable.Range(1, 5),
                ImageIds = Enumerable.Range(1, 2)
            };
        }
    }
}