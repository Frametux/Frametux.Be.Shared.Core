using System.Text.Json;
using Frametux.Shared.Core.Driving.Responses.Error;

namespace UnitTest.Driving.Response;

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
            ErrorType = ErrorType.NotFound
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void Constructor_WithNotFound_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Resource not found";
        const ErrorType expectedType = ErrorType.NotFound;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            ErrorType = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.ErrorType, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithValidationFailed_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Validation failed";
        const ErrorType expectedType = ErrorType.ValidationFailed;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            ErrorType = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.ErrorType, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithBusinessLogicFailed_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Business logic failed";
        const ErrorType expectedType = ErrorType.BusinessLogicFailed;

        // Act
        var response = new ErrorResponse
        {
            Message = expectedMessage,
            ErrorType = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.ErrorType, Is.EqualTo(expectedType));
    }

    #endregion

    #region ErrorType Property Tests

    [Test]
    public void ErrorType_WithNotFound_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.NotFound
        };

        // Assert
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.NotFound));
    }

    [Test]
    public void ErrorType_WithValidationFailed_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed
        };

        // Assert
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
    }

    [Test]
    public void ErrorType_WithBusinessLogicFailed_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.BusinessLogicFailed
        };

        // Assert
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.BusinessLogicFailed));
    }

    [Test]
    public void ErrorType_JsonSerialization_ShouldSerializeAsString()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"ValidationFailed\""));
        Assert.That(json, Does.Not.Contain("\"ErrorType\":1"));
    }

    [Test]
    public void ErrorType_JsonDeserialization_ShouldDeserializeFromString()
    {
        // Arrange
        const string json = "{\"IsSuccess\":false,\"Message\":\"Test message\",\"ErrorType\":\"NotFound\"}";

        // Act
        var response = JsonSerializer.Deserialize<ErrorResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.ErrorType, Is.EqualTo(ErrorType.NotFound));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void ErrorType_EnumValues_ShouldHaveCorrectUnderlyingValues()
    {
        // Assert
        Assert.That((int)ErrorType.NotFound, Is.EqualTo(0));
        Assert.That((int)ErrorType.ValidationFailed, Is.EqualTo(1));
        Assert.That((int)ErrorType.BusinessLogicFailed, Is.EqualTo(2));
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
            ErrorType = ErrorType.NotFound
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
            ErrorType = ErrorType.ValidationFailed
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
            ErrorType = ErrorType.BusinessLogicFailed
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
            ErrorType = ErrorType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.NotFound
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentErrorType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed
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
            ErrorType = ErrorType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Message 2",
            ErrorType = ErrorType.NotFound
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessageAndErrorType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Message 1",
            ErrorType = ErrorType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Message 2",
            ErrorType = ErrorType.ValidationFailed
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
            ErrorType = ErrorType.NotFound
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
            ErrorType = ErrorType.NotFound
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
            ErrorType = ErrorType.NotFound
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.ErrorType, Is.EqualTo(original.ErrorType));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingErrorType_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new ErrorResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.NotFound
        };

        // Act
        var modified = original with { ErrorType = ErrorType.ValidationFailed };

        // Assert
        Assert.That(modified.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(original.ErrorType, Is.EqualTo(ErrorType.NotFound));
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
            ErrorType = ErrorType.NotFound
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            ErrorType = ErrorType.BusinessLogicFailed
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.ErrorType, Is.EqualTo(ErrorType.BusinessLogicFailed));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.ErrorType, Is.EqualTo(ErrorType.NotFound));
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
            ErrorType = ErrorType.ValidationFailed
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.ErrorType, Is.EqualTo(original.ErrorType));
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
            ErrorType = ErrorType.NotFound
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("NotFound"));
    }

    [Test]
    public void ToString_WithValidationFailed_ShouldContainCorrectErrorType()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Validation error",
            ErrorType = ErrorType.ValidationFailed
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorResponse"));
        Assert.That(result, Does.Contain("Validation error"));
        Assert.That(result, Does.Contain("ValidationFailed"));
    }

    [Test]
    public void ToString_WithBusinessLogicFailed_ShouldContainCorrectErrorType()
    {
        // Arrange
        var response = new ErrorResponse
        {
            Message = "Business logic error",
            ErrorType = ErrorType.BusinessLogicFailed
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
            ErrorType = ErrorType.NotFound
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
            ErrorType = ErrorType.NotFound
        };

        // Assert
        Assert.That(response, Is.InstanceOf<Frametux.Shared.Core.Driving.Responses.BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeFalse()
    {
        // Arrange
        var response1 = new ErrorResponse
        {
            Message = "Test message 1",
            ErrorType = ErrorType.NotFound
        };
        var response2 = new ErrorResponse
        {
            Message = "Test message 2",
            ErrorType = ErrorType.ValidationFailed
        };
        var response3 = new ErrorResponse
        {
            Message = "Test message 3",
            ErrorType = ErrorType.BusinessLogicFailed
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.False);
        Assert.That(response2.IsSuccess, Is.False);
        Assert.That(response3.IsSuccess, Is.False);
    }

    #endregion
}

