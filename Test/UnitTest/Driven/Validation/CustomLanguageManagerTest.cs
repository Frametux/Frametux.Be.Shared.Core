using FluentValidation;
using Frametux.Shared.Core.Driven.Validation;

namespace UnitTest.Driven.Validation;

[TestFixture]
public class CustomLanguageManagerTest
{
    private CustomLanguageManager? _customLanguageManager;

    [SetUp]
    public void SetUp()
    {
        // Create and set the custom language manager
        _customLanguageManager = new CustomLanguageManager();
        ValidatorOptions.Global.LanguageManager = _customLanguageManager;
    }

    [Test]
    public void DirectTranslation_EmailValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("EmailValidator");

        // Assert
        // CustomLanguageManager: "Must be a valid email address."
        Assert.That(message, Is.EqualTo("Must be a valid email address."));
    }

    [Test]
    public void DirectTranslation_GreaterThanOrEqualValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("GreaterThanOrEqualValidator");

        // Assert
        // CustomLanguageManager: "Must be greater than or equal to '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must be greater than or equal to '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_GreaterThanValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("GreaterThanValidator");

        // Assert
        // CustomLanguageManager: "Must be greater than '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must be greater than '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_LengthValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("LengthValidator");

        // Assert
        // CustomLanguageManager: "Must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters."
        Assert.That(message, Is.EqualTo("Must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters."));
    }

    [Test]
    public void DirectTranslation_MinimumLengthValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("MinimumLengthValidator");

        // Assert
        // CustomLanguageManager: "The length must be at least {MinLength} characters. You entered {TotalLength} characters."
        Assert.That(message, Is.EqualTo("The length must be at least {MinLength} characters. You entered {TotalLength} characters."));
    }

    [Test]
    public void DirectTranslation_MaximumLengthValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("MaximumLengthValidator");

        // Assert
        // CustomLanguageManager: "The length must be {MaxLength} characters or fewer. You entered {TotalLength} characters."
        Assert.That(message, Is.EqualTo("The length must be {MaxLength} characters or fewer. You entered {TotalLength} characters."));
    }

    [Test]
    public void DirectTranslation_LessThanOrEqualValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("LessThanOrEqualValidator");

        // Assert
        // CustomLanguageManager: "Must be less than or equal to '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must be less than or equal to '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_LessThanValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("LessThanValidator");

        // Assert
        // CustomLanguageManager: "Must be less than '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must be less than '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_NotEmptyValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("NotEmptyValidator");

        // Assert
        // CustomLanguageManager: "Must not be empty."
        Assert.That(message, Is.EqualTo("Must not be empty."));
    }

    [Test]
    public void DirectTranslation_NotEqualValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("NotEqualValidator");

        // Assert
        // CustomLanguageManager: "Must not be equal to '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must not be equal to '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_NotNullValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("NotNullValidator");

        // Assert
        // CustomLanguageManager: "Must not be empty."
        Assert.That(message, Is.EqualTo("Must not be empty."));
    }

    [Test]
    public void DirectTranslation_PredicateValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("PredicateValidator");

        // Assert
        // CustomLanguageManager: "The specified condition was not met."
        Assert.That(message, Is.EqualTo("The specified condition was not met."));
    }

    [Test]
    public void DirectTranslation_AsyncPredicateValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("AsyncPredicateValidator");

        // Assert
        // CustomLanguageManager: "The specified condition was not met."
        Assert.That(message, Is.EqualTo("The specified condition was not met."));
    }

    [Test]
    public void DirectTranslation_RegularExpressionValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("RegularExpressionValidator");

        // Assert
        // CustomLanguageManager: "Is not in the correct format."
        Assert.That(message, Is.EqualTo("Is not in the correct format."));
    }

    [Test]
    public void DirectTranslation_EqualValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("EqualValidator");

        // Assert
        // CustomLanguageManager: "Must be equal to '{ComparisonValue}'."
        Assert.That(message, Is.EqualTo("Must be equal to '{ComparisonValue}'."));
    }

    [Test]
    public void DirectTranslation_ExactLengthValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("ExactLengthValidator");

        // Assert
        // CustomLanguageManager: "Must be exactly {MaxLength} characters in length. You entered {TotalLength} characters."
        Assert.That(message, Is.EqualTo("Must be exactly {MaxLength} characters in length. You entered {TotalLength} characters."));
    }

    [Test]
    public void DirectTranslation_InclusiveBetweenValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("InclusiveBetweenValidator");

        // Assert
        // CustomLanguageManager: "Must be between {From} and {To}. You entered {PropertyValue}."
        Assert.That(message, Is.EqualTo("Must be between {From} and {To}. You entered {PropertyValue}."));
    }

    [Test]
    public void DirectTranslation_ExclusiveBetweenValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("ExclusiveBetweenValidator");

        // Assert
        // CustomLanguageManager: "Must be between {From} and {To} (exclusive). You entered {PropertyValue}."
        Assert.That(message, Is.EqualTo("Must be between {From} and {To} (exclusive). You entered {PropertyValue}."));
    }

    [Test]
    public void DirectTranslation_CreditCardValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("CreditCardValidator");

        // Assert
        // CustomLanguageManager: "Is not a valid credit card number."
        Assert.That(message, Is.EqualTo("Is not a valid credit card number."));
    }

    [Test]
    public void DirectTranslation_ScalePrecisionValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("ScalePrecisionValidator");

        // Assert
        // CustomLanguageManager: "Must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found."
        Assert.That(message, Is.EqualTo("Must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found."));
    }

    [Test]
    public void DirectTranslation_EmptyValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("EmptyValidator");

        // Assert
        // CustomLanguageManager: "Must be empty."
        Assert.That(message, Is.EqualTo("Must be empty."));
    }

    [Test]
    public void DirectTranslation_NullValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("NullValidator");

        // Assert
        // CustomLanguageManager: "Must be empty."
        Assert.That(message, Is.EqualTo("Must be empty."));
    }

    [Test]
    public void DirectTranslation_EnumValidator_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("EnumValidator");

        // Assert
        // CustomLanguageManager: "Has a range of values which does not include '{PropertyValue}'."
        Assert.That(message, Is.EqualTo("Has a range of values which does not include '{PropertyValue}'."));
    }

    [Test]
    public void DirectTranslation_LengthSimple_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("Length_Simple");

        // Assert
        // CustomLanguageManager: "Must be between {MinLength} and {MaxLength} characters."
        Assert.That(message, Is.EqualTo("Must be between {MinLength} and {MaxLength} characters."));
    }

    [Test]
    public void DirectTranslation_MinimumLengthSimple_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("MinimumLength_Simple");

        // Assert
        // CustomLanguageManager: "The length must be at least {MinLength} characters."
        Assert.That(message, Is.EqualTo("The length must be at least {MinLength} characters."));
    }

    [Test]
    public void DirectTranslation_MaximumLengthSimple_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("MaximumLength_Simple");

        // Assert
        // CustomLanguageManager: "The length must be {MaxLength} characters or fewer."
        Assert.That(message, Is.EqualTo("The length must be {MaxLength} characters or fewer."));
    }

    [Test]
    public void DirectTranslation_ExactLengthSimple_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("ExactLength_Simple");

        // Assert
        // CustomLanguageManager: "Must be exactly {MaxLength} characters in length."
        Assert.That(message, Is.EqualTo("Must be exactly {MaxLength} characters in length."));
    }

    [Test]
    public void DirectTranslation_InclusiveBetweenSimple_ShouldReturnCustomMessage()
    {
        // Act
        var message = _customLanguageManager!.GetString("InclusiveBetween_Simple");

        // Assert
        // CustomLanguageManager: "Must be between {From} and {To}."
        Assert.That(message, Is.EqualTo("Must be between {From} and {To}."));
    }
}
