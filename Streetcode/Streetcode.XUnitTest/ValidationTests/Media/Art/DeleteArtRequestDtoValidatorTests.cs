using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.Art.Delete;
using Xunit;

namespace Streetcode.XUnitTest.ValidationTests.Media.Art;

public class DeleteArtRequestDtoValidatorTests
{
    private const int MINARTID = 1;

    private readonly DeleteArtRequestDtoValidator _validator;

    public DeleteArtRequestDtoValidatorTests()
    {
        _validator = new DeleteArtRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINARTID - 10000)]
    public void Should_have_error_when_ArtId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new DeleteArtRequestDto(Id: id);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new DeleteArtRequestDto(Id: MINARTID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
