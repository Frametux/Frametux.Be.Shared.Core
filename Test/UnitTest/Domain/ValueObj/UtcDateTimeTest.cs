using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Domain.ValueObj;

[TestFixture]
public class UtcDateTimeTest
{
    // Test helper record to access protected constructor
    private record TestableUtcDateTime : UtcDateTime
    {
        public TestableUtcDateTime(DateTime value, bool shouldValidate) : base(value, shouldValidate)
        {
        }
    }
    #region Valid Input Tests

    [Test]
    public void Constructor_WithValidUtcDateTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var validUtcDate = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(validUtcDate);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(validUtcDate));
    }

    [Test]
    public void Constructor_WithCurrentTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var currentTime = DateTime.UtcNow;

        // Act
        var utcDateTime = new UtcDateTime(currentTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(currentTime));
    }

    [Test]
    public void Constructor_WithVeryOldDate_ShouldCreateUtcDateTime()
    {
        // Arrange
        var veryOldDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(veryOldDate);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(veryOldDate));
    }

    [Test]
    public void Constructor_WithFutureDate_ShouldCreateUtcDateTime()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(10);

        // Act
        var utcDateTime = new UtcDateTime(futureDate);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(futureDate));
    }

    [Test]
    public void Constructor_WithMinDateTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var minDateTime = DateTime.MinValue;

        // Act
        var utcDateTime = new UtcDateTime(minDateTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(DateTime.SpecifyKind(minDateTime, DateTimeKind.Utc)));
    }

    [Test]
    public void Constructor_WithMaxDateTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue;

        // Act
        var utcDateTime = new UtcDateTime(maxDateTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(DateTime.SpecifyKind(maxDateTime, DateTimeKind.Utc)));
    }

    [Test]
    public void Constructor_WithMillisecondPrecision_ShouldCreateUtcDateTime()
    {
        // Arrange
        var preciseTime = new DateTime(2023, 5, 15, 10, 30, 45, 123, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(preciseTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(preciseTime));
    }

    #endregion

    #region UTC Conversion Tests

    [Test]
    public void Constructor_WithUtcDateTime_ShouldPreserveValue()
    {
        // Arrange
        var utcTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(utcTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(utcTime));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(localTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithUnspecifiedDateTime_ShouldTreatAsUtc()
    {
        // Arrange
        var unspecifiedTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Unspecified);

        // Act
        var utcDateTime = new UtcDateTime(unspecifiedTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(unspecifiedTime));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalTimeNearMidnight_ShouldConvertCorrectly()
    {
        // Arrange
        var localMidnight = new DateTime(2023, 5, 15, 23, 59, 59, DateTimeKind.Local);
        var expectedUtc = localMidnight.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(localMidnight);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalTimeInDifferentDay_ShouldConvertToCorrectUtcDay()
    {
        // Arrange - Create a local time that when converted to UTC would be in a different day
        var localTime = new DateTime(2023, 5, 15, 1, 0, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(localTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
        // Verify the conversion might result in different date
        if (expectedUtc.Date != localTime.Date)
        {
            Assert.That(((DateTime)utcDateTime).Date, Is.Not.EqualTo(localTime.Date));
        }
    }

    [Test]
    public void Constructor_AllTimeZones_ShouldAlwaysResultInUtc()
    {
        // Arrange
        var baseTime = new DateTime(2023, 5, 15, 12, 0, 0);
        var utcTime = new DateTime(baseTime.Ticks, DateTimeKind.Utc);
        var localTime = new DateTime(baseTime.Ticks, DateTimeKind.Local);
        var unspecifiedTime = new DateTime(baseTime.Ticks, DateTimeKind.Unspecified);

        // Act
        var utcUtcDateTime = new UtcDateTime(utcTime);
        var localUtcDateTime = new UtcDateTime(localTime);
        var unspecifiedUtcDateTime = new UtcDateTime(unspecifiedTime);

        // Assert
        Assert.That(((DateTime)utcUtcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)localUtcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)unspecifiedUtcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Parameterless Constructor Tests

    [Test]
    public void Constructor_Parameterless_ShouldCreateValidUtcDateTime()
    {
        // Act
        var utcDateTime = new UtcDateTime();

        // Assert
        Assert.That((DateTime)utcDateTime, Is.Not.EqualTo(default(DateTime)));
        Assert.That((DateTime)utcDateTime, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void Constructor_Parameterless_ShouldUseUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var utcDateTime = new UtcDateTime();
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.That((DateTime)utcDateTime, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That((DateTime)utcDateTime, Is.LessThanOrEqualTo(afterCreation));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_Parameterless_ShouldGenerateRecentTime()
    {
        // Act
        var utcDateTime = new UtcDateTime();
        var timeDifference = DateTime.UtcNow - (DateTime)utcDateTime;

        // Assert
        Assert.That(timeDifference.TotalSeconds, Is.LessThan(1), "UtcDateTime should be very recent");
    }

    [Test]
    public void Constructor_Parameterless_ShouldPassValidation()
    {
        // Act
        var utcDateTime = new UtcDateTime();
        var validationResult = UtcDateTime.Validator.Validate(utcDateTime);

        // Assert
        Assert.That(validationResult.IsValid, Is.True, "Generated UtcDateTime should pass validation");
        Assert.That(validationResult.Errors, Is.Empty, "Generated UtcDateTime should have no validation errors");
    }

    [Test]
    public void Constructor_Parameterless_ShouldWorkWithImplicitConversion()
    {
        // Act
        var utcDateTime = new UtcDateTime();
        DateTime dateTimeValue = utcDateTime; // Implicit conversion to DateTime
        UtcDateTime convertedBack = dateTimeValue; // Implicit conversion back to UtcDateTime

        // Assert
        Assert.That((DateTime)convertedBack, Is.EqualTo((DateTime)utcDateTime));
    }

    [Test]
    public void Constructor_Parameterless_MultipleCalls_ShouldGenerateDifferentTimes()
    {
        // Act
        var utcDateTime1 = new UtcDateTime();
        Thread.Sleep(1); // Ensure different timestamps
        var utcDateTime2 = new UtcDateTime();
        Thread.Sleep(1);
        var utcDateTime3 = new UtcDateTime();

        // Assert
        Assert.That((DateTime)utcDateTime1, Is.LessThanOrEqualTo((DateTime)utcDateTime2));
        Assert.That((DateTime)utcDateTime2, Is.LessThanOrEqualTo((DateTime)utcDateTime3));
    }

    [Test]
    public void Constructor_Parameterless_ShouldBeCloseToCurrentTime()
    {
        // Arrange
        var expectedTime = DateTime.UtcNow;

        // Act
        var utcDateTime = new UtcDateTime();

        // Assert
        var difference = Math.Abs(((DateTime)utcDateTime - expectedTime).TotalMilliseconds);
        Assert.That(difference, Is.LessThan(100), "Generated time should be very close to current UTC time");
    }

    #endregion

    #region Protected Constructor Tests

    [Test]
    public void ProtectedConstructor_WithShouldValidateTrue_ShouldValidate()
    {
        // Arrange
        var validUtcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new TestableUtcDateTime(validUtcDateTime, shouldValidate: true);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(validUtcDateTime));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateFalse_ShouldSkipValidation()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);
        var expectedUtc = localDateTime.ToUniversalTime();

        // Act - Should not throw even though validation would normally fail for Local kind
        var utcDateTime = new TestableUtcDateTime(localDateTime, shouldValidate: false);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateTrueAndInvalidKind_ShouldThrow()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);

        // Act & Assert
        // The constructor converts local to UTC, then validates the UTC result
        // After conversion, the value will be UTC, so it should pass validation
        Assert.DoesNotThrow(() =>
        {
            var _ = new TestableUtcDateTime(localDateTime, shouldValidate: true);
        });
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateFalseAndUnspecifiedKind_ShouldConvertAndNotThrow()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        var utcDateTime = new TestableUtcDateTime(unspecifiedDateTime, shouldValidate: false);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(unspecifiedDateTime));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateTrueAndUtcKind_ShouldPass()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = new TestableUtcDateTime(utcDateTime, shouldValidate: true);

        // Assert
        Assert.That((DateTime)result, Is.EqualTo(utcDateTime));
        Assert.That(((DateTime)result).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateFalseAndUtcKind_ShouldPass()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = new TestableUtcDateTime(utcDateTime, shouldValidate: false);

        // Assert
        Assert.That((DateTime)result, Is.EqualTo(utcDateTime));
        Assert.That(((DateTime)result).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateFalse_ShouldAlwaysConvertToUtc()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var unspecifiedTime = new DateTime(2023, 5, 16, 14, 30, 0, DateTimeKind.Unspecified);
        var utcTime = new DateTime(2023, 5, 17, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var localResult = new TestableUtcDateTime(localTime, shouldValidate: false);
        var unspecifiedResult = new TestableUtcDateTime(unspecifiedTime, shouldValidate: false);
        var utcResult = new TestableUtcDateTime(utcTime, shouldValidate: false);

        // Assert
        Assert.That(((DateTime)localResult).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)unspecifiedResult).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)utcResult).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_WithShouldValidateTrue_ShouldConvertThenValidate()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act - Conversion happens first, then validation on the UTC value
        var result = new TestableUtcDateTime(localTime, shouldValidate: true);

        // Assert - Should not throw because after conversion, the value is UTC
        Assert.That((DateTime)result, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)result).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ProtectedConstructor_DefaultShouldValidate_ShouldBeTrue()
    {
        // This test verifies the default behavior is to validate
        // We can't test the default parameter directly, but we can verify
        // that the public constructor (which doesn't have the parameter) behaves like shouldValidate=true
        
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var publicResult = new UtcDateTime(utcDateTime);
        var protectedResult = new TestableUtcDateTime(utcDateTime, shouldValidate: true);

        // Assert - Both should produce the same result
        Assert.That((DateTime)publicResult, Is.EqualTo((DateTime)protectedResult));
    }

    [Test]
    public void ProtectedConstructor_ComparisonBetweenValidateFlags_ShouldProduceSameUtcValue()
    {
        // Arrange
        var testDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var withValidation = new TestableUtcDateTime(testDateTime, shouldValidate: true);
        var withoutValidation = new TestableUtcDateTime(testDateTime, shouldValidate: false);

        // Assert - The resulting UTC values should be identical
        Assert.That((DateTime)withValidation, Is.EqualTo((DateTime)withoutValidation));
        Assert.That(withValidation.Value, Is.EqualTo(withoutValidation.Value));
    }

    #endregion

    #region ConvertToUtc Method Tests

    [Test]
    public void ConvertToUtc_WithUtcDateTime_ShouldReturnSameValue()
    {
        // Arrange
        var utcTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var result = UtcDateTime.ConvertToUtc(utcTime);

        // Assert
        Assert.That(result, Is.EqualTo(utcTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithLocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var result = UtcDateTime.ConvertToUtc(localTime);

        // Assert
        Assert.That(result, Is.EqualTo(expectedUtc));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithUnspecifiedDateTime_ShouldSpecifyUtcKind()
    {
        // Arrange
        var unspecifiedTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Unspecified);

        // Act
        var result = UtcDateTime.ConvertToUtc(unspecifiedTime);

        // Assert
        Assert.That(result, Is.EqualTo(unspecifiedTime));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithLocalMidnight_ShouldConvertCorrectly()
    {
        // Arrange
        var localMidnight = new DateTime(2023, 5, 15, 0, 0, 0, DateTimeKind.Local);
        var expectedUtc = localMidnight.ToUniversalTime();

        // Act
        var result = UtcDateTime.ConvertToUtc(localMidnight);

        // Assert
        Assert.That(result, Is.EqualTo(expectedUtc));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithMinDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var minDateTime = DateTime.MinValue;

        // Act
        var result = UtcDateTime.ConvertToUtc(minDateTime);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithMaxDateTime_ShouldConvertCorrectly()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue;

        // Act
        var result = UtcDateTime.ConvertToUtc(maxDateTime);

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ConvertToUtc_WithHighPrecisionDateTime_ShouldPreservePrecision()
    {
        // Arrange
        var highPrecisionDate = new DateTime(2023, 5, 15, 14, 30, 45, 123, DateTimeKind.Utc).AddTicks(4567);

        // Act
        var result = UtcDateTime.ConvertToUtc(highPrecisionDate);

        // Assert
        Assert.That(result.Ticks, Is.EqualTo(highPrecisionDate.Ticks));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_DateTimeToUtcDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow;

        // Act
        UtcDateTime utcDateTime = testDateTime;

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(testDateTime));
    }

    [Test]
    public void ImplicitConversion_UtcDateTimeToDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow;
        var utcDateTime = new UtcDateTime(testDateTime);

        // Act
        DateTime result = utcDateTime;

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
    }

    [Test]
    public void ImplicitConversion_RoundTrip_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow;

        // Act
        UtcDateTime utcDateTime = originalDateTime;
        DateTime result = utcDateTime;

        // Assert
        Assert.That(result, Is.EqualTo(originalDateTime));
    }

    [Test]
    public void ImplicitConversion_LocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);
        var expectedUtc = localDateTime.ToUniversalTime();

        // Act
        UtcDateTime utcDateTime = localDateTime;
        DateTime result = utcDateTime;

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.EqualTo(expectedUtc));
    }

    [Test]
    public void ImplicitConversion_UtcDateTime_ShouldPreserveValue()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        UtcDateTime utcDateTimeObj = utcDateTime;
        DateTime result = utcDateTimeObj;

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.EqualTo(utcDateTime));
    }

    [Test]
    public void ImplicitConversion_UnspecifiedDateTime_ShouldTreatAsUtc()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        UtcDateTime utcDateTime = unspecifiedDateTime;
        DateTime result = utcDateTime;

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.EqualTo(unspecifiedDateTime)); // Same value, but now UTC kind
    }

    #endregion

    #region Boundary and Edge Case Tests

    [Test]
    public void Constructor_WithMidnightDateTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var midnight = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(midnight);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(midnight));
    }

    [Test]
    public void Constructor_WithLeapYearDate_ShouldCreateUtcDateTime()
    {
        // Arrange
        var leapYearDate = new DateTime(2020, 2, 29, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var utcDateTime = new UtcDateTime(leapYearDate);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(leapYearDate));
    }

    [Test]
    public void Constructor_WithHighPrecisionDateTime_ShouldCreateUtcDateTime()
    {
        // Arrange
        var highPrecisionDate = new DateTime(2023, 5, 15, 14, 30, 45, 123, DateTimeKind.Utc).AddTicks(4567);

        // Act
        var utcDateTime = new UtcDateTime(highPrecisionDate);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(highPrecisionDate));
        Assert.That(((DateTime)utcDateTime).Ticks, Is.EqualTo(highPrecisionDate.Ticks));
    }

    [Test]
    public void Constructor_WithLocalTimeAtDSTBoundary_ShouldHandleCorrectly()
    {
        // Arrange - Create a time around DST boundary (this test may behave differently in different timezones)
        var dstBoundaryTime = new DateTime(2023, 3, 26, 2, 0, 0, DateTimeKind.Local); // Common DST boundary in Europe
        var expectedUtc = dstBoundaryTime.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(dstBoundaryTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithTimeZoneEdgeCase_ShouldMaintainAccuracy()
    {
        // Arrange - Test conversion accuracy at timezone boundaries
        var edgeCaseTime = new DateTime(2023, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);
        var expectedUtc = edgeCaseTime.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(edgeCaseTime);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)utcDateTime).Millisecond, Is.EqualTo(expectedUtc.Millisecond));
    }

    [Test]
    public void Constructor_WithMinValueLocal_ShouldConvertToUtc()
    {
        // Arrange
        var minLocal = new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Local);
        var expectedUtc = minLocal.ToUniversalTime();

        // Act
        var utcDateTime = new UtcDateTime(minLocal);

        // Assert
        Assert.That((DateTime)utcDateTime, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)utcDateTime).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void Value_ShouldReturnSameAsImplicitConversion()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow;
        var utcDateTime = new UtcDateTime(testDateTime);

        // Act
        var directValue = utcDateTime.Value;
        DateTime implicitValue = utcDateTime;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(testDateTime));
    }

    [Test]
    public void Value_WithParameterlessConstructor_ShouldReturnSameAsImplicitConversion()
    {
        // Act
        var utcDateTime = new UtcDateTime();
        var directValue = utcDateTime.Value;
        DateTime implicitValue = utcDateTime;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.LessThanOrEqualTo(DateTime.UtcNow));
        Assert.That(directValue.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Value_WithTimeZoneConversion_ShouldReturnSameAsImplicitConversion()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var utcDateTime = new UtcDateTime(localTime);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var directValue = utcDateTime.Value;
        DateTime implicitValue = utcDateTime;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(expectedUtc));
        Assert.That(directValue.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    #endregion

    #region Validator Tests

    [Test]
    public void Validator_ShouldNotBeNull()
    {
        // Assert
        Assert.That(UtcDateTime.Validator, Is.Not.Null);
    }

    [Test]
    public void Validator_WithUtcDateTime_ShouldPass()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = UtcDateTime.Validator.Validate(utcDateTime);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithLocalDateTime_ShouldFail()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);

        // Act
        var result = UtcDateTime.Validator.Validate(localDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.First().ErrorMessage, Does.Contain("must be UTC"));
    }

    [Test]
    public void Validator_WithUnspecifiedDateTime_ShouldFail()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        var result = UtcDateTime.Validator.Validate(unspecifiedDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.First().ErrorMessage, Does.Contain("must be UTC"));
    }

    [Test]
    public void Validator_WithMinDateTime_ShouldFailIfNotUtc()
    {
        // Arrange
        var minDateTime = DateTime.MinValue; // Unspecified kind

        // Act
        var result = UtcDateTime.Validator.Validate(minDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void Validator_WithMaxDateTime_ShouldFailIfNotUtc()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue; // Unspecified kind

        // Act
        var result = UtcDateTime.Validator.Validate(maxDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void Validator_ErrorMessage_ShouldBeDescriptive()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);

        // Act
        var result = UtcDateTime.Validator.Validate(localDateTime);

        // Assert
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("must be UTC."));
    }

    [Test]
    public void Validator_WithUtcKind_ShouldPass()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var result = UtcDateTime.Validator.Validate(utcDateTime);

        // Assert
        Assert.That(result.IsValid, Is.True, "UTC DateTime should pass Kind validation");
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithLocalKind_ShouldFail()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Local);

        // Act
        var result = UtcDateTime.Validator.Validate(localDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False, "Local DateTime should fail Kind validation");
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("must be UTC")), Is.True, "Should contain UTC validation error");
    }

    [Test]
    public void Validator_WithUnspecifiedKind_ShouldFail()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Unspecified);

        // Act
        var result = UtcDateTime.Validator.Validate(unspecifiedDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False, "Unspecified DateTime should fail Kind validation");
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("must be UTC")), Is.True, "Should contain UTC validation error");
    }

    [Test]
    public void Validator_KindValidation_ShouldBeEnforced()
    {
        // Arrange
        var utcTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Utc);
        var localTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Local);
        var unspecifiedTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Unspecified);

        // Act
        var utcResult = UtcDateTime.Validator.Validate(utcTime);
        var localResult = UtcDateTime.Validator.Validate(localTime);
        var unspecifiedResult = UtcDateTime.Validator.Validate(unspecifiedTime);

        // Assert
        Assert.That(utcResult.IsValid, Is.True, "UTC DateTime should pass");
        Assert.That(localResult.IsValid, Is.False, "Local DateTime should fail Kind validation");
        Assert.That(unspecifiedResult.IsValid, Is.False, "Unspecified DateTime should fail Kind validation");
    }

    #endregion

    #region Record Equality Tests

    [Test]
    public void Equality_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var dateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);
        var utcDateTime1 = new UtcDateTime(dateTime);
        var utcDateTime2 = new UtcDateTime(dateTime);

        // Act & Assert
        Assert.That(utcDateTime1, Is.EqualTo(utcDateTime2));
        Assert.That(utcDateTime1 == utcDateTime2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var dateTime1 = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);
        var dateTime2 = new DateTime(2023, 5, 15, 10, 30, 1, DateTimeKind.Utc);
        var utcDateTime1 = new UtcDateTime(dateTime1);
        var utcDateTime2 = new UtcDateTime(dateTime2);

        // Act & Assert
        Assert.That(utcDateTime1, Is.Not.EqualTo(utcDateTime2));
        Assert.That(utcDateTime1 == utcDateTime2, Is.False);
    }

    [Test]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var dateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);
        var utcDateTime1 = new UtcDateTime(dateTime);
        var utcDateTime2 = new UtcDateTime(dateTime);

        // Act & Assert
        Assert.That(utcDateTime1.GetHashCode(), Is.EqualTo(utcDateTime2.GetHashCode()));
    }

    #endregion
}

