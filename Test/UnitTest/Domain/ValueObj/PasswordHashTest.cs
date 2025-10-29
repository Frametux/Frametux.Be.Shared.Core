using FluentValidation;
using Frametux.Shared.Core.Domain.ValueObj;

namespace UnitTest.Domain.ValueObj;

[TestFixture]
public class PasswordHashTest
{
    #region Valid Input Tests

    [Test]
    public void Constructor_WithValidPasswordHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string validHash = "$2b$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewisAPI.KtAkmW.S";

        // Act
        var passwordHash = new PasswordHash(validHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(validHash));
    }

    [Test]
    public void Constructor_WithSingleCharacter_ShouldCreatePasswordHash()
    {
        // Arrange
        const string singleChar = "a";

        // Act
        var passwordHash = new PasswordHash(singleChar);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(singleChar));
    }

    [Test]
    public void Constructor_WithBcryptHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string bcryptHash = "$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi";

        // Act
        var passwordHash = new PasswordHash(bcryptHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(bcryptHash));
    }

    [Test]
    public void Constructor_WithArgon2Hash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string argon2Hash = "$argon2id$v=19$m=4096,t=3,p=1$MTIzNDU2Nzg$GpZ3sK/oH";

        // Act
        var passwordHash = new PasswordHash(argon2Hash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(argon2Hash));
    }

    [Test]
    public void Constructor_WithScryptHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string scryptHash = "$s0$e0801$epIxT/h6HbbwHaehFnh/bw$7H0vsXlY8UxxyW/BWx/9GuY7jEvGjT71GFd6O4SZND0";

        // Act
        var passwordHash = new PasswordHash(scryptHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(scryptHash));
    }

    [Test]
    public void Constructor_WithShaHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string shaHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

        // Act
        var passwordHash = new PasswordHash(shaHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(shaHash));
    }

    [Test]
    public void Constructor_WithMixedCaseHash_ShouldPreserveCase()
    {
        // Arrange
        const string mixedCaseHash = "$2B$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewisAPI.KtAkmW.S";

        // Act
        var passwordHash = new PasswordHash(mixedCaseHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(mixedCaseHash));
    }

    [Test]
    public void Constructor_WithNumericHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string numericHash = "1234567890abcdef1234567890abcdef12345678";

        // Act
        var passwordHash = new PasswordHash(numericHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(numericHash));
    }

    [Test]
    public void Constructor_WithSpecialCharacters_ShouldCreatePasswordHash()
    {
        // Arrange
        const string hashWithSpecialChars = "$2a$10$N9qo8uLOickgx2ZMRZoMye.IRI/.og/at2.uheWG/igi";

        // Act
        var passwordHash = new PasswordHash(hashWithSpecialChars);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(hashWithSpecialChars));
    }

    [Test]
    public void Constructor_WithMaxLength_ShouldCreatePasswordHash()
    {
        // Arrange
        var maxLengthHash = new string('A', PasswordHash.MaxLength);

        // Act
        var passwordHash = new PasswordHash(maxLengthHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(maxLengthHash));
        Assert.That(((string)passwordHash).Length, Is.EqualTo(PasswordHash.MaxLength));
    }

    [Test]
    public void Constructor_WithLongValidHash_ShouldCreatePasswordHash()
    {
        // Arrange - Create a valid hash close to max length
        var baseHash = "$2b$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewisAPI.KtAkmW.S";
        var longHash = baseHash + new string('a', PasswordHash.MaxLength - baseHash.Length);

        // Act
        var passwordHash = new PasswordHash(longHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(longHash));
        Assert.That(((string)passwordHash).Length, Is.LessThanOrEqualTo(PasswordHash.MaxLength));
    }

    [Test]
    public void Constructor_WithUnicodeCharacters_ShouldCreatePasswordHash()
    {
        // Arrange - Though unlikely in real hashes, should be supported
        const string unicodeHash = "hash_with_émojis_🔒_and_üñíçødé";

        // Act
        var passwordHash = new PasswordHash(unicodeHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(unicodeHash));
    }

    [Test]
    public void Constructor_WithWhitespaceInContent_ShouldPreserveWhitespace()
    {
        // Arrange - Though unusual, should preserve as-is per requirements
        const string hashWithSpaces = "hash with spaces in content";

        // Act
        var passwordHash = new PasswordHash(hashWithSpaces);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(hashWithSpaces));
    }

    [Test]
    public void Constructor_WithLeadingAndTrailingSpaces_ShouldPreserveSpaces()
    {
        // Arrange - No normalization per requirements
        const string hashWithSpaces = " $2b$12$hash_content ";

        // Act
        var passwordHash = new PasswordHash(hashWithSpaces);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(hashWithSpaces));
    }

    #endregion

    #region Invalid Input Tests

    [Test]
    public void Constructor_WithNull_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<InvalidOperationException>(() => new PasswordHash(null!));
    }

    [Test]
    public void Constructor_WithEmptyString_ShouldThrowValidationException()
    {
        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(string.Empty));
    }

    [Test]
    public void Constructor_WithWhitespaceOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string whitespaceOnly = "   ";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(whitespaceOnly));
    }

    [Test]
    public void Constructor_WithTabsOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string tabsOnly = "\t\t\t";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(tabsOnly));
    }

    [Test]
    public void Constructor_WithNewlinesOnly_ShouldThrowValidationException()
    {
        // Arrange
        const string newlinesOnly = "\n\r\n";

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(newlinesOnly));
    }

    [Test]
    public void Constructor_ExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var tooLongHash = new string('A', PasswordHash.MaxLength + 1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(tooLongHash));
    }

    [Test]
    public void Constructor_SignificantlyExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var veryLongHash = new string('A', PasswordHash.MaxLength + 100);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(veryLongHash));
    }

    [Test]
    public void Constructor_ExceedingMaxLengthByOne_ShouldThrowValidationException()
    {
        // Arrange
        var overLimitHash = new string('B', PasswordHash.MaxLength + 1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(overLimitHash));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_StringToPasswordHash_ShouldWork()
    {
        // Arrange
        const string testHash = "$2b$12$test.hash.value";

        // Act
        PasswordHash passwordHash = testHash;

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(testHash));
    }

    [Test]
    public void ImplicitConversion_PasswordHashToString_ShouldWork()
    {
        // Arrange
        const string testHash = "$2y$10$example.hash.string";
        var passwordHash = new PasswordHash(testHash);

        // Act
        string result = passwordHash;

        // Assert
        Assert.That(result, Is.EqualTo(testHash));
    }

    [Test]
    public void ImplicitConversion_RoundTrip_ShouldPreserveValue()
    {
        // Arrange
        const string originalHash = "$argon2id$v=19$m=65536,t=2,p=1$example";

        // Act
        PasswordHash passwordHash = originalHash;
        string result = passwordHash;

        // Assert
        Assert.That(result, Is.EqualTo(originalHash));
    }

    [Test]
    public void ImplicitConversion_StringToPasswordHash_WithInvalidValue_ShouldThrowValidationException()
    {
        // Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            PasswordHash unused = string.Empty;
        });
    }

    [Test]
    public void ImplicitConversion_CasePreservation_ShouldWork()
    {
        // Arrange
        const string mixedCaseHash = "$2B$12$MiXeD.CaSe.HaSh.VaLuE";

        // Act
        PasswordHash passwordHash = mixedCaseHash;
        string result = passwordHash;

        // Assert
        Assert.That(result, Is.EqualTo(mixedCaseHash));
    }

    [Test]
    public void ImplicitConversion_WithSpecialCharacters_ShouldPreserveCharacters()
    {
        // Arrange
        const string specialCharHash = "$2a$10$special./chars+in=hash*value";

        // Act
        PasswordHash passwordHash = specialCharHash;
        string result = passwordHash;

        // Assert
        Assert.That(result, Is.EqualTo(specialCharHash));
    }

    #endregion

    #region Boundary and Edge Case Tests

    [Test]
    public void Constructor_WithMaxLengthMinusOne_ShouldCreatePasswordHash()
    {
        // Arrange
        var nearMaxHash = new string('C', PasswordHash.MaxLength - 1);

        // Act
        var passwordHash = new PasswordHash(nearMaxHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(nearMaxHash));
        Assert.That(((string)passwordHash).Length, Is.EqualTo(PasswordHash.MaxLength - 1));
    }

    [Test]
    public void Constructor_WithMaxLengthPlusOne_ShouldThrowValidationException()
    {
        // Arrange
        var overMaxHash = new string('D', PasswordHash.MaxLength + 1);

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ValidationException>(() => new PasswordHash(overMaxHash));
    }

    [Test]
    public void MaxLength_ShouldBe512()
    {
        // Assert
        Assert.That(PasswordHash.MaxLength, Is.EqualTo(512));
    }

    [Test]
    public void Constructor_WithComplexUnicodeAtMaxLength_ShouldCreatePasswordHash()
    {
        // Arrange - Create hash with complex Unicode characters at max length
        var unicodeChar = "🔒"; // This is actually multiple UTF-16 code units
        var baseString = new string('A', PasswordHash.MaxLength - unicodeChar.Length);
        var complexHash = baseString + unicodeChar;

        // Act
        var passwordHash = new PasswordHash(complexHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(complexHash));
        Assert.That(((string)passwordHash).Length, Is.LessThanOrEqualTo(PasswordHash.MaxLength));
    }

    [Test]
    public void Constructor_WithRealWorldBcryptHash_ShouldCreatePasswordHash()
    {
        // Arrange - Real bcrypt hash format
        const string realBcryptHash = "$2b$10$N9qo8uLOickgx2ZMRZoMyeIRI.Og/at2.uheWG/igi";

        // Act
        var passwordHash = new PasswordHash(realBcryptHash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(realBcryptHash));
    }

    [Test]
    public void Constructor_WithRealWorldArgon2Hash_ShouldCreatePasswordHash()
    {
        // Arrange - Real Argon2 hash format
        const string realArgon2Hash = "$argon2id$v=19$m=65536,t=2,p=1$c29tZXNhbHQ$RdescudvJCsgt3ub+b+dWRWJTmaaJObG";

        // Act
        var passwordHash = new PasswordHash(realArgon2Hash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(realArgon2Hash));
    }

    [Test]
    public void Constructor_WithRealWorldSHA256Hash_ShouldCreatePasswordHash()
    {
        // Arrange - SHA256 hash format
        const string sha256Hash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

        // Act
        var passwordHash = new PasswordHash(sha256Hash);

        // Assert
        Assert.That((string)passwordHash, Is.EqualTo(sha256Hash));
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void Value_ShouldReturnSameAsImplicitConversion()
    {
        // Arrange
        const string testHash = "$2b$12$test.hash.for.value.property";
        var passwordHash = new PasswordHash(testHash);

        // Act
        var directValue = passwordHash.Value;
        string implicitValue = passwordHash;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(testHash));
    }

    [Test]
    public void Value_WithCasePreservation_ShouldReturnOriginalValue()
    {
        // Arrange
        const string mixedCaseHash = "$2B$12$MiXeD.CaSe.HaSh";
        var passwordHash = new PasswordHash(mixedCaseHash);

        // Act
        var directValue = passwordHash.Value;
        string implicitValue = passwordHash;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(mixedCaseHash));
    }

    [Test]
    public void Value_WithSpecialCharacters_ShouldReturnOriginalValue()
    {
        // Arrange
        const string specialCharHash = "$2a$10$special./+chars=in*hash";
        var passwordHash = new PasswordHash(specialCharHash);

        // Act
        var directValue = passwordHash.Value;
        string implicitValue = passwordHash;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(specialCharHash));
    }

    [Test]
    public void Value_WithWhitespace_ShouldReturnOriginalValue()
    {
        // Arrange
        const string hashWithSpaces = " hash with spaces ";
        var passwordHash = new PasswordHash(hashWithSpaces);

        // Act
        var directValue = passwordHash.Value;
        string implicitValue = passwordHash;

        // Assert
        Assert.That(directValue, Is.EqualTo(implicitValue));
        Assert.That(directValue, Is.EqualTo(hashWithSpaces));
    }

    #endregion

    #region Validator Tests

    [Test]
    public void Validator_ShouldNotBeNull()
    {
        // Assert
        Assert.That(PasswordHash.Validator, Is.Not.Null);
    }

    [Test]
    public void Validator_WithValidHash_ShouldPass()
    {
        // Arrange
        const string validHash = "$2b$12$valid.hash.example";

        // Act
        var result = PasswordHash.Validator.Validate(validHash);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithInvalidHash_ShouldFail()
    {
        // Arrange
        var invalidHash = string.Empty;

        // Act
        var result = PasswordHash.Validator.Validate(invalidHash);

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
        var result = PasswordHash.Validator.Validate(emptyString);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public void Validator_WithTooLongHash_ShouldFail()
    {
        // Arrange
        var tooLongHash = new string('a', PasswordHash.MaxLength + 1);

        // Act
        var result = PasswordHash.Validator.Validate(tooLongHash);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public void Validator_WithValidHashAtMaxLength_ShouldPass()
    {
        // Arrange
        var maxLengthHash = new string('a', PasswordHash.MaxLength);

        // Act
        var result = PasswordHash.Validator.Validate(maxLengthHash);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithComplexValidHash_ShouldPass()
    {
        // Arrange
        const string complexHash = "$argon2id$v=19$m=65536,t=2,p=1$example.salt$hash.value";

        // Act
        var result = PasswordHash.Validator.Validate(complexHash);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void Validator_WithVariousInvalidInputs_ShouldFail()
    {
        // Arrange
        var invalidInputs = new[]
        {
            string.Empty,
            "   ",
            "\t\t",
            "\n\r",
            new string('x', PasswordHash.MaxLength + 1),
            new string('y', PasswordHash.MaxLength + 100)
        };

        foreach (var invalidInput in invalidInputs)
        {
            // Act
            var result = PasswordHash.Validator.Validate(invalidInput);

            // Assert
            Assert.That(result.IsValid, Is.False, $"Input '{invalidInput}' should be invalid");
            Assert.That(result.Errors, Is.Not.Empty, $"Input '{invalidInput}' should have validation errors");
        }
    }

    [Test]
    public void Validator_WithValidInputs_ShouldPass()
    {
        // Arrange
        var validInputs = new[]
        {
            "a",
            "$2b$12$valid.hash",
            "$argon2id$v=19$example",
            "simple_hash_value",
            new string('z', PasswordHash.MaxLength),
            new string('w', PasswordHash.MaxLength - 1)
        };

        foreach (var validInput in validInputs)
        {
            // Act
            var result = PasswordHash.Validator.Validate(validInput);

            // Assert
            Assert.That(result.IsValid, Is.True, $"Input '{validInput}' should be valid");
            Assert.That(result.Errors, Is.Empty, $"Input '{validInput}' should have no validation errors");
        }
    }

    #endregion

    #region No Parameterless Constructor Tests

    [Test]
    public void Constructor_Parameterless_ShouldNotExist()
    {
        // Arrange
        var constructors = typeof(PasswordHash).GetConstructors();
        var parameterlessConstructors = constructors.Where(c => c.GetParameters().Length == 0);

        // Assert
        Assert.That(parameterlessConstructors, Is.Empty, "PasswordHash should not have a parameterless constructor");
    }

    [Test]
    public void Constructor_OnlyParameterizedConstructor_ShouldExist()
    {
        // Arrange
        var constructors = typeof(PasswordHash).GetConstructors();
        var parameterizedConstructors = constructors.Where(c => c.GetParameters().Length == 1).ToList();

        // Assert
        Assert.That(parameterizedConstructors.Count, Is.EqualTo(1), "PasswordHash should have exactly one parameterized constructor");
        Assert.That(parameterizedConstructors.First().GetParameters()[0].ParameterType, Is.EqualTo(typeof(string)));
    }

    #endregion

    #region Case Preservation Tests

    [Test]
    public void Constructor_CasePreservation_ShouldPreserveOriginalCase()
    {
        // Arrange
        var testCases = new[]
        {
            ("$2B$12$UPPERCASE.HASH", "$2B$12$UPPERCASE.HASH"),
            ("$2b$12$lowercase.hash", "$2b$12$lowercase.hash"),
            ("$2A$10$MiXeD.CaSe.HaSh", "$2A$10$MiXeD.CaSe.HaSh"),
            ("SHA256_HASH_UPPERCASE", "SHA256_HASH_UPPERCASE"),
            ("argon2_hash_lowercase", "argon2_hash_lowercase")
        };

        foreach (var (input, expected) in testCases)
        {
            // Act
            var passwordHash = new PasswordHash(input);

            // Assert
            Assert.That((string)passwordHash, Is.EqualTo(expected), $"Input '{input}' should preserve case as '{expected}'");
        }
    }

    [Test]
    public void ImplicitConversion_CasePreservation_ShouldPreserveOriginalCase()
    {
        // Arrange
        const string mixedCaseHash = "$2B$12$MiXeD.CaSe.ExAmPlE";

        // Act
        PasswordHash passwordHash = mixedCaseHash;
        string result = passwordHash;

        // Assert
        Assert.That(result, Is.EqualTo(mixedCaseHash));
    }

    [Test]
    public void Value_AfterCasePreservation_ShouldReturnOriginalValue()
    {
        // Arrange
        const string originalCaseHash = "$2A$10$Original.Case.Value";

        // Act
        var passwordHash = new PasswordHash(originalCaseHash);

        // Assert
        Assert.That(passwordHash.Value, Is.EqualTo(originalCaseHash));
    }

    #endregion
}
