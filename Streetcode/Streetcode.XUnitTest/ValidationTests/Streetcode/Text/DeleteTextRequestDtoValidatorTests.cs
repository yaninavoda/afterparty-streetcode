using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.MediatR.Streetcode.Text.Delete;
using Xunit;

namespace Streetcode.XUnitTest.ValidationTests.Streetcode.Text;

public class DeleteTextRequestDtoValidatorTests
{
    private const int MINID = 1;

    private readonly DeleteTextRequestDtoValidator _validator;

    public DeleteTextRequestDtoValidatorTests()
    {
        _validator = new DeleteTextRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINID - 10000)]
    public void Should_have_error_when_Id_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new DeleteTextRequestDto(Id: id);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new DeleteTextRequestDto(Id: MINID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}