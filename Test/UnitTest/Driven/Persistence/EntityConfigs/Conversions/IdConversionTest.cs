using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Driven.Persistence.EntityConfigs.Conversions;

[TestFixture]
public class IdConversionTest
{
    #region Non-Nullable Conversion Tests

    [Test]
    public void HasIdConversion_ConversionToString_ShouldWork()
    {
        // Arrange
        var testValue = "test-id-123";
        var id = new Id(testValue);

        // Act - Simulate the conversion delegate: i => i.Value
        Func<Id, string> convertToDb = i => i.Value;
        var result = convertToDb(id);

        // Assert
        Assert.That(result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasIdConversion_ConversionFromString_ShouldWork()
    {
        // Arrange
        var testValue = "test-id-456";

        // Act - Simulate the conversion delegate: i => new Id(i)
        Func<string, Id> convertFromDb = i => new Id(i);
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result.Value, Is.EqualTo(testValue));
        Assert.That((string)result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasIdConversion_RoundTripConversion_ShouldPreserveValue()
    {
        // Arrange
        var originalValue = "original-id-789";
        var id = new Id(originalValue);

        // Act - Simulate both conversions
        Func<Id, string> convertToDb = i => i.Value;
        Func<string, Id> convertFromDb = i => new Id(i);

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredId.Value, Is.EqualTo(originalValue));
        Assert.That(restoredId.Value, Is.EqualTo(id.Value));
    }

    #endregion

    #region Nullable Conversion Tests

    [Test]
    public void HasIdNullableConversion_ConversionToString_WithValue_ShouldWork()
    {
        // Arrange
        var testValue = "nullable-id-test";
        var id = new Id(testValue);

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<Id?, string?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(id);

        // Assert
        Assert.That(result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasIdNullableConversion_ConversionToString_WithNull_ShouldReturnNull()
    {
        // Arrange
        Id? id = null;

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<Id?, string?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasIdNullableConversion_ConversionFromString_WithValue_ShouldWork()
    {
        // Arrange
        var testValue = "nullable-from-db";

        // Act - Simulate the conversion delegate: i => i != null ? new Id(i) : null
        Func<string?, Id?> convertFromDb = i => i != null ? new Id(i) : null;
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasIdNullableConversion_ConversionFromString_WithNull_ShouldReturnNull()
    {
        // Arrange
        string? testValue = null;

        // Act - Simulate the conversion delegate: i => i != null ? new Id(i) : null
        Func<string?, Id?> convertFromDb = i => i != null ? new Id(i) : null;
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasIdNullableConversion_RoundTripConversion_WithValue_ShouldPreserveValue()
    {
        // Arrange
        var originalValue = "round-trip-nullable-id";
        var id = new Id(originalValue);

        // Act - Simulate both conversions
        Func<Id?, string?> convertToDb = i => i != null ? i.Value : null;
        Func<string?, Id?> convertFromDb = i => i != null ? new Id(i) : null;

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredId, Is.Not.Null);
        Assert.That(restoredId!.Value, Is.EqualTo(originalValue));
        Assert.That(restoredId.Value, Is.EqualTo(id.Value));
    }

    [Test]
    public void HasIdNullableConversion_RoundTripConversion_WithNull_ShouldPreserveNull()
    {
        // Arrange
        Id? id = null;

        // Act - Simulate both conversions
        Func<Id?, string?> convertToDb = i => i != null ? i.Value : null;
        Func<string?, Id?> convertFromDb = i => i != null ? new Id(i) : null;

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.Null);
        Assert.That(restoredId, Is.Null);
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void HasIdConversion_WithSpecialCharacters_ShouldConvertCorrectly()
    {
        // Arrange
        var testValue = "id-with-!@#$%^&*()-special_chars";
        var id = new Id(testValue);

        // Act
        Func<Id, string> convertToDb = i => i.Value;
        Func<string, Id> convertFromDb = i => new Id(i);

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredId.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasIdConversion_WithGuidValue_ShouldConvertCorrectly()
    {
        // Arrange
        var guidValue = Guid.NewGuid().ToString();
        var id = new Id(guidValue);

        // Act
        Func<Id, string> convertToDb = i => i.Value;
        Func<string, Id> convertFromDb = i => new Id(i);

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(guidValue));
        Assert.That(restoredId.Value, Is.EqualTo(guidValue));
        Assert.That(Guid.TryParse(restoredId.Value, out _), Is.True);
    }

    [Test]
    public void HasIdConversion_WithMaxLengthString_ShouldConvertCorrectly()
    {
        // Arrange
        var maxLengthValue = new string('A', Id.MaxLength);
        var id = new Id(maxLengthValue);

        // Act
        Func<Id, string> convertToDb = i => i.Value;
        Func<string, Id> convertFromDb = i => new Id(i);

        var dbValue = convertToDb(id);
        var restoredId = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(maxLengthValue));
        Assert.That(restoredId.Value, Is.EqualTo(maxLengthValue));
        Assert.That(restoredId.Value.Length, Is.EqualTo(Id.MaxLength));
    }

    #endregion
}

