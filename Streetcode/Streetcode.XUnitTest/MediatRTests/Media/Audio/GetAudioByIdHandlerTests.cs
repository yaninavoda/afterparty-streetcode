namespace Streetcode.XUnitTest.MediatRTests.Media.Audio;

using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

public class GetAudioByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IBlobService> _mockBlobService;

    public GetAudioByIdHandlerTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnsOkResult_WhenIdExists(int id)
    {
        // Arrange
        MockRepositorySetupReturnsAudio(id);
        MockMapperSetup(id);

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_RepositoryCallGetFirstOrDefaultAsyncOnlyOnce_WhenAudioExists(int id)
    {
        // Arrange
        MockRepositorySetupReturnsAudio(id);
        MockMapperSetup(id);

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        _mockRepositoryWrapper.Verify(
            repo =>
            repo.AudioRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Streetcode.DAL.Entities.Media.Audio, bool>>>(),
               It.IsAny<Func<IQueryable<Streetcode.DAL.Entities.Media.Audio>,
               IIncludableQueryable<Streetcode.DAL.Entities.Media.Audio, object>>>()),
            Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_MapperCallMapOnlyOnce_WhenAudioExists(int id)
    {
        // Arrange
        MockRepositorySetupReturnsAudio(id);
        MockMapperSetup(id);

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        _mockMapper.Verify(
            mapper => mapper.Map<AudioDTO>(It.IsAny<Audio>()),
            Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnsVideoWithCorrectId_WhenAudioExists(int id)
    {
        // Arrange
        MockRepositorySetupReturnsAudio(id);
        MockMapperSetup(id);

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Equal(id, result.Value.Id);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnAudioDto_WhenAudioExists(int id)
    {
        // Arrange
        MockRepositorySetupReturnsAudio(id);
        MockMapperSetup(id);

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.IsType<AudioDTO>(result.Value);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnFail_WhenAudioIsNotFound(int id)
    {
        // Arrange
        MockRepositorySetupReturnsNull();

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ShouldLogCorrectErrorMessage_WhenAudioIsNotFound(int id)
    {
        // Arrange
        MockRepositorySetupReturnsNull();

        var handler = new GetAudioByIdHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockBlobService.Object,
            _mockLogger.Object);

        var expectedMessage = $"Cannot find an audio with corresponding id: {id}";

        // Act
        var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);
        var actualMessage = result.Errors.First().Message;

        // Assert
        Assert.Equal(expectedMessage, actualMessage);
    }

    private void MockMapperSetup(int id)
    {
        _mockMapper.Setup(x => x
            .Map<AudioDTO>(It.IsAny<Streetcode.DAL.Entities.Media.Audio>()))
            .Returns(new AudioDTO { Id = id });
    }

    private void MockRepositorySetupReturnsAudio(int id)
    {
        _mockRepositoryWrapper.Setup(x => x.AudioRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Audio, bool>>>(),
               It.IsAny<Func<IQueryable<Audio>,
               IIncludableQueryable<Audio, object>>>()))
            .ReturnsAsync(new Audio { Id = id });
    }

    private void MockRepositorySetupReturnsNull()
    {
        _mockRepositoryWrapper.Setup(x => x.AudioRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Audio, bool>>>(),
                It.IsAny<Func<IQueryable<Audio>,
            IIncludableQueryable<Audio, object>>>()))
            .ReturnsAsync((IEnumerable<Audio>?)null);
    }
}
