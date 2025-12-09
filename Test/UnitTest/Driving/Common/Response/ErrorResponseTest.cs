using System.Text.Json;
using Frametux.Shared.Core.Driving.Common.Responses;
using Frametux.Shared.Core.Driving.Common.Responses.Error;

namespace UnitTest.Driving.Common.Response;

[TestFixture]
public class ErrorResponseTest
{
    #region Constructor and Inheritance Tests

    [Test]
    public void Constructor_ShouldSetIsSuccessToFalse()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Error message",
            Type = ResponseType.NotFound
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void Constructor_WithNotFound_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Resource not found";
        const ResponseType expectedType = ResponseType.NotFound;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            Type = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithValidationFailed_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Validation failed";
        const ResponseType expectedType = ResponseType.ValidationFailed;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            Type = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithBusinessLogicFailed_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Business logic failed";
        const ResponseType expectedType = ResponseType.BusinessLogicFailed;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            Type = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
    }

    #endregion

    #region ResponseType Property Tests

    [Test]
    public void ResponseType_WithNotFound_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.NotFound));
    }

    [Test]
    public void ResponseType_WithValidationFailed_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.ValidationFailed
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.ValidationFailed));
    }

    [Test]
    public void ResponseType_WithBusinessLogicFailed_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.BusinessLogicFailed
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.BusinessLogicFailed));
    }

    [Test]
    public void ResponseType_JsonSerialization_ShouldSerializeAsString()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.ValidationFailed
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"ValidationFailed\""));
        Assert.That(json, Does.Not.Contain("\"Type\":1"));
    }

    [Test]
    public void ResponseType_JsonDeserialization_ShouldDeserializeFromString()
    {
        // Arrange
        const string json = "{\"IsSuccess\":false,\"Type\":\"NotFound\",\"Message\":\"Test message\"}";

        // Act
        var response = JsonSerializer.Deserialize<ErrorResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Type, Is.EqualTo(ResponseType.NotFound));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void ResponseType_EnumValues_ShouldHaveCorrectUnderlyingValues()
    {
        // Assert
        Assert.That((int)ResponseType.NotFound, Is.EqualTo(2));
        Assert.That((int)ResponseType.ValidationFailed, Is.EqualTo(3));
        Assert.That((int)ResponseType.BusinessLogicFailed, Is.EqualTo(4));
    }

    #endregion

    #region Message Property Tests

    [Test]
    public void Message_WhenSet_ShouldReturnSameValue()
    {
        // Arrange
        const string expectedMessage = "Operation failed";

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            Type = ResponseType.NotFound
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Message_WithEmptyString_ShouldAllowEmptyString()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = string.Empty,
            Type = ResponseType.ValidationFailed
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Message_WithLongString_ShouldAcceptLongMessage()
    {
        // Arrange
        var longMessage = new string('a', 1000);

        // Act
        var response = new ErrorResponse
        {
            Message = longMessage,
            Type = ResponseType.BusinessLogicFailed
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(longMessage));
        Assert.That(response.Message.Length, Is.EqualTo(1000));
    }

    #endregion

    #region Record Equality Tests

    [Test]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentResponseType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.ValidationFailed
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Message 1",
            Type = ResponseType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Message 2",
            Type = ResponseType.NotFound
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessageAndResponseType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Message 1",
            Type = ResponseType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Message 2",
            Type = ResponseType.ValidationFailed
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Act & Assert
        Assert.That(response, Is.Not.EqualTo(null));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.That(response is null, Is.False);
    }

    [Test]
    public void Equality_SameReference_ShouldBeEqual()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };
        var sameReference = response;

        // Act & Assert
        Assert.That(response, Is.EqualTo(sameReference));
        // ReSharper disable once EqualExpressionComparison
        Assert.That(response == sameReference, Is.True);
        Assert.That(ReferenceEquals(response, sameReference), Is.True);
    }

    #endregion

    #region Record With Expression Tests

    [Test]
    public void WithExpression_ModifyingMessage_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new ErrorResponse
        {
            Message = "Original message",
            Type = ResponseType.NotFound
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.Type, Is.EqualTo(original.Type));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingResponseType_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Act
        var modified = original with { Type = ResponseType.ValidationFailed };

        // Assert
        Assert.That(modified.Type, Is.EqualTo(ResponseType.ValidationFailed));
        Assert.That(original.Type, Is.EqualTo(ResponseType.NotFound));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingBothProperties_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new ErrorResponse
        {
            Message = "Original message",
            Type = ResponseType.NotFound
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            Type = ResponseType.BusinessLogicFailed
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.Type, Is.EqualTo(ResponseType.BusinessLogicFailed));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.Type, Is.EqualTo(ResponseType.NotFound));
        Assert.That(modified.IsSuccess, Is.False);
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_WithoutChanges_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.ValidationFailed
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.Type, Is.EqualTo(original.Type));
        Assert.That(copy.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(copy, Is.EqualTo(original));
        Assert.That(ReferenceEquals(original, copy), Is.False);
    }

    #endregion

    #region ToString Tests

    [Test]
    public void ToString_WithNotFound_ShouldContainAllPropertyValues()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("NotFound"));
    }

    [Test]
    public void ToString_WithValidationFailed_ShouldContainCorrectResponseType()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Validation error",
            Type = ResponseType.ValidationFailed
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("Validation error"));
        Assert.That(result, Does.Contain("ValidationFailed"));
    }

    [Test]
    public void ToString_WithBusinessLogicFailed_ShouldContainCorrectResponseType()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Business logic error",
            Type = ResponseType.BusinessLogicFailed
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("Business logic error"));
        Assert.That(result, Does.Contain("BusinessLogicFailed"));
    }

    [Test]
    public void ToString_WithEmptyMessage_ShouldHandleGracefully()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = string.Empty,
            Type = ResponseType.NotFound
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("NotFound"));
    }

    #endregion

    #region Inheritance Tests

    [Test]
    public void Inheritance_ShouldInheritFromBaseResponse()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            Type = ResponseType.NotFound
        };

        // Assert
        Assert.That(response, Is.InstanceOf<BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeFalse()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Test message 1",
            Type = ResponseType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message 2",
            Type = ResponseType.ValidationFailed
        };
        var response3 = new ErrorResponse
        {
            Message = "Test message 3",
            Type = ResponseType.BusinessLogicFailed
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.False);
        Assert.That(response2.IsSuccess, Is.False);
        Assert.That(response3.IsSuccess, Is.False);
    }

    #endregion
}


