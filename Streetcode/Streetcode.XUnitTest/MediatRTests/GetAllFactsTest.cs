namespace Streetcode.XUnitTest.MediatRTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentAssertions;
    using FluentResults;
    using Moq;
    using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
    using Streetcode.BLL.Interfaces.Logging;
    using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
    using Streetcode.DAL.Entities.Streetcode.TextContent;
    using Streetcode.DAL.Repositories.Interfaces.Base;
    using Xunit;

    public class GetAllFactsTest
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllFactsTest()
        {
            this._mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task GetAllFacts_ShouldReturnOk_WhenFactsExist()
        {
            // Arrange
            List<Fact> mockFacts = GetMockData();

            this._mockRepositoryWrapper.Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync(mockFacts);

            var handler = new GetAllFactsHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllFacts_RepositoryShouldCallGetAllAsyncOnlyOnce_WhenFactsExist()
        {
            // Arrange
            List<Fact> mockFacts = GetMockData();

            this._mockRepositoryWrapper.Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync(mockFacts);

            var handler = new GetAllFactsHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            // Assert
            this._mockRepositoryWrapper.Verify(
                repo =>
                repo.FactRepository.GetAllAsync(null, null), Times.Once);
        }

        [Fact]
        public async Task GetAllFacts_MapperShouldMapOnlyOnce_WhenFactsExist()
        {
            // Arrange
            List<Fact> mockFacts = GetMockData();

            this._mockRepositoryWrapper.Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync(mockFacts);

            var handler = new GetAllFactsHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            // Assert
            this._mockMapper.Verify(
                mapper =>
                mapper.Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllFacts_ShouldReturnCollectionOfFactDto_WhenFactsExist()
        {
            //Arrange

            List<Fact> mockFacts = GetMockData();

            this._mockRepositoryWrapper.Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync(mockFacts);

            var handler = new GetAllFactsHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            //Assert
            Assert.IsType<FactDto[]>(result.ValueOrDefault);
        }

        [Fact]
        public async Task GetAllFacts_ShouldReturnFail_WhenFactsAreNull()
        {
            // Arrange
            this._mockRepositoryWrapper
                .Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync((IEnumerable<Fact>)null);

            var handler = new GetAllFactsHandler(this._mockRepositoryWrapper.Object, this._mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task GetAllFacts_ShouldLogCorrectErrorMessage_WhenFactsAreNull()
        {
            // Arrange
            this._mockRepositoryWrapper
                .Setup(repo => repo.FactRepository.GetAllAsync(null, null))
                .ReturnsAsync((IEnumerable<Fact>)null);

            var handler = new GetAllFactsHandler(this._mockRepositoryWrapper.Object, this._mockMapper.Object, _mockLogger.Object);

            var expectedError = "Cannot find any fact";

            // Act
            var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private static List<Fact> GetMockData()
        {
            return new List<Fact>
            {
                new ()
                {
                    Id = 1,
                    Title = "Title1",
                    FactContent = "Fact content 1",
                    ImageId = 1,
                },
                new ()
                {
                    Id = 2,
                    Title = "Title2",
                    FactContent = "Fact content 2",
                    ImageId = 2,
                },
                new ()
                {
                    Id = 3,
                    Title = "Title3",
                    FactContent = "Fact content 3",
                    ImageId = 3,
                },
            };
        }
    }
}
