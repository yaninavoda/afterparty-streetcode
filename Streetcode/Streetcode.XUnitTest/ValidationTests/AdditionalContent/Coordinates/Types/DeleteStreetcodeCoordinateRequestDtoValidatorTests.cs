using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.MediatR.AdditionalContent.StreetcodeCoordinate.Delete;
using Xunit;

namespace Streetcode.XUnitTest.ValidationTests.AdditionalContent.Coordinates.Types;

public class DeleteStreetcodeCoordinateRequestDtoValidatorTests
{
    private const int MINID = 1;

    private readonly DeleteStreetcodeCoordinateRequestDtoValidator _validator;

    public DeleteStreetcodeCoordinateRequestDtoValidatorTests()
    {
        _validator = new DeleteStreetcodeCoordinateRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINID - 10000)]
    public void Should_have_error_when_StreetcodeId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new DeleteStreetcodeCoordinateRequestDto(Id: id);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new DeleteStreetcodeCoordinateRequestDto(Id: MINID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}