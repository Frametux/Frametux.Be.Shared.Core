using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Driven.Persistence.EntityConfigs.Conversions;

[TestFixture]
public class EmailConversionTest
{
    #region Non-Nullable Conversion Tests

    [Test]
    public void HasEmailConversion_ConversionToString_ShouldWork()
    {
        // Arrange
        var testValue = "test@example.com";
        var email = new Email(testValue);

        // Act - Simulate the conversion delegate: i => i.Value
        Func<Email, string> convertToDb = i => i.Value;
        var result = convertToDb(email);

        // Assert
        Assert.That(result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_ConversionFromString_ShouldWork()
    {
        // Arrange
        var testValue = "user@domain.com";

        // Act - Simulate the conversion delegate: i => new Email(i)
        Func<string, Email> convertFromDb = i => new Email(i);
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result.Value, Is.EqualTo(testValue));
        Assert.That((string)result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_RoundTripConversion_ShouldPreserveValue()
    {
        // Arrange
        var originalValue = "roundtrip@test.com";
        var email = new Email(originalValue);

        // Act - Simulate both conversions
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredEmail.Value, Is.EqualTo(originalValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(email.Value));
    }

    [Test]
    public void HasEmailConversion_WithUpperCaseEmail_ShouldConvertToLowerCase()
    {
        // Arrange
        var upperCaseEmail = "UPPER@EXAMPLE.COM";
        var expectedLowerCase = "upper@example.com";

        // Act - Email constructor normalizes to lowercase
        Func<string, Email> convertFromDb = i => new Email(i);
        var result = convertFromDb(upperCaseEmail);

        // Assert
        Assert.That(result.Value, Is.EqualTo(expectedLowerCase));
    }

    #endregion

    #region Nullable Conversion Tests

    [Test]
    public void HasEmailNullableConversion_ConversionToString_WithValue_ShouldWork()
    {
        // Arrange
        var testValue = "nullable@example.com";
        var email = new Email(testValue);

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<Email?, string?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(email);

        // Assert
        Assert.That(result, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailNullableConversion_ConversionToString_WithNull_ShouldReturnNull()
    {
        // Arrange
        Email? email = null;

        // Act - Simulate the conversion delegate: i => i != null ? i.Value : null
        Func<Email?, string?> convertToDb = i => i != null ? i.Value : null;
        var result = convertToDb(email);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasEmailNullableConversion_ConversionFromString_WithValue_ShouldWork()
    {
        // Arrange
        var testValue = "fromdb@example.com";

        // Act - Simulate the conversion delegate: i => i != null ? new Email(i) : null
        Func<string?, Email?> convertFromDb = i => i != null ? new Email(i) : null;
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailNullableConversion_ConversionFromString_WithNull_ShouldReturnNull()
    {
        // Arrange
        string? testValue = null;

        // Act - Simulate the conversion delegate: i => i != null ? new Email(i) : null
        Func<string?, Email?> convertFromDb = i => i != null ? new Email(i) : null;
        var result = convertFromDb(testValue);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void HasEmailNullableConversion_RoundTripConversion_WithValue_ShouldPreserveValue()
    {
        // Arrange
        var originalValue = "roundtrip@nullable.com";
        var email = new Email(originalValue);

        // Act - Simulate both conversions
        Func<Email?, string?> convertToDb = i => i != null ? i.Value : null;
        Func<string?, Email?> convertFromDb = i => i != null ? new Email(i) : null;

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(restoredEmail, Is.Not.Null);
        Assert.That(restoredEmail!.Value, Is.EqualTo(originalValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(email.Value));
    }

    [Test]
    public void HasEmailNullableConversion_RoundTripConversion_WithNull_ShouldPreserveNull()
    {
        // Arrange
        Email? email = null;

        // Act - Simulate both conversions
        Func<Email?, string?> convertToDb = i => i != null ? i.Value : null;
        Func<string?, Email?> convertFromDb = i => i != null ? new Email(i) : null;

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.Null);
        Assert.That(restoredEmail, Is.Null);
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void HasEmailConversion_WithSubdomains_ShouldConvertCorrectly()
    {
        // Arrange
        var testValue = "user@mail.subdomain.example.com";
        var email = new Email(testValue);

        // Act
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_WithPlusAddressing_ShouldConvertCorrectly()
    {
        // Arrange
        var testValue = "user+tag@example.com";
        var email = new Email(testValue);

        // Act
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_WithDotsInLocalPart_ShouldConvertCorrectly()
    {
        // Arrange
        var testValue = "first.last@example.com";
        var email = new Email(testValue);

        // Act
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_WithNumericDomain_ShouldConvertCorrectly()
    {
        // Arrange
        var testValue = "user@123.456.com";
        var email = new Email(testValue);

        // Act
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasEmailConversion_WithMixedCaseEmail_ShouldNormalizeDuringConversion()
    {
        // Arrange
        var mixedCaseValue = "User@EXAMPLE.Com";
        var expectedNormalized = "user@example.com";

        // Act - Conversion from DB should normalize
        Func<string, Email> convertFromDb = i => new Email(i);
        var email = convertFromDb(mixedCaseValue);
        
        Func<Email, string> convertToDb = i => i.Value;
        var dbValue = convertToDb(email);

        // Assert
        Assert.That(dbValue, Is.EqualTo(expectedNormalized));
    }

    [Test]
    public void HasEmailConversion_WithMaxLengthEmail_ShouldConvertCorrectly()
    {
        // Arrange - Create a long but valid email
        var localPart = new string('a', 64); // Max local part is 64 chars
        var domain = "example.com";
        var testValue = $"{localPart}@{domain}";
        var email = new Email(testValue);

        // Act
        Func<Email, string> convertToDb = i => i.Value;
        Func<string, Email> convertFromDb = i => new Email(i);

        var dbValue = convertToDb(email);
        var restoredEmail = convertFromDb(dbValue);

        // Assert
        Assert.That(dbValue, Is.EqualTo(testValue));
        Assert.That(restoredEmail.Value, Is.EqualTo(testValue));
    }

    #endregion
}

