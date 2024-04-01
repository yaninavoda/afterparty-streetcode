using Moq;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;
using Xunit;
using AutoMapper;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using FluentAssertions;
using Streetcode.BLL.Resources.Errors;

using FactEntity = Streetcode.DAL.Entities.Streetcode.TextContent.Fact;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact
{
    public class UpdateFactHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        public UpdateFactHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenSaveChangesAsyncWorkedSuccessfully()
        {
            // Arrange
            var request = GetRequest();
            var fact = GetFact();

            MockMapperSetup(request, fact);
            MockRepositoryWrapperWorkedSuccessfullySetup();

            var command = new UpdateFactCommand(request);
            var handler = new UpdateFactHandler(_mockRepositoryWrapper.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnFail_WhenSaveChangesAsyncWorkedIncorrectly()
        {
            // Arrange
            var request = GetRequest();
            var fact = GetFact();

            MockMapperSetup(request, fact);
            MockRepositoryWrapperWorkedFailSetup();

            var command = new UpdateFactCommand(request);
            var handler = new UpdateFactHandler(_mockRepositoryWrapper.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnCorrectError_WhenSaveChangesAsyncWorkedIncorrectly()
        {
            // Arrange
            var request = GetRequest();
            var fact = GetFact();

            MockMapperSetup(request, fact);
            MockRepositoryWrapperWorkedFailSetup();

            var command = new UpdateFactCommand(request);
            var handler = new UpdateFactHandler(_mockRepositoryWrapper.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().Contain(error => error.Message == ExpectedErrorMessage(request));
        }

        private void MockMapperSetup(UpdateFactDto request, FactEntity fact)
        {
            _mockMapper.Setup(mapper => mapper
            .Map<FactEntity>(It.IsAny<UpdateFactDto>()))
            .Returns(fact);
            _mockMapper.Setup(mapper => mapper
            .Map<UpdateFactDto>(It.IsAny<FactEntity>()))
            .Returns(request);
        }

        private void MockRepositoryWrapperWorkedSuccessfullySetup()
        {
            _mockRepositoryWrapper.Setup(repository => repository.FactRepository.Update(It.IsAny<FactEntity>()));
            _mockRepositoryWrapper.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void MockRepositoryWrapperWorkedFailSetup()
        {
            _mockRepositoryWrapper.Setup(repository => repository.FactRepository.Update(It.IsAny<FactEntity>()));
            _mockRepositoryWrapper.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(-1);
        }

        private UpdateFactDto GetRequest()
        {
            return new UpdateFactDto(1, "Title", "Content", 1, 1);
        }

        private FactEntity GetFact()
        {
            return new FactEntity()
            {
                Id = 1,
                Title = "Title",
                FactContent = "Content",
                ImageId = 1,
                StreetcodeId = 1,
            };
        }

        private string ExpectedErrorMessage(UpdateFactDto request)
        {
           return string.Format(
                ErrorMessages.UpdateFailed,
                nameof(FactEntity),
                request.Id);
        }
    }
}