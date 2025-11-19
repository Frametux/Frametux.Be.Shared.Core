using System.Text.Json;
using Frametux.Shared.Core.Driving.Responses;
using Frametux.Shared.Core.Driving.Responses.Success;

namespace UnitTest.Driving.Response;

[TestFixture]
public class SuccessResponseWithDataTest
{
    #region Test Helper Classes

    private record TestDataObject(string Name, int Value);
    
    private record ComplexTestData
    {
        public int Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public List<string> Tags { get; init; } = new();
    }

    #endregion

    #region Constructor and Inheritance Tests

    [Test]
    public void Constructor_WithIntData_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Data retrieved successfully";
        const ResponseType expectedType = ResponseType.RetrieveDataSuccess;
        const int expectedData = 42;

        // Act
        var response = new SuccessResponseWithData<int>
        {
            Message = expectedMessage,
            Type = expectedType,
            Data = expectedData
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
        Assert.That(response.Data, Is.EqualTo(expectedData));
    }

    [Test]
    public void Constructor_WithStringData_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "User created successfully";
        const ResponseType expectedType = ResponseType.CreateSuccess;
        const string expectedData = "Test data string";

        // Act
        var response = new SuccessResponseWithData<string>
        {
            Message = expectedMessage,
            Type = expectedType,
            Data = expectedData
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
        Assert.That(response.Data, Is.EqualTo(expectedData));
    }

    [Test]
    public void Constructor_WithComplexObjectData_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Object retrieved";
        const ResponseType expectedType = ResponseType.RetrieveDataSuccess;
        var expectedData = new TestDataObject("Test", 100);

