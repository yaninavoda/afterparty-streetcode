using AutoMapper;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.XUnitTest.MediatRTests.Sources.SourceLinkCategory;

using DAL.Entities.Sources;
using FluentResults;
using Streetcode.BLL.Dto.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;
using System.Reflection.Metadata;
using Xunit;

public class CreateCategoryHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public CreateCategoryHandlerTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_IfSuccessfulScenario()
    {
        // Arrangevar
        var category = GetCategory();
        MockMapperSetup(category);
        MockRepositorySetup(category);

        var handler = new CreateCategoryHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateCategoryCommand(GetCategoryDto()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnOkWithCorrectType_IfSuccessfulScenario()
    {
        // Arrange
        var category = GetCategory();
        MockMapperSetup(category);
        MockRepositorySetup(category);

        var handler = new CreateCategoryHandler(
            _mockRepositoryWrapper.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreateCategoryCommand(GetCategoryDto()), CancellationToken.None);

        // Assert
        Assert.IsType<SourceLinkCategory>(result.ValueOrDefault);
    }

    private static SourceLinkCategory GetCategory()
    {
        return new SourceLinkCategory()
        {
            Id = 1,
            Title = "Test Title",
            ImageId = 1,
        };
    }

    private static SourceLinkCategoryDto GetCategoryDto()
    {
        return new SourceLinkCategoryDto()
        {
            Id = 1,
            Title = "Test Title",
            ImageId = 1,
        };
    }

    private void MockMapperSetup(SourceLinkCategory category)
    {
        _mockMapper.Setup(x => x.Map<SourceLinkCategory>(It.IsAny<SourceLinkCategoryDto>()))
            .Returns(category);
    }

    private void MockRepositorySetup(SourceLinkCategory category)
    {
        _mockRepositoryWrapper.Setup(x => x.SourceCategoryRepository.Create(category)).Returns(category);

        _mockRepositoryWrapper.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
    }
}
