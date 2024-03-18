using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests
{
    public class GetFactByIdTests
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetFactByIdTests()
        {
            this._mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_ShouldReturnOk_IfIdExists(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsFact(id);
            this.MockMapperSetup(id);

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_RepositoryShouldCallGetFirstOrDefaultAsyncOnlyOnce_IfFactExists(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsFact(id);
            this.MockMapperSetup(id);

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            this._mockRepositoryWrapper.Verify(
                repo =>
                repo.FactRepository.GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Fact, bool>>>(),
                   It.IsAny<Func<IQueryable<Fact>,
                   IIncludableQueryable<Fact, object>>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_MapperShouldCallMapOnlyOnce_IfFactExists(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsFact(id);
            this.MockMapperSetup(id);

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            this._mockMapper.Verify(
                mapper => mapper.Map<FactDto>(It.IsAny<Fact>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_ShouldReturnFactWithCorrectId_IfFactExists(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsFact(id);
            this.MockMapperSetup(id);

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_ShouldReturnFactDto_IfFactExists(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsFact(id);
            this.MockMapperSetup(id);

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<FactDto>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_ShouldReturnFail_WhenFactIsNotFound(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsNull();

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetFactById_ShouldLogCorrectErrorMessage_WhenFactIsNotFound(int id)
        {
            // Arrange
            this.MockRepositorySetupReturnsNull();

            var handler = new GetFactByIdHandler(
                this._mockRepositoryWrapper.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            var expectedMessage = $"Cannot find any fact with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);
            var actualMessage = result.Errors.First().Message;

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        private void MockMapperSetup(int id)
        {
            this._mockMapper.Setup(x => x
                .Map<FactDto>(It.IsAny<Fact>()))
                .Returns(new FactDto { Id = id });
        }

        private void MockRepositorySetupReturnsFact(int id)
        {
            this._mockRepositoryWrapper.Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Fact, bool>>>(),
                   It.IsAny<Func<IQueryable<Fact>,
                   IIncludableQueryable<Fact, object>>>()))
                .ReturnsAsync(new Fact { Id = id });
        }

        private void MockRepositorySetupReturnsNull()
        {
            this._mockRepositoryWrapper.Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Fact, bool>>>(),
                   It.IsAny<Func<IQueryable<Fact>,
                   IIncludableQueryable<Fact, object>>>()))
                .ReturnsAsync((Fact?)null);
        }
    }
}
