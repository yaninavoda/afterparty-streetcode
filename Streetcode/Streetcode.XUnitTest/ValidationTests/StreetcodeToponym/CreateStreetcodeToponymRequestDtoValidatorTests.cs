using Streetcode.BLL.DTO.StreetcodeToponym;
using Streetcode.BLL.MediatR.StreetcodeToponym.Create;
using Xunit;
using FluentValidation.TestHelper;

namespace Streetcode.XUnitTest.ValidationTests.StreetcodeToponym;

public class CreateStreetcodeToponymRequestDtoValidatorTests
{
    private const int MINSTREETCODEID = 1;
    private const int MINTOPONYMID = 1;

    private readonly CreateStreetcodeToponymRequestDtoValidator _validator;

    public CreateStreetcodeToponymRequestDtoValidatorTests()
    {
        _validator = new CreateStreetcodeToponymRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINTOPONYMID - 10000)]
    public void Should_have_error_when_ToponymId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new CreateStreetcodeToponymRequestDto(
                    StreetcodeId: MINSTREETCODEID,
                    ToponymId: id);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.ToponymId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINSTREETCODEID - 10000)]
    public void Should_have_error_when_StreetcodeId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new CreateStreetcodeToponymRequestDto(
                    StreetcodeId: id,
                    ToponymId: MINSTREETCODEID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.StreetcodeId);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new CreateStreetcodeToponymRequestDto(
            ToponymId: MINTOPONYMID,
            StreetcodeId: MINSTREETCODEID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.StreetcodeId);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.ToponymId);
    }
}
