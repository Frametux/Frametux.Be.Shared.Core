using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObj;

namespace UnitTest.Domain.ValueObj;

[TestFixture]
public class IdTest
{
    #region Valid Input Tests

    [Test]
    public void Constructor_WithValidString_ShouldCreateId()
    {
        // Arrange
        const string validValue = "valid-id-123";

        // Act
        var id = new Id(validValue);

        // Assert
        Assert.That((string)id, Is.EqualTo(validValue));
    }

    [Test]
    public void Constructor_WithSingleCharacter_ShouldCreateId()
    {
        // Arrange
        const string singleChar = "A";

        // Act
        var id = new Id(singleChar);

        // Assert
        Assert.That((string)id, Is.EqualTo(singleChar));
    }

    [Test]
    public void Constructor_WithMaxLength_ShouldCreateId()
    {
        // Arrange
        var maxLengthString = new string('A', Id.MaxLength);

        // Act
        var id = new Id(maxLengthString);

        // Assert
        Assert.That((string)id, Is.EqualTo(maxLengthString));
    }

    [Test]
    public void Constructor_WithMixedContent_ShouldCreateId()
    {
        // Arrange
        const string mixedContent = "abc123_ÄÖÜäöü-!@#$%";

        // Act
        var id = new Id(mixedContent);

        // Assert
        Assert.That((string)id, Is.EqualTo(mixedContent));
    }

    [Test]
    public void Constructor_WithUnicodeCharacters_ShouldCreateId()
    {
        // Arrange
        const string unicodeString = "こんにちは🌟世界";

        // Act
        var id = new Id(unicodeString);

        // Assert
        Assert.That((string)id, Is.EqualTo(unicodeString));
    }

    [Test]
    public void Constructor_WithWhitespaceInContent_ShouldCreateId()
    {
        // Arrange
        const string stringWithSpaces = "valid id with spaces";

        // Act
        var id = new Id(stringWithSpaces);

        // Assert
        Assert.That((string)id, Is.EqualTo(stringWithSpaces));
    }

    #endregion

    #region Invalid Input Tests

    [Test]
    public void Constructor_WithNull_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<InvalidOperationException>(() => new Id(null!));
    }

    [Test]
    public void Constructor_WithEmptyString_ShouldThrowValidationException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(string.Empty));
    }

    [Test]
    public void Constructor_WithWhitespaceOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string whitespaceOnly = "   ";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(whitespaceOnly));
    }

    [Test]
    public void Constructor_WithTabsOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string tabsOnly = "\t\t\t";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(tabsOnly));
    }

    [Test]
    public void Constructor_WithNewlinesOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string newlinesOnly = "\n\r\n";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(newlinesOnly));
    }

    [Test]
    public void Constructor_ExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var tooLongString = new string('A', Id.MaxLength + 1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(tooLongString));
    }

    [Test]
    public void Constructor_SignificantlyExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var veryLongString = new string('A', Id.MaxLength + 100);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(veryLongString));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_StringToId_ShouldWork()
    {
        // Arrange
        const string testValue = "test-string-to-id";

        // Act
        Id id = testValue;

        // Assert
        Assert.That((string)id, Is.EqualTo(testValue));
    }

    [Test]
    public void ImplicitConversion_IdToString_ShouldWork()
    {
        // Arrange
        const string testValue = "test-id-to-string";
        var id = new Id(testValue);

        // Act
        string result = id;

        // Assert
        Assert.That(result, Is.EqualTo(testValue));
    }

    [Test]
    public void ImplicitConversion_RoundTrip_ShouldPreserveValue()
    {
        // Arrange
        const string originalValue = "round-trip-test-value";

        // Act
        Id id = originalValue;
        string result = id;

        // Assert
        Assert.That(result, Is.EqualTo(originalValue));
    }

    [Test]
    public void ImplicitConversion_StringToId_WithInvalidValue_ShouldThrowValidationException()
    {
        // Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            Id id = string.Empty;
        });
    }

    #endregion

    #region Boundary and Edge Case Tests

    [Test]
    public void Constructor_WithMaxLengthMinusOne_ShouldCreateId()
    {
        // Arrange
        var nearMaxString = new string('B', Id.MaxLength - 1);

        // Act
        var id = new Id(nearMaxString);

        // Assert
        Assert.That((string)id, Is.EqualTo(nearMaxString));
        Assert.That(((string)id).Length, Is.EqualTo(Id.MaxLength - 1));
    }

    [Test]
    public void Constructor_WithMaxLengthPlusOne_ShouldThrowValidationException()
    {
        // Arrange
        var overMaxString = new string('C', Id.MaxLength + 1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Id(overMaxString));
    }

    [Test]
    public void MaxLength_ShouldBe255()
    {
        // Assert
        Assert.That(Id.MaxLength, Is.EqualTo(255));
    }

    [Test]
    public void Constructor_WithComplexUnicodeAtMaxLength_ShouldCreateId()
    {
        // Arrange - Create string with complex Unicode characters at max length
        var unicodeChar = "🌟"; // This is actually 2 UTF-16 code units
        var baseString = new string('A', Id.MaxLength - unicodeChar.Length);
        var complexString = baseString + unicodeChar;

        // Act
        var id = new Id(complexString);

        // Assert
        Assert.That((string)id, Is.EqualTo(complexString));
        Assert.That(((string)id).Length, Is.LessThanOrEqualTo(Id.MaxLength));
    }

    [Test]
    public void Constructor_WithLeadingAndTrailingSpaces_ShouldCreateId()
    {
        // Arrange
        const string stringWithSpaces = " valid content ";

        // Act
        var id = new Id(stringWithSpaces);

        // Assert
        Assert.That((string)id, Is.EqualTo(stringWithSpaces));
    }

    #endregion

    #region Validator Tests

    [Test]
    public void Validator_ShouldNotBeNull()
    {
        // Assert
        Assert.That(Id.Validator, Is.Not.Null);
    }

    [Test]
    public void Validator_WithValidString_ShouldPass()
    {
        // Arrange
        const string validString = "valid-test-string";

        // Act
        var result = Id.Validator.Validate(validString);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithInvalidString_ShouldFail()
    {
        // Arrange
        var invalidString = string.Empty;

        // Act
        var result = Id.Validator.Validate(invalidString);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    #endregion
}