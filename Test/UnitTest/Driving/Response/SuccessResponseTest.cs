using System.Text.Json;
using Frametux.Shared.Core.Driving.Responses.Success;

namespace UnitTest.Driving.Response;

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
            SuccessType = SuccessType.CreateSuccess
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void Constructor_WithRetrieveDataSuccess_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Data retrieved successfully";
        const SuccessType expectedType = SuccessType.RetrieveDataSuccess;

        // Act
        var response = new SuccessResponse
        {
            Message = expectedMessage,
            SuccessType = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.SuccessType, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor_WithCreateSuccess_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Resource created successfully";
        const SuccessType expectedType = SuccessType.CreateSuccess;

        // Act
        var response = new SuccessResponse
        {
            Message = expectedMessage,
            SuccessType = expectedType
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.SuccessType, Is.EqualTo(expectedType));
    }

    #endregion

    #region SuccessType Property Tests

    [Test]
    public void SuccessType_WithRetrieveDataSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.RetrieveDataSuccess
        };

        // Assert
        Assert.That(response.SuccessType, Is.EqualTo(SuccessType.RetrieveDataSuccess));
    }

    [Test]
    public void SuccessType_WithCreateSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.CreateSuccess
        };

        // Assert
        Assert.That(response.SuccessType, Is.EqualTo(SuccessType.CreateSuccess));
    }

    [Test]
    public void SuccessType_JsonSerialization_ShouldSerializeAsString()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"CreateSuccess\""));
        Assert.That(json, Does.Not.Contain("\"SuccessType\":1"));
    }

    [Test]
    public void SuccessType_JsonDeserialization_ShouldDeserializeFromString()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Message\":\"Test message\",\"SuccessType\":\"RetrieveDataSuccess\"}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.SuccessType, Is.EqualTo(SuccessType.RetrieveDataSuccess));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void SuccessType_EnumValues_ShouldHaveCorrectUnderlyingValues()
    {
        // Assert
        Assert.That((int)SuccessType.RetrieveDataSuccess, Is.EqualTo(0));
        Assert.That((int)SuccessType.CreateSuccess, Is.EqualTo(1));
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
            SuccessType = SuccessType.CreateSuccess
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
            SuccessType = SuccessType.RetrieveDataSuccess
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
            SuccessType = SuccessType.CreateSuccess
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
            SuccessType = SuccessType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.CreateSuccess
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentSuccessType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.RetrieveDataSuccess
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
            SuccessType = SuccessType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Message 2",
            SuccessType = SuccessType.CreateSuccess
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessageAndSuccessType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Message 1",
            SuccessType = SuccessType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Message 2",
            SuccessType = SuccessType.RetrieveDataSuccess
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
            SuccessType = SuccessType.CreateSuccess
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
            SuccessType = SuccessType.CreateSuccess
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
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.SuccessType, Is.EqualTo(original.SuccessType));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingSuccessType_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponse
        {
            Message = "Test message",
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var modified = original with { SuccessType = SuccessType.RetrieveDataSuccess };

        // Assert
        Assert.That(modified.SuccessType, Is.EqualTo(SuccessType.RetrieveDataSuccess));
        Assert.That(original.SuccessType, Is.EqualTo(SuccessType.CreateSuccess));
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
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            SuccessType = SuccessType.RetrieveDataSuccess
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.SuccessType, Is.EqualTo(SuccessType.RetrieveDataSuccess));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.SuccessType, Is.EqualTo(SuccessType.CreateSuccess));
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
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.SuccessType, Is.EqualTo(original.SuccessType));
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
            SuccessType = SuccessType.CreateSuccess
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponse"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    [Test]
    public void ToString_WithRetrieveDataSuccess_ShouldContainCorrectSuccessType()
    {
        // Arrange
        var response = new SuccessResponse
        {
            Message = "Data retrieved",
            SuccessType = SuccessType.RetrieveDataSuccess
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
            SuccessType = SuccessType.CreateSuccess
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
            SuccessType = SuccessType.CreateSuccess
        };

        // Assert
        Assert.That(response, Is.InstanceOf<Frametux.Shared.Core.Driving.Responses.BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeTrue()
    {
        // Arrange
        var response1 = new SuccessResponse
        {
            Message = "Test message 1",
            SuccessType = SuccessType.CreateSuccess
        };
        var response2 = new SuccessResponse
        {
            Message = "Test message 2",
            SuccessType = SuccessType.RetrieveDataSuccess
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.True);
        Assert.That(response2.IsSuccess, Is.True);
    }

    #endregion
}

