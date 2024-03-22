namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using DAL.Entities.Media.Images;
using System.Linq.Expressions;
using Xunit;

public class DeleteFactTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public DeleteFactTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_DeletesFactAndReturnsOkResult_WhenFactFound(int id)
    {
        // Arrange
        MockRepositoryWrapperSetup(id);

        var handler = new DeleteFactHandler(_mockRepositoryWrapper.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    private static Fact GetFact(int id)
    {
        return new Fact
        {
            Id = id,
        };
    }

    private static Fact? GetFactWithNotExistingId()
    {
        return null;
    }

    private void MockRepositoryWrapperSetup(int id)
    {
        _mockRepositoryWrapper.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        _mockRepositoryWrapper.Setup(x => x.FactRepository
            .Delete(GetFact(id)));

        _mockRepositoryWrapper.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
    }
}
