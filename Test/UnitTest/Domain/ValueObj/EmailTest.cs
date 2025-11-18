using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObjs;

namespace UnitTest.Domain.ValueObj;

[TestFixture]
public class EmailTest
{
    #region Valid Input Tests

    [Test]
    public void Constructor_WithValidEmail_ShouldCreateEmail()
    {
        // Arrange
        const string validEmail = "user@example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(validEmail));
    }

    [Test]
    public void Constructor_WithValidEmailUppercase_ShouldConvertToLowercase()
    {
        // Arrange
        const string uppercaseEmail = "USER@EXAMPLE.COM";
        const string expectedLowercase = "user@example.com";

        // Act
        var email = new Email(uppercaseEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(expectedLowercase));
    }

    [Test]
    public void Constructor_WithMixedCaseEmail_ShouldConvertToLowercase()
    {
        // Arrange
        const string mixedCaseEmail = "User.Name@Example.ORG";
        const string expectedLowercase = "user.name@example.org";

        // Act
        var email = new Email(mixedCaseEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(expectedLowercase));
    }

    [Test]
    public void Constructor_WithComplexValidEmail_ShouldCreateEmail()
    {
        // Arrange
        const string complexEmail = "test.email+tag@sub.domain.example.com";

        // Act
        var email = new Email(complexEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(complexEmail));
    }

    [Test]
    public void Constructor_WithEmailContainingNumbers_ShouldCreateEmail()
    {
        // Arrange
        const string emailWithNumbers = "user123@example123.com";

        // Act
        var email = new Email(emailWithNumbers);

        // Assert
        Assert.That((string)email, Is.EqualTo(emailWithNumbers));
    }

    [Test]
    public void Constructor_WithEmailContainingHyphen_ShouldCreateEmail()
    {
        // Arrange
        const string emailWithHyphen = "user-name@sub-domain.example.com";

        // Act
        var email = new Email(emailWithHyphen);

        // Assert
        Assert.That((string)email, Is.EqualTo(emailWithHyphen));
    }

    [Test]
    public void Constructor_WithEmailContainingUnderscore_ShouldCreateEmail()
    {
        // Arrange
        const string emailWithUnderscore = "user_name@example_domain.com";

        // Act
        var email = new Email(emailWithUnderscore);

        // Assert
        Assert.That((string)email, Is.EqualTo(emailWithUnderscore));
    }

    [Test]
    public void Constructor_WithLongValidEmail_ShouldCreateEmail()
    {
        // Arrange - Create a valid email close to max length
        var longLocalPart = new string('a', 64); // Max local part length
        var longDomain = new string('b', 60) + ".example.com"; // Long but valid domain
        var longEmail = $"{longLocalPart}@{longDomain}";

        // Act
        var email = new Email(longEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(longEmail));
        Assert.That(((string)email).Length, Is.LessThanOrEqualTo(Email.MaxLength));
    }

    [Test]
    public void Constructor_WithEmailAtMaxLength_ShouldCreateEmail()
    {
        // Arrange - Create email exactly at max length
        var localPart = new string('a', 64);
        var domainPart = new string('b', Email.MaxLength - localPart.Length - 1 - 4) + ".com"; // -1 for @, -4 for .com
        var maxLengthEmail = $"{localPart}@{domainPart}";

        // Act
        var email = new Email(maxLengthEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(maxLengthEmail));
        Assert.That(((string)email).Length, Is.LessThanOrEqualTo(Email.MaxLength));
    }

    #endregion

    #region Invalid Input Tests

