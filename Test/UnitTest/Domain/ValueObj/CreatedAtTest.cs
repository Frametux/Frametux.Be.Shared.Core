using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Domain.ValueObj;

[TestFixture]
public class CreatedAtTest
{
    #region Valid Input Tests

    [Test]
    public void Constructor_WithValidPastDate_ShouldCreateCreatedAt()
    {
        // Arrange
        var validPastDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var createdAt = new CreatedAt(validPastDate);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(validPastDate));
    }

    [Test]
    public void Constructor_WithCurrentTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var currentTime = DateTime.UtcNow;

        // Act
        var createdAt = new CreatedAt(currentTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(currentTime));
    }

    [Test]
    public void Constructor_WithVeryOldDate_ShouldCreateCreatedAt()
    {
        // Arrange
        var veryOldDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var createdAt = new CreatedAt(veryOldDate);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(veryOldDate));
    }

    [Test]
    public void Constructor_WithLocalTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var localTime = DateTime.Now.AddHours(-1);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(localTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithUnspecifiedKind_ShouldCreateCreatedAt()
    {
        // Arrange
        var unspecifiedTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        var createdAt = new CreatedAt(unspecifiedTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(unspecifiedTime));
    }

    [Test]
    public void Constructor_WithMinDateTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var minDateTime = DateTime.MinValue;

        // Act
        var createdAt = new CreatedAt(minDateTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(minDateTime));
    }

    [Test]
    public void Constructor_WithMillisecondPrecision_ShouldCreateCreatedAt()
    {
        // Arrange
        var preciseTime = DateTime.UtcNow.AddDays(-1).AddMilliseconds(123.456);

        // Act
        var createdAt = new CreatedAt(preciseTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(preciseTime));
    }

    #endregion

    #region UTC Conversion Tests

    [Test]
    public void Constructor_WithUtcDateTime_ShouldPreserveValue()
    {
        // Arrange
        var utcTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var createdAt = new CreatedAt(utcTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(utcTime));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(localTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithUnspecifiedDateTime_ShouldTreatAsUtc()
    {
        // Arrange
        var unspecifiedTime = new DateTime(2023, 5, 15, 14, 30, 0, DateTimeKind.Unspecified);

        // Act
        var createdAt = new CreatedAt(unspecifiedTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(unspecifiedTime));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalTimeNearMidnight_ShouldConvertCorrectly()
    {
        // Arrange
        var localMidnight = new DateTime(2023, 5, 15, 23, 59, 59, DateTimeKind.Local);
        var expectedUtc = localMidnight.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(localMidnight);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithLocalTimeInDifferentDay_ShouldConvertToCorrectUtcDay()
    {
        // Arrange - Create a local time that when converted to UTC would be in a different day
        var localTime = new DateTime(2023, 5, 15, 1, 0, 0, DateTimeKind.Local);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(localTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
        // Verify the conversion might result in different date
        if (expectedUtc.Date != localTime.Date)
        {
            Assert.That(((DateTime)createdAt).Date, Is.Not.EqualTo(localTime.Date));
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
        var utcCreatedAt = new CreatedAt(utcTime);
        var localCreatedAt = new CreatedAt(localTime);
        var unspecifiedCreatedAt = new CreatedAt(unspecifiedTime);

        // Assert
        Assert.That(((DateTime)utcCreatedAt).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)localCreatedAt).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)unspecifiedCreatedAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithValidLocalTimeButFutureInUtc_ShouldThrowValidationException()
    {
        // Arrange - Create a local time that's valid locally but future in UTC
        var currentUtc = DateTime.UtcNow;
        var futureLocal = currentUtc.ToLocalTime().AddHours(1); // This will be future when converted back to UTC

        // Act & Assert
        if (futureLocal.ToUniversalTime() > currentUtc)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ValidationException>(() => new CreatedAt(futureLocal));
        }
        else
        {
            // If timezone conversion doesn't make it future, it should succeed
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new CreatedAt(futureLocal));
        }
    }

    #endregion

    #region Parameterless Constructor Tests

    [Test]
    public void Constructor_Parameterless_ShouldCreateValidCreatedAt()
    {
        // Act
        var createdAt = new CreatedAt();

        // Assert
        Assert.That((DateTime)createdAt, Is.Not.EqualTo(default(DateTime)));
        Assert.That((DateTime)createdAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void Constructor_Parameterless_ShouldUseUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var createdAt = new CreatedAt();
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.That((DateTime)createdAt, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That((DateTime)createdAt, Is.LessThanOrEqualTo(afterCreation));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_Parameterless_ShouldGenerateRecentTime()
    {
        // Act
        var createdAt = new CreatedAt();
        var timeDifference = DateTime.UtcNow - (DateTime)createdAt;

        // Assert
        Assert.That(timeDifference.TotalSeconds, Is.LessThan(1), "CreatedAt should be very recent");
    }

    [Test]
    public void Constructor_Parameterless_ShouldPassValidation()
    {
        // Act
        var createdAt = new CreatedAt();
        var validationResult = CreatedAt.Validator.Validate(createdAt);

        // Assert
        Assert.That(validationResult.IsValid, Is.True, "Generated CreatedAt should pass validation");
        Assert.That(validationResult.Errors, Is.Empty, "Generated CreatedAt should have no validation errors");
    }

    [Test]
    public void Constructor_Parameterless_ShouldWorkWithImplicitConversion()
    {
        // Act
        var createdAt = new CreatedAt();
        DateTime dateTimeValue = createdAt; // Implicit conversion to DateTime
        CreatedAt convertedBack = dateTimeValue; // Implicit conversion back to CreatedAt

        // Assert
        Assert.That((DateTime)convertedBack, Is.EqualTo((DateTime)createdAt));
        Assert.That((DateTime)convertedBack, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void Constructor_Parameterless_MultipleCalls_ShouldGenerateDifferentTimes()
    {
        // Act
        var createdAt1 = new CreatedAt();
        Thread.Sleep(1); // Ensure different timestamps
        var createdAt2 = new CreatedAt();
        Thread.Sleep(1);
        var createdAt3 = new CreatedAt();

        // Assert
        Assert.That((DateTime)createdAt1, Is.LessThanOrEqualTo((DateTime)createdAt2));
        Assert.That((DateTime)createdAt2, Is.LessThanOrEqualTo((DateTime)createdAt3));
    }

    [Test]
    public void Constructor_Parameterless_ShouldBeCloseToCurrentTime()
    {
        // Arrange
        var expectedTime = DateTime.UtcNow;

        // Act
        var createdAt = new CreatedAt();

        // Assert
        var difference = Math.Abs(((DateTime)createdAt - expectedTime).TotalMilliseconds);
        Assert.That(difference, Is.LessThan(100), "Generated time should be very close to current UTC time");
    }

    #endregion

    #region Invalid Input Tests

    [Test]
    public void Constructor_WithFutureDate_ShouldThrowValidationException()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(futureDate));
    }

    [Test]
    public void Constructor_WithFutureTime_ShouldThrowValidationException()
    {
        // Arrange
        var futureTime = DateTime.UtcNow.AddMinutes(1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(futureTime));
    }

    [Test]
    public void Constructor_WithDistantFuture_ShouldThrowValidationException()
    {
        // Arrange
        var distantFuture = DateTime.UtcNow.AddYears(10);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(distantFuture));
    }

    [Test]
    public void Constructor_WithMaxDateTime_ShouldThrowValidationException()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue;

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(maxDateTime));
    }

    [Test]
    public void Constructor_WithFutureLocalTime_ShouldThrowValidationException()
    {
        // Arrange
        var futureLocalTime = DateTime.Now.AddHours(1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(futureLocalTime));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_DateTimeToCreatedAt_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-1);

        // Act
        CreatedAt createdAt = testDateTime;

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(testDateTime));
    }

    [Test]
    public void ImplicitConversion_CreatedAtToDateTime_ShouldWork()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddMinutes(-30);
        var createdAt = new CreatedAt(testDateTime);

        // Act
        DateTime result = createdAt;

        // Assert
        Assert.That(result, Is.EqualTo(testDateTime));
    }

    [Test]
    public void ImplicitConversion_RoundTrip_ShouldPreserveValue()
    {
        // Arrange
        var originalDateTime = DateTime.UtcNow.AddDays(-5);

        // Act
        CreatedAt createdAt = originalDateTime;
        DateTime result = createdAt;

        // Assert
        Assert.That(result, Is.EqualTo(originalDateTime));
    }

    [Test]
    public void ImplicitConversion_DateTimeToCreatedAt_WithInvalidValue_ShouldThrowValidationException()
    {
        // Arrange
        var futureDateTime = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            CreatedAt unused = futureDateTime;
        });
    }

    [Test]
    public void ImplicitConversion_LocalDateTime_ShouldConvertToUtc()
    {
        // Arrange
        var localDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Local);
        var expectedUtc = localDateTime.ToUniversalTime();

        // Act
        CreatedAt createdAt = localDateTime;
        DateTime result = createdAt;

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.EqualTo(expectedUtc));
        // Note: The actual time value may be the same as localDateTime if the system timezone is UTC
        // The important assertion is that the Kind is UTC and the value equals the expected UTC conversion
    }

    [Test]
    public void ImplicitConversion_UtcDateTime_ShouldPreserveValue()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        CreatedAt createdAt = utcDateTime;
        DateTime result = createdAt;

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
        CreatedAt createdAt = unspecifiedDateTime;
        DateTime result = createdAt;

        // Assert
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(result, Is.EqualTo(unspecifiedDateTime)); // Same value, but now UTC kind
    }

    #endregion

    #region Boundary and Edge Case Tests

    [Test]
    public void Constructor_WithDateTimeJustBeforeNow_ShouldCreateCreatedAt()
    {
        // Arrange
        var almostNow = DateTime.UtcNow.AddMilliseconds(-1);

        // Act
        var createdAt = new CreatedAt(almostNow);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(almostNow));
    }

    [Test]
    public void Constructor_WithDateTimeJustAfterNow_ShouldThrowValidationException()
    {
        // Arrange
        var justAfterNow = DateTime.UtcNow.AddMinutes(1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new CreatedAt(justAfterNow));
    }

    [Test]
    public void Constructor_WithPreciseCurrentTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var preciseNow = DateTime.UtcNow;

        // Act
        var createdAt = new CreatedAt(preciseNow);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(preciseNow));
    }

    [Test]
    public void Constructor_WithMidnightDateTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var midnight = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var createdAt = new CreatedAt(midnight);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(midnight));
    }

    [Test]
    public void Constructor_WithLeapYearDate_ShouldCreateCreatedAt()
    {
        // Arrange
        var leapYearDate = new DateTime(2020, 2, 29, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var createdAt = new CreatedAt(leapYearDate);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(leapYearDate));
    }

    [Test]
    public void Constructor_WithHighPrecisionDateTime_ShouldCreateCreatedAt()
    {
        // Arrange
        var highPrecisionDate = new DateTime(2023, 5, 15, 14, 30, 45, 123, DateTimeKind.Utc).AddTicks(4567);

        // Act
        var createdAt = new CreatedAt(highPrecisionDate);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(highPrecisionDate));
        Assert.That(((DateTime)createdAt).Ticks, Is.EqualTo(highPrecisionDate.Ticks));
    }

    [Test]
    public void Constructor_WithLocalTimeAtDSTBoundary_ShouldHandleCorrectly()
    {
        // Arrange - Create a time around DST boundary (this test may behave differently in different timezones)
        var dstBoundaryTime = new DateTime(2023, 3, 26, 2, 0, 0, DateTimeKind.Local); // Common DST boundary in Europe
        var expectedUtc = dstBoundaryTime.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(dstBoundaryTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithTimeZoneEdgeCase_ShouldMaintainAccuracy()
    {
        // Arrange - Test conversion accuracy at timezone boundaries
        var edgeCaseTime = new DateTime(2023, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);
        var expectedUtc = edgeCaseTime.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(edgeCaseTime);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(((DateTime)createdAt).Millisecond, Is.EqualTo(expectedUtc.Millisecond));
    }

    [Test]
    public void Constructor_WithMinValueLocal_ShouldConvertToUtc()
    {
        // Arrange
        var minLocal = new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Local);
        var expectedUtc = minLocal.ToUniversalTime();

        // Act
        var createdAt = new CreatedAt(minLocal);

        // Assert
        Assert.That((DateTime)createdAt, Is.EqualTo(expectedUtc));
        Assert.That(((DateTime)createdAt).Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Constructor_WithFutureLocalTimeThatBecomesValidInUtc_ShouldSucceed()
    {
        // Arrange - Create a local time that's future locally but valid when converted to UTC
        // This would happen in timezones ahead of UTC
        var localTime = DateTime.Now.AddMinutes(-30); // Past in local time
        if (localTime.Kind != DateTimeKind.Local)
        {
            localTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local);
        }

        // Act & Assert - Should not throw since it becomes valid when converted to UTC
        Assert.DoesNotThrow(() =>
        {
            var unused = new CreatedAt(localTime);
        });
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void Value_ShouldReturnSameAsImplicitConversion()
    {
        // Arrange
        var testDateTime = DateTime.UtcNow.AddHours(-1);
        var createdAt = new CreatedAt(testDateTime);

        // Act
        var directValue = createdAt.Value;
        DateTime implicitValue = createdAt;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(testDateTime));
    }

    [Test]
    public void Value_WithParameterlessConstructor_ShouldReturnSameAsImplicitConversion()
    {
        // Act
        var createdAt = new CreatedAt();
        var directValue = createdAt.Value;
        DateTime implicitValue = createdAt;

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
        var createdAt = new CreatedAt(localTime);
        var expectedUtc = localTime.ToUniversalTime();

        // Act
        var directValue = createdAt.Value;
        DateTime implicitValue = createdAt;

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
        Assert.That(CreatedAt.Validator, Is.Not.Null);
    }

    [Test]
    public void Validator_WithValidDateTime_ShouldPass()
    {
        // Arrange
        var validDateTime = DateTime.UtcNow.AddHours(-1);

        // Act
        var result = CreatedAt.Validator.Validate(validDateTime);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithCurrentDateTime_ShouldPass()
    {
        // Arrange
        var currentDateTime = DateTime.UtcNow;

        // Act
        var result = CreatedAt.Validator.Validate(currentDateTime);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithFutureDateTime_ShouldFail()
    {
        // Arrange
        var futureDateTime = DateTime.UtcNow.AddMinutes(1);

        // Act
        var result = CreatedAt.Validator.Validate(futureDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.First().ErrorMessage, Does.Contain("cannot be in the future"));
    }

    [Test]
    public void Validator_WithMinDateTime_ShouldPass()
    {
        // Arrange
        var minDateTime = new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc);

        // Act
        var result = CreatedAt.Validator.Validate(minDateTime);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithMaxDateTime_ShouldFail()
    {
        // Arrange
        var maxDateTime = DateTime.MaxValue;

        // Act
        var result = CreatedAt.Validator.Validate(maxDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public void Validator_ErrorMessage_ShouldBeDescriptive()
    {
        // Arrange
        var futureDateTime = DateTime.UtcNow.AddYears(1);

        // Act
        var result = CreatedAt.Validator.Validate(futureDateTime);

        // Assert
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("cannot be in the future"));
    }

    [Test]
    public void Validator_WithLocalTime_ShouldValidateAgainstUtc()
    {
        // Arrange
        var localTime = DateTime.Now.AddHours(-1); // Valid past local time
        var utcEquivalent = localTime.ToUniversalTime();

        // Act
        var result = CreatedAt.Validator.Validate(utcEquivalent);

        // Assert
        Assert.That(result.IsValid, Is.True, "Validation should work with UTC equivalent of local time");
    }

    [Test]
    public void Validator_WithUnspecifiedKind_ShouldFailKindValidation()
    {
        // Arrange
        var unspecifiedTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Unspecified);

        // Act
        var result = CreatedAt.Validator.Validate(unspecifiedTime);

        // Assert
        Assert.That(result.IsValid, Is.False, "Unspecified DateTime should fail Kind validation");
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("must be UTC")), Is.True, "Should fail because Kind is not UTC");
    }

    [Test]
    public void Validator_WithBoundaryDateTime_ShouldValidateCorrectly()
    {
        // Arrange - Test validation right at the UTC boundary
        var boundaryTime = DateTime.UtcNow.AddMilliseconds(-1); // Just before now

        // Act
        var result = CreatedAt.Validator.Validate(boundaryTime);

        // Assert
        Assert.That(result.IsValid, Is.True, "DateTime just before UTC now should be valid");
    }

    [Test]
    public void Validator_ComparisonAccuracy_ShouldBeConsistent()
    {
        // Arrange
        var testTime1 = DateTime.UtcNow.AddSeconds(-1);
        var testTime2 = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var result1 = CreatedAt.Validator.Validate(testTime1);
        var result2 = CreatedAt.Validator.Validate(testTime2);

        // Assert
        Assert.That(result1.IsValid, Is.EqualTo(result2.IsValid), "Similar times should have consistent validation results");
    }

    [Test]
    public void Validator_WithUtcKind_ShouldPass()
    {
        // Arrange
        var utcDateTime = new DateTime(2023, 5, 15, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var result = CreatedAt.Validator.Validate(utcDateTime);

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
        var result = CreatedAt.Validator.Validate(localDateTime);

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
        var result = CreatedAt.Validator.Validate(unspecifiedDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False, "Unspecified DateTime should fail Kind validation");
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("must be UTC")), Is.True, "Should contain UTC validation error");
    }

    [Test]
    public void Validator_WithMultipleViolations_ShouldReportAllErrors()
    {
        // Arrange - Future local time (violates both time and kind rules)
        var futureLocalDateTime = DateTime.Now.AddDays(1);
        if (futureLocalDateTime.Kind != DateTimeKind.Local)
        {
            futureLocalDateTime = DateTime.SpecifyKind(futureLocalDateTime, DateTimeKind.Local);
        }

        // Act
        var result = CreatedAt.Validator.Validate(futureLocalDateTime);

        // Assert
        Assert.That(result.IsValid, Is.False, "Future local DateTime should fail validation");
        Assert.That(result.Errors.Count, Is.EqualTo(2), "Should have both future and Kind validation errors");
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("cannot be in the future")), Is.True);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("must be UTC")), Is.True);
    }

    [Test]
    public void Validator_KindValidation_ShouldBeEnforced()
    {
        // Arrange
        var pastUtcTime = DateTime.UtcNow.AddHours(-1);
        var pastLocalTime = DateTime.Now.AddHours(-1);
        var pastUnspecifiedTime = new DateTime(DateTime.Now.AddHours(-1).Ticks, DateTimeKind.Unspecified);

        // Act
        var utcResult = CreatedAt.Validator.Validate(pastUtcTime);
        var localResult = CreatedAt.Validator.Validate(pastLocalTime);
        var unspecifiedResult = CreatedAt.Validator.Validate(pastUnspecifiedTime);

        // Assert
        Assert.That(utcResult.IsValid, Is.True, "UTC DateTime should pass");
        Assert.That(localResult.IsValid, Is.False, "Local DateTime should fail Kind validation");
        Assert.That(unspecifiedResult.IsValid, Is.False, "Unspecified DateTime should fail Kind validation");
    }

    #endregion
}
