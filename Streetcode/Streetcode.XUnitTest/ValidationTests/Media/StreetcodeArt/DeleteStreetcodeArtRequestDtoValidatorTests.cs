using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.Delete;
using Xunit;

namespace Streetcode.XUnitTest.ValidationTests.Media.StreetcodeArt;

public class DeleteStreetcodeArtRequestDtoValidatorTests
{
    private const int MINARTID = 1;
    private const int MINSTREETCODEID = 1;

    private readonly DeleteStreetcodeArtRequestDtoValidator _validator;

    public DeleteStreetcodeArtRequestDtoValidatorTests()
    {
        _validator = new DeleteStreetcodeArtRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINARTID - 10000)]
    public void Should_have_error_when_ArtId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new DeleteStreetcodeArtRequestDto(
            ArtId: id,
            StreetcodeId: MINSTREETCODEID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.ArtId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINSTREETCODEID - 10000)]
    public void Should_have_error_when_StreetcodeId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new DeleteStreetcodeArtRequestDto(
            ArtId: MINARTID,
            StreetcodeId: id);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.StreetcodeId);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new DeleteStreetcodeArtRequestDto(
            ArtId: MINARTID,
            StreetcodeId: MINSTREETCODEID);

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.ArtId);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.StreetcodeId);
    }
}