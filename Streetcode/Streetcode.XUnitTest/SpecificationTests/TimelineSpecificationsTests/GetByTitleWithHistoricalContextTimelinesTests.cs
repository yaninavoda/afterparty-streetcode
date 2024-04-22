using Ardalis.Specification;
using FluentAssertions;
using Xunit;
using static Streetcode.DAL.Specifications.TimelineSpecifications.HistoricalContextSpecs;

namespace Streetcode.XUnitTest.SpecificationTests.TimelineSpecificationsTests;

public class GetByTitleWithHistoricalContextTimelinesTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("test")]
    public void AddsOneExpressionToList_GivenOneWhereExpression(string contextName)
    {
        var spec = new GetByTitleWithHistoricalContextTimelines(contextName);

        spec.WhereExpressions.Should().ContainSingle();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("test")]
    public void AddsIncludeExpressionInfoToListWithTypeInclude_GivenIncludeExpression(string contextName)
    {
        var spec = new GetByTitleWithHistoricalContextTimelines(contextName);

        spec.IncludeExpressions.Should().ContainSingle();
        spec.IncludeExpressions.Single().Type.Should().Be(IncludeTypeEnum.Include);
    }
}
