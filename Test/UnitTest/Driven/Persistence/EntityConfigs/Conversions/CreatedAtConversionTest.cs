using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Driven.Persistence.EntityConfigs.Conversions;

[TestFixture]
public class CreatedAtConversionTest
{
    #region Non-Nullable Conversion Tests

    [Test]
    public void HasCreatedAtConversion_ConversionToDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-1);
        var createdAt = new CreatedAt(testDateTime);

        // Act - Simulate the conversion delegate: i => i.Value
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasCreatedAtConversion_ConversionFromDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-2);

        // Act - Simulate the conversion delegate: i => new CreatedAt(i)
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result.Value, Is.EqualTo(testDateTime));
        Assert.That((DateTime)result, Is.EqualTo(testDateTime));
    }

    [Test]
    public void HasCreatedAtConversion_RoundTripConversion_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow.AddDays(-1);
        var createdAt = new CreatedAt(originalDateTime);

        // Act - Simulate both conversions
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(originalDateTime));
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(createdAt.Value));
    }

    [Test]
    public void HasCreatedAtConversion_WithPastDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var pastDateTime = DateTime.UtcNow.AddYears(-5);
        var createdAt = new CreatedAt(pastDateTime);

        // Act
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result, Is.EqualTo(pastDateTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Nullable Conversion Tests

    [Test]
    public void HasCreatedAtNullableConversion_ConversionToDateTime_WithValue_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-3);
        var createdAt = new CreatedAt(testDateTime);

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<CreatedAt?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
        Assert.That(result!.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasCreatedAtNullableConversion_ConversionToDateTime_WithNull_ShouldReturnNull()
    {
        // Arrange
        CreatedAt? createdAt = null;

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<CreatedAt?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasCreatedAtNullableConversion_ConversionFromDateTime_WithValue_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-4);

        // Act - Simulate the conversion delegate: i => i != null ? new CreatedAt(i.Value) : null
        Func<DateTime?, CreatedAt?> convertFromDb = i => i != null ? new CreatedAt(i.Value) : null;
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(testDateTime));
    }

    [Test]
    public void HasCreatedAtNullableConversion_ConversionFromDateTime_WithNull_ShouldReturnNull()
    {
        // Arrange
        DateTime? testDateTime = null;

        // Act - Simulate the conversion delegate: i => i != null ? new CreatedAt(i.Value) : null
        Func<DateTime?, CreatedAt?> convertFromDb = i => i != null ? new CreatedAt(i.Value) : null;
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasCreatedAtNullableConversion_RoundTripConversion_WithValue_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow.AddMonths(-6);
        var createdAt = new CreatedAt(originalDateTime);

        // Act - Simulate both conversions
        Func<CreatedAt?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        Func<DateTime?, CreatedAt?> convertFromDb = i => i != null ? new CreatedAt(i.Value) : null;

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredCreatedAt, Is.Not.Null);
        Assert.That(restoredCreatedAt!.Value, Is.EqualTo(originalDateTime));
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(createdAt.Value));
    }

    [Test]
    public void HasCreatedAtNullableConversion_RoundTripConversion_WithNull_ShouldPreserveNull()
    {
        // Arrange
        CreatedAt? createdAt = null;

        // Act - Simulate both conversions
        Func<CreatedAt?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        Func<DateTime?, CreatedAt?> convertFromDb = i => i != null ? new CreatedAt(i.Value) : null;

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.Null);
        Assert.That(restoredCreatedAt, Is.Null);
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void HasCreatedAtConversion_WithUnspecifiedKind_ShouldConvertToUtc()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

        // Act - CreatedAt should convert unspecified to UTC
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);
        var createdAt = convertFromDb(unspecifiedDateTime);
        
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasCreatedAtConversion_WithLocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 6, 15, 10, 30, 0, DateTimeKind.Local);

        // Act - CreatedAt should convert local to UTC
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);
        var createdAt = convertFromDb(localDateTime);
        
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasCreatedAtConversion_WithMinDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var minDateTime = DateTime.MinValue.ToUniversalTime();
        var createdAt = new CreatedAt(minDateTime);

        // Act
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(minDateTime));
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(minDateTime));
    }

    [Test]
    public void HasCreatedAtConversion_WithRecentPastDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var recentPast = DateTime.UtcNow.AddMinutes(-5);
        var createdAt = new CreatedAt(recentPast);

        // Act
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(recentPast));
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(recentPast));
        Assert.That(restoredCreatedAt.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasCreatedAtConversion_WithParameterlessConstructor_ShouldConvertCorrectly()
    {
        // Arrange
        var createdAt = new CreatedAt();

        // Act
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(createdAt);

        // Assert
        Assert.That(result, Is.Not.EqualTo(default(DateTime)));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void HasCreatedAtConversion_WithVeryOldDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var veryOldDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var createdAt = new CreatedAt(veryOldDate);

        // Act
        Func<CreatedAt, DateTime> convertToDb = i => i.Value;
        Func<DateTime, CreatedAt> convertFromDb = i => new CreatedAt(i);

        var dbValue = convertToDb(createdAt);
        var restoredCreatedAt = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(veryOldDate));
        Assert.That(restoredCreatedAt.Value, Is.EqualTo(veryOldDate));
        Assert.That(restoredCreatedAt.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion
}

