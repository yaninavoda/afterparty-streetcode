using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Art
{
    public class GetArtByIdTest
    {
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IMapper> _mockMapper;

        public GetArtByIdTest()
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
            MockRepositorySetupReturnsArt(id);
            MockMapperSetup(id);

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_RepositoryCallGetFirstOrDefaultAsyncOnlyOnce_WhenArtExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsArt(id);
            MockMapperSetup(id);

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(
                repo =>
                repo.ArtRepository.GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Images.Art, bool>>>(),
                   It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Images.Art>,
                   IIncludableQueryable<Streetcode.DAL.Entities.Media.Images.Art, object>>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_MapperCallMapOnlyOnce_WhenArtExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsArt(id);
            MockMapperSetup(id);

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            _mockMapper.Verify(
                mapper => mapper.Map<ArtDTO>(It.IsAny<Streetcode.DAL.Entities.Media.Images.Art>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsArtWithCorrectId_WhenArtExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsArt(id);
            MockMapperSetup(id);

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnArtDto_WhenArtExists(int id)
        {
            // Arrange
            MockRepositorySetupReturnsArt(id);
            MockMapperSetup(id);

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<ArtDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnFail_WhenArtIsNotFound(int id)
        {
            // Arrange
            MockRepositorySetupReturnsNull();

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ShouldLogCorrectErrorMessage_WhenArtIsNotFound(int id)
        {
            // Arrange
            MockRepositorySetupReturnsNull();

            var handler = new GetArtByIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            var expectedMessage = $"Cannot find an art with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);
            var actualMessage = result.Errors.First().Message;

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        private void MockMapperSetup(int id)
        {
            _mockMapper.Setup(x => x
                .Map<ArtDTO>(It.IsAny<Streetcode.DAL.Entities.Media.Images.Art>()))
                .Returns(new ArtDTO { Id = id });
        }

        private void MockRepositorySetupReturnsArt(int id)
        {
            _mockRepositoryWrapper.Setup(x => x.ArtRepository
                .GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Images.Art, bool>>>(),
                   It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Images.Art>,
                   IIncludableQueryable<Streetcode.DAL.Entities.Media.Images.Art, object>>>()))
                .ReturnsAsync(new Streetcode.DAL.Entities.Media.Images.Art { Id = id });
        }

        private void MockRepositorySetupReturnsNull()
        {
            _mockRepositoryWrapper.Setup(x => x.ArtRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Images.Art, bool>>>(),
                    It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Images.Art>,
                IIncludableQueryable<Streetcode.DAL.Entities.Media.Images.Art, object>>>()))
                .ReturnsAsync((IEnumerable<Streetcode.DAL.Entities.Media.Images.Art>?)null);
        }
    }
}
