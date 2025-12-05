using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Driven.Persistence.EntityConfigs.Conversions;

[TestFixture]
public class UtcDateTimeConversionTest
{
    #region Non-Nullable Conversion Tests

    [Test]
    public void HasUtcDateTimeConversion_ConversionToDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow;
        var utcDateTime = new UtcDateTime(testDateTime);

        // Act - Simulate the conversion delegate: i => i.Value
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasUtcDateTimeConversion_ConversionFromDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(5);

        // Act - Simulate the conversion delegate: i => new UtcDateTime(i)
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result.Value, Is.EqualTo(testDateTime));
        Assert.That((DateTime)result, Is.EqualTo(testDateTime));
    }

    [Test]
    public void HasUtcDateTimeConversion_RoundTripConversion_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow.AddDays(10);
        var utcDateTime = new UtcDateTime(originalDateTime);

        // Act - Simulate both conversions
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(originalDateTime));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(utcDateTime.Value));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithFutureDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var futureDateTime = DateTime.UtcNow.AddYears(1);
        var utcDateTime = new UtcDateTime(futureDateTime);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.EqualTo(futureDateTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithPastDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var pastDateTime = DateTime.UtcNow.AddYears(-1);
        var utcDateTime = new UtcDateTime(pastDateTime);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.EqualTo(pastDateTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Nullable Conversion Tests

    [Test]
    public void HasUtcDateTimeNullableConversion_ConversionToDateTime_WithValue_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(3);
        var utcDateTime = new UtcDateTime(testDateTime);

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<UtcDateTime?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
        Assert.That(result!.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasUtcDateTimeNullableConversion_ConversionToDateTime_WithNull_ShouldReturnNull()
    {
        // Arrange
        UtcDateTime? utcDateTime = null;

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<UtcDateTime?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasUtcDateTimeNullableConversion_ConversionFromDateTime_WithValue_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddMonths(2);

        // Act - Simulate the conversion delegate: i => i != null ? new UtcDateTime(i.Value) : null
        Func<DateTime?, UtcDateTime?> convertFromDb = i => i != null ? new UtcDateTime(i.Value) : null;
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(testDateTime));
    }

    [Test]
    public void HasUtcDateTimeNullableConversion_ConversionFromDateTime_WithNull_ShouldReturnNull()
    {
        // Arrange
        DateTime? testDateTime = null;

        // Act - Simulate the conversion delegate: i => i != null ? new UtcDateTime(i.Value) : null
        Func<DateTime?, UtcDateTime?> convertFromDb = i => i != null ? new UtcDateTime(i.Value) : null;
        var result = convertFromDb(testDateTime);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasUtcDateTimeNullableConversion_RoundTripConversion_WithValue_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow.AddDays(7);
        var utcDateTime = new UtcDateTime(originalDateTime);

        // Act - Simulate both conversions
        Func<UtcDateTime?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        Func<DateTime?, UtcDateTime?> convertFromDb = i => i != null ? new UtcDateTime(i.Value) : null;

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredUtcDateTime, Is.Not.Null);
        Assert.That(restoredUtcDateTime!.Value, Is.EqualTo(originalDateTime));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(utcDateTime.Value));
    }

    [Test]
    public void HasUtcDateTimeNullableConversion_RoundTripConversion_WithNull_ShouldPreserveNull()
    {
        // Arrange
        UtcDateTime? utcDateTime = null;

        // Act - Simulate both conversions
        Func<UtcDateTime?, DateTime?> convertToDb = i => i != null ? i.Value : null;
        Func<DateTime?, UtcDateTime?> convertFromDb = i => i != null ? new UtcDateTime(i.Value) : null;

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.Null);
        Assert.That(restoredUtcDateTime, Is.Null);
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void HasUtcDateTimeConversion_WithUnspecifiedKind_ShouldConvertToUtc()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

        // Act - UtcDateTime should convert unspecified to UTC
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);
        var utcDateTime = convertFromDb(unspecifiedDateTime);
        
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithLocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localDateTime = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Local);

        // Act - UtcDateTime should convert local to UTC
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);
        var utcDateTime = convertFromDb(localDateTime);
        
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithMinDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var minDateTime = DateTime.MinValue.ToUniversalTime();
        var utcDateTime = new UtcDateTime(minDateTime);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(minDateTime));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(minDateTime));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithMaxDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue.ToUniversalTime();
        var utcDateTime = new UtcDateTime(maxDateTime);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(maxDateTime));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(maxDateTime));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithParameterlessConstructor_ShouldConvertCorrectly()
    {
        // Arrange
        var utcDateTime = new UtcDateTime();

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        var result = convertToDb(utcDateTime);

        // Assert
        Assert.That(result, Is.Not.EqualTo(default(DateTime)));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithMidnight_ShouldConvertCorrectly()
    {
        // Arrange
        var midnightDateTime = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var utcDateTime = new UtcDateTime(midnightDateTime);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(midnightDateTime));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(midnightDateTime));
        Assert.That(restoredUtcDateTime.Value.Hour, Is.EqualTo(0));
        Assert.That(restoredUtcDateTime.Value.Minute, Is.EqualTo(0));
        Assert.That(restoredUtcDateTime.Value.Second, Is.EqualTo(0));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithMilliseconds_ShouldPreserveMilliseconds()
    {
        // Arrange
        var dateTimeWithMs = new DateTime(2024, 6, 15, 14, 30, 45, 123, DateTimeKind.Utc);
        var utcDateTime = new UtcDateTime(dateTimeWithMs);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(dateTimeWithMs));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(dateTimeWithMs));
        Assert.That(restoredUtcDateTime.Value.Millisecond, Is.EqualTo(123));
    }

    [Test]
    public void HasUtcDateTimeConversion_WithLeapYearDate_ShouldConvertCorrectly()
    {
        // Arrange
        var leapYearDate = new DateTime(2024, 2, 29, 12, 0, 0, DateTimeKind.Utc);
        var utcDateTime = new UtcDateTime(leapYearDate);

        // Act
        Func<UtcDateTime, DateTime> convertToDb = i => i.Value;
        Func<DateTime, UtcDateTime> convertFromDb = i => new UtcDateTime(i);

        var dbValue = convertToDb(utcDateTime);
        var restoredUtcDateTime = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(leapYearDate));
        Assert.That(restoredUtcDateTime.Value, Is.EqualTo(leapYearDate));
        Assert.That(restoredUtcDateTime.Value.Month, Is.EqualTo(2));
        Assert.That(restoredUtcDateTime.Value.Day, Is.EqualTo(29));
    }

    #endregion
}

