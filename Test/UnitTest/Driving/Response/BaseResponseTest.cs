using Frametux.Shared.Core.Driving.Responses;

namespace UnitTest.Driving.Response;

[TestFixture]
public class BaseResponseTest
{
    #region Test Helper Class

    // Concrete implementation for testing the abstract BaseResponse
    private record TestResponse(bool IsSuccess) : BaseResponse(IsSuccess);

    #endregion

    #region Constructor and IsSuccess Property Tests

    [Test]
    public void Constructor_WithTrue_ShouldSetIsSuccessToTrue()
    {
        // Arrange & Act
        var response = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Success message"
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void Constructor_WithFalse_ShouldSetIsSuccessToFalse()
    {
        // Arrange & Act
        var response = new TestResponse(false)
        {
            Type = ResponseType.NotFound,
            Message = "Error message"
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
    }

    #endregion

    #region Message Property Tests

    [Test]
    public void Message_WhenSet_ShouldReturnSameValue()
    {
        // Arrange
        const string expectedMessage = "Test message";

        // Act
        var response = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = expectedMessage
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Message_WithEmptyString_ShouldAllowEmptyString()
    {
        // Arrange & Act
        var response = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = string.Empty
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(string.Empty));
    }

    #endregion

    #region Record Equality Tests

    [Test]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var response1 = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };
        var response2 = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentIsSuccess_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };
        var response2 = new TestResponse(false)
        {
            Type = ResponseType.NotFound,
            Message = "Test message"
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Message 1"
        };
        var response2 = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Message 2"
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var response = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };

        // Act & Assert
        Assert.That(response, Is.Not.EqualTo(null));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.That(response is null, Is.False);
    }

    #endregion

    #region Record With Expression Tests

    [Test]
    public void WithExpression_ModifyingMessage_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Original message"
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_WithoutChanges_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(copy, Is.EqualTo(original));
        Assert.That(ReferenceEquals(original, copy), Is.False);
    }

    #endregion

    #region ToString Tests

    [Test]
    public void ToString_ShouldContainPropertyValues()
    {
        // Arrange
        var response = new TestResponse(true)
        {
            Type = ResponseType.CreateSuccess,
            Message = "Test message"
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("TestResponse"));
        Assert.That(result, Does.Contain("True"));
        Assert.That(result, Does.Contain("Test message"));
    }

    [Test]
    public void ToString_WithFalseIsSuccess_ShouldContainFalse()
    {
        // Arrange
        var response = new TestResponse(false)
        {
            Type = ResponseType.NotFound,
            Message = "Error message"
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("TestResponse"));
        Assert.That(result, Does.Contain("False"));
        Assert.That(result, Does.Contain("Error message"));
    }

    #endregion
}