    [Test]
    public void Constructor_WithNull_ShouldThrowNullReferenceException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<NullReferenceException>(() => new Email(null!));
    }

    [Test]
    public void Constructor_WithEmptyString_ShouldThrowValidationException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(string.Empty));
    }

    [Test]
    public void Constructor_WithWhitespaceOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string whitespaceOnly = "   ";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(whitespaceOnly));
    }

    [Test]
    public void Constructor_WithInvalidEmailNoAtSymbol_ShouldThrowValidationException()
    {
        // Arrange
        const string invalidEmail = "userexample.com";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Constructor_WithInvalidEmailNoDomain_ShouldThrowValidationException()
    {
        // Arrange
        const string invalidEmail = "user@";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Constructor_WithInvalidEmailNoLocalPart_ShouldThrowValidationException()
    {
        // Arrange
        const string invalidEmail = "@example.com";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Constructor_WithInvalidEmailMultipleAtSymbols_ShouldThrowValidationException()
    {
        // Arrange
        const string invalidEmail = "user@@example.com";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Constructor_WithInvalidEmailSpecialCharacters_ShouldCreateEmail()
    {
        // Arrange - FluentValidation allows many special characters
        const string emailWithSpecialChars = "user<>@example.com";

        // Act
        var email = new Email(emailWithSpecialChars);

        // Assert
        Assert.That((string)email, Is.EqualTo(emailWithSpecialChars.ToLowerInvariant()));
    }

    [Test]
    public void Constructor_ExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var tooLongEmail = new string('a', Email.MaxLength - 10) + "@example.com"; // This will exceed max length

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(tooLongEmail));
    }

    [Test]
    public void Constructor_SignificantlyExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var veryLongEmail = new string('a', Email.MaxLength + 100) + "@example.com";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new Email(veryLongEmail));
    }

    [Test]
    public void Constructor_WithInvalidDomainFormat_ShouldCreateEmail()
    {
        // Arrange - FluentValidation allows domains without TLD
        const string emailWithoutTld = "user@domain";

        // Act
        var email = new Email(emailWithoutTld);

        // Assert
        Assert.That((string)email, Is.EqualTo(emailWithoutTld.ToLowerInvariant()));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_StringToEmail_ShouldWork()
    {
        // Arrange
        const string testEmail = "test@example.com";

        // Act
        Email email = testEmail;

        // Assert
        Assert.That((string)email, Is.EqualTo(testEmail));
    }

    [Test]
    public void ImplicitConversion_EmailToString_ShouldWork()
    {
        // Arrange
        const string testEmail = "test@example.com";
        var email = new Email(testEmail);

        // Act
        string result = email;

        // Assert
        Assert.That(result, Is.EqualTo(testEmail));
    }

    [Test]
    public void ImplicitConversion_RoundTrip_ShouldPreserveValue()
    {
        // Arrange
        const string originalEmail = "roundtrip@example.com";

        // Act
        Email email = originalEmail;
        string result = email;

        // Assert
        Assert.That(result, Is.EqualTo(originalEmail));
    }

    [Test]
    public void ImplicitConversion_StringToEmail_WithInvalidValue_ShouldThrowValidationException()
    {
        // Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            Email unused = "invalid-email";
        });
    }

    [Test]
    public void ImplicitConversion_CaseNormalization_ShouldWork()
    {
        // Arrange
        const string uppercaseEmail = "TEST@EXAMPLE.COM";
        const string expectedLowercase = "test@example.com";

        // Act
        Email email = uppercaseEmail;
        string result = email;

        // Assert
        Assert.That(result, Is.EqualTo(expectedLowercase));
    }

    #endregion

    #region Boundary and Edge Case Tests

    [Test]
    public void Constructor_WithMaxLengthMinusOne_ShouldCreateEmail()
    {
        // Arrange
        var localPart = new string('a', 64);
        var domainPart = new string('b', Email.MaxLength - localPart.Length - 1 - 4 - 1) + ".com"; // -1 to be under max
        var nearMaxEmail = $"{localPart}@{domainPart}";

        // Act
        var email = new Email(nearMaxEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(nearMaxEmail));
        Assert.That(((string)email).Length, Is.LessThan(Email.MaxLength));
    }

    [Test]
    public void MaxLength_ShouldBe320()
    {
        // Assert
        Assert.That(Email.MaxLength, Is.EqualTo(320));
    }

    [Test]
    public void Constructor_WithInternationalDomain_ShouldCreateEmail()
    {
        // Arrange
        const string internationalEmail = "user@example.org";

        // Act
        var email = new Email(internationalEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(internationalEmail));
    }

    [Test]
    public void Constructor_WithSubdomain_ShouldCreateEmail()
    {
        // Arrange
        const string subdomainEmail = "user@mail.example.com";

        // Act
        var email = new Email(subdomainEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(subdomainEmail));
    }

    [Test]
    public void Constructor_WithPlusAddressing_ShouldCreateEmail()
    {
        // Arrange
        const string plusEmail = "user+tag@example.com";

        // Act
        var email = new Email(plusEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(plusEmail));
    }

    [Test]
    public void Constructor_WithDotInLocalPart_ShouldCreateEmail()
    {
        // Arrange
        const string dotEmail = "first.last@example.com";

        // Act
        var email = new Email(dotEmail);

        // Assert
        Assert.That((string)email, Is.EqualTo(dotEmail));
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void Value_ShouldReturnSameAsImplicitConversion()
    {
        // Arrange
        const string testEmail = "test@example.com";
        var email = new Email(testEmail);

        // Act
        var directValue = email.Value;
        string implicitValue = email;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(testEmail));
    }

    [Test]
    public void Value_WithCaseConversion_ShouldReturnNormalizedValue()
    {
        // Arrange
        const string uppercaseEmail = "TEST@EXAMPLE.COM";
        const string expectedLowercase = "test@example.com";
        var email = new Email(uppercaseEmail);

        // Act
        var directValue = email.Value;
        string implicitValue = email;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(expectedLowercase));
        Assert.That(directValue, Is.Not.EqualTo(uppercaseEmail));
    }

    #endregion

    #region Validator Tests

    [Test]
    public void Validator_ShouldNotBeNull()
    {
        // Assert
        Assert.That(Email.Validator, Is.Not.Null);
    }

    [Test]
    public void Validator_WithValidEmail_ShouldPass()
    {
        // Arrange
        const string validEmail = "test@example.com";

        // Act
        var result = Email.Validator.Validate(validEmail);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        const string invalidEmail = "invalid-email";

        // Act
        var result = Email.Validator.Validate(invalidEmail);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public void Validator_WithEmptyString_ShouldFail()
    {
        // Arrange
        var emptyString = string.Empty;

        // Act
        var result = Email.Validator.Validate(emptyString);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }
    
    [Test]
    public void Validator_WithTooLongEmail_ShouldFail()
    {
        // Arrange
        var tooLongEmail = new string('a', Email.MaxLength + 1) + "@example.com";

        // Act
        var result = Email.Validator.Validate(tooLongEmail);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public void Validator_WithValidEmailAtMaxLength_ShouldPass()
    {
        // Arrange
        var localPart = new string('a', 64);
        var domainPart = new string('b', Email.MaxLength - localPart.Length - 1 - 4) + ".com";
        var maxLengthEmail = $"{localPart}@{domainPart}";

        // Act
        var result = Email.Validator.Validate(maxLengthEmail);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithComplexValidEmail_ShouldPass()
    {
        // Arrange
        const string complexEmail = "user.name+tag@sub.domain.example.com";

        // Act
        var result = Email.Validator.Validate(complexEmail);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithInvalidEmailFormats_ShouldFail()
    {
        // Arrange
        var invalidEmails = new[]
        {
            "plainaddress",
            "@missingdomain.com",
            "user@",
            "@",
            "user@@domain.com",
            "user@domain@com"
        };

        foreach (var invalidEmail in invalidEmails)
        {
            // Act
            var result = Email.Validator.Validate(invalidEmail);

            // Assert
            Assert.That(result.IsValid, Is.False, $"Email '{invalidEmail}' should be invalid");
            Assert.That(result.Errors, Is.Not.Empty, $"Email '{invalidEmail}' should have validation errors");
        }
    }

    #endregion

    #region Case Normalization Tests

    [Test]
    public void Constructor_CaseNormalization_ShouldConvertToLowercase()
    {
        // Arrange
        var testCases = new[]
        {
            ("USER@EXAMPLE.COM", "user@example.com"),
            ("User@Example.Com", "user@example.com"),
            ("user@EXAMPLE.com", "user@example.com"),
            ("USER.NAME@SUB.EXAMPLE.ORG", "user.name@sub.example.org"),
            ("Test.Email+TAG@Domain.Example.NET", "test.email+tag@domain.example.net")
        };

        foreach (var (input, expected) in testCases)
        {
            // Act
            var email = new Email(input);

            // Assert
            Assert.That((string)email, Is.EqualTo(expected), $"Input '{input}' should normalize to '{expected}'");
        }
    }

    [Test]
    public void ImplicitConversion_CaseNormalization_ShouldConvertToLowercase()
    {
        // Arrange
        const string uppercaseEmail = "CONVERT@EXAMPLE.COM";
        const string expectedLowercase = "convert@example.com";

        // Act
        Email email = uppercaseEmail;

        // Assert
        Assert.That((string)email, Is.EqualTo(expectedLowercase));
    }

    [Test]
    public void Value_AfterCaseConversion_ShouldReturnLowercaseValue()
    {
        // Arrange
        const string mixedCaseEmail = "Mixed.Case@Example.COM";
        const string expectedLowercase = "mixed.case@example.com";

        // Act
        var email = new Email(mixedCaseEmail);

        // Assert
        Assert.That(email.Value, Is.EqualTo(expectedLowercase));
    }

    #endregion
}
