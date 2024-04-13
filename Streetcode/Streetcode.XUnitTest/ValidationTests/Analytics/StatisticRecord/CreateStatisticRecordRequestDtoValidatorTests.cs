using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Analytics.StatisticRecord;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Xunit;

namespace Streetcode.XUnitTest.ValidationTests.Analytics.StatisticRecord;

public class CreateStatisticRecordRequestDtoValidatorTests
{
    private const int MINSTREETCODEID = 1;
    private const int MINSTREETCODECOORDINATEID = 1;
    private const int MAXADDRESSLENGTH = 150;
    private const int MINADDRESSLENGTH = 1;

    private readonly CreateStatisticRecordRequestDtoValidator _validator;

    public CreateStatisticRecordRequestDtoValidatorTests()
    {
        _validator = new CreateStatisticRecordRequestDtoValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINSTREETCODEID - 10000)]
    public void Should_have_error_when_StreetcodeId_is_zero_or_negative(int id)
    {
        // Arrange
        var dto = new CreateStatisticRecordRequestDto(
                    StreetcodeId: id,
                    StreetcodeCoordinateId: MINSTREETCODECOORDINATEID,
                    Address: new string('a', MINADDRESSLENGTH));

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.StreetcodeId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MINSTREETCODECOORDINATEID - 10000)]
    public void Should_have_error_when_StreetcodeCoordinateId_is_zero_or_negative(int streetcodeCoordinateId)
    {
        // Arrange
        var dto = new CreateStatisticRecordRequestDto(
                    StreetcodeId: MINSTREETCODEID,
                    StreetcodeCoordinateId: streetcodeCoordinateId,
                    Address: new string('a', MINADDRESSLENGTH));

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.StreetcodeCoordinateId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MAXADDRESSLENGTH + 10000)]
    public void Should_have_error_when_Address_length_is_greater_than_MAXADDRESSLENGTH_or_equel_Zero(int id)
    {
        // Arrange
        var dto = new CreateStatisticRecordRequestDto(
                    StreetcodeId: MINSTREETCODEID,
                    StreetcodeCoordinateId: MINSTREETCODECOORDINATEID,
                    Address: new string('a', id));

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Should_not_have_error_when_dto_is_valid()
    {
        // Arrange
        var dto = new CreateStatisticRecordRequestDto(
            StreetcodeId: MINSTREETCODEID,
            StreetcodeCoordinateId: MINSTREETCODECOORDINATEID,
            Address: new string('a', MINADDRESSLENGTH));

        // Act
        var validationResult = _validator.TestValidate(dto);

        // Assert
        validationResult.ShouldNotHaveValidationErrorFor(x => x.StreetcodeId);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.StreetcodeCoordinateId);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Address);
    }
}