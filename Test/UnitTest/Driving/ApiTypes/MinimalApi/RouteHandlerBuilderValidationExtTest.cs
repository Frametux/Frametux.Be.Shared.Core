using Frametux.Shared.Core.Driving.ApiTypes.MinimalApi.RequestValidation;
using Microsoft.AspNetCore.Builder;

namespace UnitTest.Driving.ApiTypes.MinimalApi;

[TestFixture]
public class RouteHandlerBuilderValidationExtTest
{
    #region Test Request Class

    public class TestRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public class AnotherTestRequest
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    #endregion

    #region Extension Method Tests

    [Test]
    public void WithRequestValidation_ShouldReturnSameBuilderInstance()
    {
        // Arrange
        var builder = CreateRouteHandlerBuilder();

        // Act
        var result = builder.WithRequestValidation<TestRequest>();

        // Assert
        Assert.That(result, Is.SameAs(builder));
    }

    [Test]
    public void WithRequestValidation_ShouldAllowChaining()
    {
        // Arrange
        var builder = CreateRouteHandlerBuilder();

        // Act
        var result = builder
            .WithRequestValidation<TestRequest>()
            .WithRequestValidation<AnotherTestRequest>();

        // Assert
        Assert.That(result, Is.SameAs(builder));
    }

    [Test]
    public void WithRequestValidation_ShouldBeCallableAsStaticMethod()
    {
        // Arrange
        var builder = CreateRouteHandlerBuilder();

        // Act
        var result = RouteHandlerBuilderValidationExt.WithRequestValidation<TestRequest>(builder);

        // Assert
        Assert.That(result, Is.SameAs(builder));
    }

    #endregion

    #region Helper Methods

    private static RouteHandlerBuilder CreateRouteHandlerBuilder() =>
        new([]);

    #endregion
}

