using System.Text.Json;
using Frametux.Shared.Core.Driving.Common.Responses;
using Frametux.Shared.Core.Driving.Common.Responses.Success;

namespace UnitTest.Driving.Common.Response;

[TestFixture]
public class SuccessResponseTest
{
    #region Constructor and Inheritance Tests

    [Test]
    public void Constructor_ShouldSetIsSuccessToTrue()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Success message",
            Type = ResponseType.CreateSuccess
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void Constructor_WithRetrieveDataSuccess_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Data retrieved successfully";
        const ResponseType expectedType = ResponseType.RetrieveDataSuccess;

        // Act
        var response = new SuccessResponse
        {
            Message = expectedMessage,
            Type = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithCreateSuccess_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Resource created successfully";
        const ResponseType expectedType = ResponseType.CreateSuccess;

        // Act
        var response = new SuccessResponse
        {
            Message = expectedMessage,
            Type = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
    }

    #endregion

    #region ResponseType Property Tests

    [Test]
    public void ResponseType_WithRetrieveDataSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
    }

    [Test]
    public void ResponseType_WithCreateSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.CreateSuccess));
    }

    [Test]
    public void ResponseType_JsonSerialization_ShouldSerializeAsString()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"CreateSuccess\""));
        Assert.That(json, Does.Not.Contain("\"Type\":1"));
    }

    [Test]
    public void ResponseType_JsonDeserialization_ShouldDeserializeFromString()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Type\":\"RetrieveDataSuccess\",\"Message\":\"Test message\"}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void ResponseType_EnumValues_ShouldHaveCorrectUnderlyingValues()
    {
        // Assert
        Assert.That((int)ResponseType.RetrieveDataSuccess, Is.EqualTo(0));
        Assert.That((int)ResponseType.CreateSuccess, Is.EqualTo(1));
    }

    #endregion

    #region Message Property Tests

    [Test]
    public void Message_WhenSet_ShouldReturnSameValue()
    {
        // Arrange
        const string expectedMessage = "Operation completed successfully";

        // Act
        var response = new SuccessResponse
        {
            Message = expectedMessage,
            Type = ResponseType.CreateSuccess
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Message_WithEmptyString_ShouldAllowEmptyString()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = string.Empty,
            Type = ResponseType.RetrieveDataSuccess
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
        var response = new SuccessResponse
        {
            Message = longMessage,
            Type = ResponseType.CreateSuccess
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
        var response1 = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
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
        var response1 = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Message 1",
            Type = ResponseType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Message 2",
            Type = ResponseType.CreateSuccess
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessageAndResponseType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Message 1",
            Type = ResponseType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Message 2",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
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
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
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
        var original = new SuccessResponse
        {
            Message = "Original message",
            Type = ResponseType.CreateSuccess
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
        var original = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };

        // Act
        var modified = original with { Type = ResponseType.RetrieveDataSuccess };

        // Assert
        Assert.That(modified.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(original.Type, Is.EqualTo(ResponseType.CreateSuccess));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingBothProperties_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponse
        {
            Message = "Original message",
            Type = ResponseType.CreateSuccess
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.Type, Is.EqualTo(ResponseType.CreateSuccess));
        Assert.That(modified.IsSuccess, Is.True);
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_WithoutChanges_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
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
    public void ToString_WithCreateSuccess_ShouldContainAllPropertyValues()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponse"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    [Test]
    public void ToString_WithRetrieveDataSuccess_ShouldContainCorrectResponseType()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Data retrieved",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponse"));
        Assert.That(result, Does.Contain("Data retrieved"));
        Assert.That(result, Does.Contain("RetrieveDataSuccess"));
    }

    [Test]
    public void ToString_WithEmptyMessage_ShouldHandleGracefully()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = string.Empty,
            Type = ResponseType.CreateSuccess
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponse"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    #endregion

    #region Inheritance Tests

    [Test]
    public void Inheritance_ShouldInheritFromBaseResponse()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess
        };

        // Assert
        Assert.That(response, Is.InstanceOf<BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeTrue()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Test message 1",
            Type = ResponseType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message 2",
            Type = ResponseType.RetrieveDataSuccess
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.True);
        Assert.That(response2.IsSuccess, Is.True);
    }

    #endregion
}