        // Act
        var response = new SuccessResponseWithData<TestDataObject>
        {
            Message = expectedMessage,
            Type = expectedType,
            Data = expectedData
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.Type, Is.EqualTo(expectedType));
        Assert.That(response.Data, Is.EqualTo(expectedData));
        Assert.That(response.Data.Name, Is.EqualTo("Test"));
        Assert.That(response.Data.Value, Is.EqualTo(100));
    }

    [Test]
    public void Constructor_ShouldSetIsSuccessToTrue()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Success message",
            Type = ResponseType.CreateSuccess,
            Data = 123
        };

        // Assert
        Assert.That(response.IsSuccess, Is.True);
    }

    #endregion

    #region Data Property Tests

    [Test]
    public void Data_WithIntValue_ShouldReturnCorrectValue()
    {
        // Arrange
        const int expectedData = 999;

        // Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(expectedData));
    }

    [Test]
    public void Data_WithStringValue_ShouldReturnCorrectValue()
    {
        // Arrange
        const string expectedData = "Test data value";

        // Act
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(expectedData));
    }

    [Test]
    public void Data_WithEmptyString_ShouldAllowEmptyString()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = string.Empty
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Data_WithNullString_ShouldAllowNull()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string?>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = null
        };

        // Assert
        Assert.That(response.Data, Is.Null);
    }

    [Test]
    public void Data_WithList_ShouldReturnCorrectList()
    {
        // Arrange
        var expectedData = new List<string> { "item1", "item2", "item3" };

        // Act
        var response = new SuccessResponseWithData<List<string>>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(expectedData));
        Assert.That(response.Data.Count, Is.EqualTo(3));
        Assert.That(response.Data[0], Is.EqualTo("item1"));
    }

    [Test]
    public void Data_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var expectedData = new List<int>();

        // Act
        var response = new SuccessResponseWithData<List<int>>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.Empty);
    }

    [Test]
    public void Data_WithComplexObject_ShouldReturnCorrectObject()
    {
        // Arrange
        var expectedData = new ComplexTestData
        {
            Id = 42,
            Description = "Test description",
            Tags = new List<string> { "tag1", "tag2" }
        };

        // Act
        var response = new SuccessResponseWithData<ComplexTestData>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(expectedData));
        Assert.That(response.Data.Id, Is.EqualTo(42));
        Assert.That(response.Data.Description, Is.EqualTo("Test description"));
        Assert.That(response.Data.Tags.Count, Is.EqualTo(2));
    }

    [Test]
    public void Data_WithBoolValue_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var responseTrue = new SuccessResponseWithData<bool>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = true
        };
        var responseFalse = new SuccessResponseWithData<bool>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = false
        };

        // Assert
        Assert.That(responseTrue.Data, Is.True);
        Assert.That(responseFalse.Data, Is.False);
    }

    [Test]
    public void Data_WithDoubleValue_ShouldReturnCorrectValue()
    {
        // Arrange
        const double expectedData = 123.456;

        // Act
        var response = new SuccessResponseWithData<double>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = expectedData
        };

        // Assert
        Assert.That(response.Data, Is.EqualTo(expectedData));
    }

    #endregion

    #region ResponseType Property Tests

    [Test]
    public void ResponseType_WithRetrieveDataSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = 42
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
    }

    [Test]
    public void ResponseType_WithCreateSuccess_ShouldReturnCorrectValue()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = "test"
        };

        // Assert
        Assert.That(response.Type, Is.EqualTo(ResponseType.CreateSuccess));
    }

    #endregion

    #region Message Property Tests

    [Test]
    public void Message_WhenSet_ShouldReturnSameValue()
    {
        // Arrange
        const string expectedMessage = "Operation completed successfully";

        // Act
        var response = new SuccessResponseWithData<int>
        {
            Message = expectedMessage,
            Type = ResponseType.CreateSuccess,
            Data = 100
        };

        // Assert
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Message_WithEmptyString_ShouldAllowEmptyString()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string>
        {
            Message = string.Empty,
            Type = ResponseType.RetrieveDataSuccess,
            Data = "test data"
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
        var response = new SuccessResponseWithData<int>
        {
            Message = longMessage,
            Type = ResponseType.CreateSuccess,
            Data = 42
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
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentData_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 100
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Message 1",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<int>
        {
            Message = "Message 2",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentResponseType_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = 42
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithComplexObjectData_SamValues_ShouldBeEqual()
    {
        // Arrange
        var data1 = new TestDataObject("Test", 100);
        var data2 = new TestDataObject("Test", 100);
        
        var response1 = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = data1
        };
        var response2 = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = data2
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
    }

    [Test]
    public void Equality_WithComplexObjectData_DifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var data1 = new TestDataObject("Test1", 100);
        var data2 = new TestDataObject("Test2", 200);
        
        var response1 = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = data1
        };
        var response2 = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = data2
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
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
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = "test data"
        };
        var sameReference = response;

        // Act & Assert
        Assert.That(response, Is.EqualTo(sameReference));
        // ReSharper disable once EqualExpressionComparison
        Assert.That(response == sameReference, Is.True);
        Assert.That(ReferenceEquals(response, sameReference), Is.True);
    }

    [Test]
    public void Equality_WithAllPropertiesDifferent_ShouldNotBeEqual()
    {
        // Arrange
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Message 1",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<int>
        {
            Message = "Message 2",
            Type = ResponseType.RetrieveDataSuccess,
            Data = 100
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    #endregion

    #region Record With Expression Tests

    [Test]
    public void WithExpression_ModifyingData_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponseWithData<int>
        {
            Message = "Original message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var modified = original with { Data = 100 };

        // Assert
        Assert.That(modified.Data, Is.EqualTo(100));
        Assert.That(original.Data, Is.EqualTo(42));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(modified.Type, Is.EqualTo(original.Type));
        Assert.That(modified.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingMessage_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponseWithData<string>
        {
            Message = "Original message",
            Type = ResponseType.CreateSuccess,
            Data = "test data"
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.Data, Is.EqualTo(original.Data));
        Assert.That(modified.Type, Is.EqualTo(original.Type));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingResponseType_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var modified = original with { Type = ResponseType.RetrieveDataSuccess };

        // Assert
        Assert.That(modified.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(original.Type, Is.EqualTo(ResponseType.CreateSuccess));
        Assert.That(modified.Data, Is.EqualTo(original.Data));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingAllProperties_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponseWithData<string>
        {
            Message = "Original message",
            Type = ResponseType.CreateSuccess,
            Data = "original data"
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = "modified data"
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(modified.Data, Is.EqualTo("modified data"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.Type, Is.EqualTo(ResponseType.CreateSuccess));
        Assert.That(original.Data, Is.EqualTo("original data"));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_WithoutChanges_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.Type, Is.EqualTo(original.Type));
        Assert.That(copy.Data, Is.EqualTo(original.Data));
        Assert.That(copy.IsSuccess, Is.EqualTo(original.IsSuccess));
        Assert.That(copy, Is.EqualTo(original));
        Assert.That(ReferenceEquals(original, copy), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingComplexObjectData_ShouldCreateNewInstance()
    {
        // Arrange
        var originalData = new TestDataObject("Original", 100);
        var modifiedData = new TestDataObject("Modified", 200);
        
        var original = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = originalData
        };

        // Act
        var modified = original with { Data = modifiedData };

        // Assert
        Assert.That(modified.Data, Is.EqualTo(modifiedData));
        Assert.That(original.Data, Is.EqualTo(originalData));
        Assert.That(modified.Data.Name, Is.EqualTo("Modified"));
        Assert.That(original.Data.Name, Is.EqualTo("Original"));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    #endregion

    #region ToString Tests

    [Test]
    public void ToString_WithIntData_ShouldContainAllPropertyValues()
    {
        // Arrange
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponseWithData"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("CreateSuccess"));
        Assert.That(result, Does.Contain("42"));
    }

    [Test]
    public void ToString_WithStringData_ShouldContainDataValue()
    {
        // Arrange
        var response = new SuccessResponseWithData<string>
        {
            Message = "Data retrieved",
            Type = ResponseType.RetrieveDataSuccess,
            Data = "test data value"
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponseWithData"));
        Assert.That(result, Does.Contain("Data retrieved"));
        Assert.That(result, Does.Contain("RetrieveDataSuccess"));
        Assert.That(result, Does.Contain("test data value"));
    }

    [Test]
    public void ToString_WithComplexObjectData_ShouldContainObjectRepresentation()
    {
        // Arrange
        var data = new TestDataObject("TestName", 999);
        var response = new SuccessResponseWithData<TestDataObject>
        {
            Message = "Object created",
            Type = ResponseType.CreateSuccess,
            Data = data
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponseWithData"));
        Assert.That(result, Does.Contain("Object created"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    [Test]
    public void ToString_WithNullData_ShouldHandleGracefully()
    {
        // Arrange
        var response = new SuccessResponseWithData<string?>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = null
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponseWithData"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    [Test]
    public void ToString_WithEmptyStringData_ShouldHandleGracefully()
    {
        // Arrange
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = string.Empty
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("SuccessResponseWithData"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("CreateSuccess"));
    }

    #endregion

    #region JSON Serialization Tests

    [Test]
    public void JsonSerialization_WithIntData_ShouldSerializeCorrectly()
    {
        // Arrange
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"Data\":42"));
        Assert.That(json, Does.Contain("\"Message\":\"Test message\""));
        Assert.That(json, Does.Contain("\"CreateSuccess\""));
        Assert.That(json, Does.Contain("\"IsSuccess\":true"));
    }

    [Test]
    public void JsonDeserialization_WithIntData_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Type\":\"CreateSuccess\",\"Message\":\"Test message\",\"Data\":42}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponseWithData<int>>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Data, Is.EqualTo(42));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.Type, Is.EqualTo(ResponseType.CreateSuccess));
        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void JsonSerialization_WithStringData_ShouldSerializeCorrectly()
    {
        // Arrange
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = "test data"
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"Data\":\"test data\""));
        Assert.That(json, Does.Contain("\"Message\":\"Test message\""));
        Assert.That(json, Does.Contain("\"RetrieveDataSuccess\""));
    }

    [Test]
    public void JsonDeserialization_WithStringData_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Type\":\"RetrieveDataSuccess\",\"Message\":\"Test message\",\"Data\":\"test data\"}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponseWithData<string>>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Data, Is.EqualTo("test data"));
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
    }

    [Test]
    public void JsonSerialization_WithComplexObjectData_ShouldSerializeCorrectly()
    {
        // Arrange
        var data = new ComplexTestData
        {
            Id = 100,
            Description = "Test description",
            Tags = new List<string> { "tag1", "tag2" }
        };
        var response = new SuccessResponseWithData<ComplexTestData>
        {
            Message = "Complex data",
            Type = ResponseType.CreateSuccess,
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"Id\":100"));
        Assert.That(json, Does.Contain("\"Description\":\"Test description\""));
        Assert.That(json, Does.Contain("\"tag1\""));
        Assert.That(json, Does.Contain("\"tag2\""));
        Assert.That(json, Does.Contain("\"Message\":\"Complex data\""));
    }

    [Test]
    public void JsonDeserialization_WithComplexObjectData_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Type\":\"CreateSuccess\",\"Message\":\"Complex data\",\"Data\":{\"Id\":100,\"Description\":\"Test description\",\"Tags\":[\"tag1\",\"tag2\"]}}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponseWithData<ComplexTestData>>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Data, Is.Not.Null);
        Assert.That(response.Data.Id, Is.EqualTo(100));
        Assert.That(response.Data.Description, Is.EqualTo("Test description"));
        Assert.That(response.Data.Tags, Has.Count.EqualTo(2));
        Assert.That(response.Data.Tags[0], Is.EqualTo("tag1"));
    }

    [Test]
    public void JsonSerialization_WithListData_ShouldSerializeCorrectly()
    {
        // Arrange
        var data = new List<string> { "item1", "item2", "item3" };
        var response = new SuccessResponseWithData<List<string>>
        {
            Message = "List data",
            Type = ResponseType.RetrieveDataSuccess,
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"Data\":[\"item1\",\"item2\",\"item3\"]"));
        Assert.That(json, Does.Contain("\"Message\":\"List data\""));
    }

    [Test]
    public void JsonDeserialization_WithListData_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = "{\"IsSuccess\":true,\"Type\":\"RetrieveDataSuccess\",\"Message\":\"List data\",\"Data\":[\"item1\",\"item2\",\"item3\"]}";

        // Act
        var response = JsonSerializer.Deserialize<SuccessResponseWithData<List<string>>>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Data, Has.Count.EqualTo(3));
        Assert.That(response.Data[0], Is.EqualTo("item1"));
        Assert.That(response.Data[2], Is.EqualTo("item3"));
    }

    [Test]
    public void JsonSerialization_ResponseType_ShouldSerializeAsString()
    {
        // Arrange
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"CreateSuccess\""));
        Assert.That(json, Does.Not.Contain("\"Type\":1"));
    }

    [Test]
    public void JsonRoundTrip_WithIntData_ShouldMaintainEquality()
    {
        // Arrange
        var original = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<SuccessResponseWithData<int>>(json);

        // Assert
        Assert.That(deserialized, Is.EqualTo(original));
    }

    [Test]
    public void JsonRoundTrip_WithStringData_ShouldMaintainEquality()
    {
        // Arrange
        var original = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = "test data"
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<SuccessResponseWithData<string>>(json);

        // Assert
        Assert.That(deserialized, Is.EqualTo(original));
    }

    #endregion

    #region Inheritance Tests

    [Test]
    public void Inheritance_ShouldInheritFromSuccessResponse()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Assert
        Assert.That(response, Is.InstanceOf<SuccessResponse>());
    }

    [Test]
    public void Inheritance_ShouldInheritFromBaseResponse()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = "test"
        };

        // Assert
        Assert.That(response, Is.InstanceOf<Frametux.Shared.Core.Driving.Responses.BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeTrue()
    {
        // Arrange
        var response1 = new SuccessResponseWithData<int>
        {
            Message = "Test message 1",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var response2 = new SuccessResponseWithData<string>
        {
            Message = "Test message 2",
            Type = ResponseType.RetrieveDataSuccess,
            Data = "test"
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.True);
        Assert.That(response2.IsSuccess, Is.True);
    }

    [Test]
    public void Inheritance_ShouldHaveAllBaseResponseProperties()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Test message",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };

        // Assert - can access BaseResponse properties
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.Not.Null);
    }

    [Test]
    public void Inheritance_ShouldHaveAllSuccessResponseProperties()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<double>
        {
            Message = "Test message",
            Type = ResponseType.RetrieveDataSuccess,
            Data = 3.14
        };

        // Assert - can access SuccessResponse properties
        Assert.That(response.Type, Is.EqualTo(ResponseType.RetrieveDataSuccess));
        Assert.That(response.IsSuccess, Is.True);
        Assert.That(response.Message, Is.Not.Null);
    }

    #endregion

    #region Generic Type Variations Tests

    [Test]
    public void GenericType_WithInt_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<int>
        {
            Message = "Integer data",
            Type = ResponseType.CreateSuccess,
            Data = 12345
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<int>());
        Assert.That(response.Data, Is.EqualTo(12345));
    }

    [Test]
    public void GenericType_WithString_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<string>
        {
            Message = "String data",
            Type = ResponseType.CreateSuccess,
            Data = "test string"
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<string>());
        Assert.That(response.Data, Is.EqualTo("test string"));
    }

    [Test]
    public void GenericType_WithBool_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<bool>
        {
            Message = "Boolean data",
            Type = ResponseType.CreateSuccess,
            Data = true
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<bool>());
        Assert.That(response.Data, Is.True);
    }

    [Test]
    public void GenericType_WithDouble_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<double>
        {
            Message = "Double data",
            Type = ResponseType.CreateSuccess,
            Data = 123.456
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<double>());
        Assert.That(response.Data, Is.EqualTo(123.456).Within(0.001));
    }

    [Test]
    public void GenericType_WithDecimal_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new SuccessResponseWithData<decimal>
        {
            Message = "Decimal data",
            Type = ResponseType.CreateSuccess,
            Data = 999.99m
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<decimal>());
        Assert.That(response.Data, Is.EqualTo(999.99m));
    }

    [Test]
    public void GenericType_WithGuid_ShouldWorkCorrectly()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var response = new SuccessResponseWithData<Guid>
        {
            Message = "Guid data",
            Type = ResponseType.CreateSuccess,
            Data = guid
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<Guid>());
        Assert.That(response.Data, Is.EqualTo(guid));
    }

    [Test]
    public void GenericType_WithDateTime_ShouldWorkCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);

        // Act
        var response = new SuccessResponseWithData<DateTime>
        {
            Message = "DateTime data",
            Type = ResponseType.CreateSuccess,
            Data = dateTime
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<DateTime>());
        Assert.That(response.Data, Is.EqualTo(dateTime));
    }

    [Test]
    public void GenericType_WithListOfIntegers_ShouldWorkCorrectly()
    {
        // Arrange
        var data = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var response = new SuccessResponseWithData<List<int>>
        {
            Message = "List of integers",
            Type = ResponseType.RetrieveDataSuccess,
            Data = data
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<List<int>>());
        Assert.That(response.Data, Has.Count.EqualTo(5));
        Assert.That(response.Data[0], Is.EqualTo(1));
    }

    [Test]
    public void GenericType_WithDictionary_ShouldWorkCorrectly()
    {
        // Arrange
        var data = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        // Act
        var response = new SuccessResponseWithData<Dictionary<string, int>>
        {
            Message = "Dictionary data",
            Type = ResponseType.RetrieveDataSuccess,
            Data = data
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<Dictionary<string, int>>());
        Assert.That(response.Data, Has.Count.EqualTo(3));
        Assert.That(response.Data["one"], Is.EqualTo(1));
    }

    [Test]
    public void GenericType_WithArray_ShouldWorkCorrectly()
    {
        // Arrange
        var data = new[] { "a", "b", "c" };

        // Act
        var response = new SuccessResponseWithData<string[]>
        {
            Message = "Array data",
            Type = ResponseType.RetrieveDataSuccess,
            Data = data
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<string[]>());
        Assert.That(response.Data, Has.Length.EqualTo(3));
        Assert.That(response.Data[0], Is.EqualTo("a"));
    }

    [Test]
    public void GenericType_WithComplexCustomObject_ShouldWorkCorrectly()
    {
        // Arrange
        var data = new ComplexTestData
        {
            Id = 999,
            Description = "Complex object",
            Tags = new List<string> { "tag1", "tag2", "tag3" }
        };

        // Act
        var response = new SuccessResponseWithData<ComplexTestData>
        {
            Message = "Complex object data",
            Type = ResponseType.CreateSuccess,
            Data = data
        };

        // Assert
        Assert.That(response.Data, Is.TypeOf<ComplexTestData>());
        Assert.That(response.Data.Id, Is.EqualTo(999));
        Assert.That(response.Data.Description, Is.EqualTo("Complex object"));
        Assert.That(response.Data.Tags, Has.Count.EqualTo(3));
    }

    [Test]
    public void GenericType_WithNullableValueType_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var responseWithValue = new SuccessResponseWithData<int?>
        {
            Message = "Nullable with value",
            Type = ResponseType.CreateSuccess,
            Data = 42
        };
        var responseWithNull = new SuccessResponseWithData<int?>
        {
            Message = "Nullable with null",
            Type = ResponseType.CreateSuccess,
            Data = null
        };

        // Assert
        Assert.That(responseWithValue.Data, Is.EqualTo(42));
        Assert.That(responseWithNull.Data, Is.Null);
    }

    #endregion
}

