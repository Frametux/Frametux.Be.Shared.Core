namespace Frametux.Shared.Core.Driven.Validation;

public class CustomLanguageManager : FluentValidation.Resources.LanguageManager
{
    public CustomLanguageManager() 
    {
        AddTranslation("en", "EmailValidator", "Must be a valid email address.");
        AddTranslation("en", "GreaterThanOrEqualValidator", "Must be greater than or equal to '{ComparisonValue}'.");
        AddTranslation("en", "GreaterThanValidator", "Must be greater than '{ComparisonValue}'.");
        AddTranslation("en", "LengthValidator", "Must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters.");
        AddTranslation("en", "MinimumLengthValidator", "The length must be at least {MinLength} characters. You entered {TotalLength} characters.");
        AddTranslation("en", "MaximumLengthValidator", "The length must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");
        AddTranslation("en", "LessThanOrEqualValidator", "Must be less than or equal to '{ComparisonValue}'.");
        AddTranslation("en", "LessThanValidator", "Must be less than '{ComparisonValue}'.");
        AddTranslation("en", "NotEmptyValidator", "Must not be empty.");
        AddTranslation("en", "NotEqualValidator", "Must not be equal to '{ComparisonValue}'.");
        AddTranslation("en", "NotNullValidator", "Must not be empty.");
        AddTranslation("en", "PredicateValidator", "The specified condition was not met.");
        AddTranslation("en", "AsyncPredicateValidator", "The specified condition was not met.");
        AddTranslation("en", "RegularExpressionValidator", "Is not in the correct format.");
        AddTranslation("en", "EqualValidator", "Must be equal to '{ComparisonValue}'.");
        AddTranslation("en", "ExactLengthValidator", "Must be exactly {MaxLength} characters in length. You entered {TotalLength} characters.");
        AddTranslation("en", "InclusiveBetweenValidator", "Must be between {From} and {To}. You entered {PropertyValue}.");
        AddTranslation("en", "ExclusiveBetweenValidator", "Must be between {From} and {To} (exclusive). You entered {PropertyValue}.");
        AddTranslation("en", "CreditCardValidator", "Is not a valid credit card number.");
        AddTranslation("en", "ScalePrecisionValidator", "Must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found.");
        AddTranslation("en", "EmptyValidator", "Must be empty.");
        AddTranslation("en", "NullValidator", "Must be empty.");
        AddTranslation("en", "EnumValidator", "Has a range of values which does not include '{PropertyValue}'.");
        // Additional fallback messages used by clientside validation integration.
        AddTranslation("en", "Length_Simple", "Must be between {MinLength} and {MaxLength} characters.");
        AddTranslation("en", "MinimumLength_Simple", "The length must be at least {MinLength} characters.");
        AddTranslation("en", "MaximumLength_Simple", "The length must be {MaxLength} characters or fewer.");
        AddTranslation("en", "ExactLength_Simple", "Must be exactly {MaxLength} characters in length.");
        AddTranslation("en", "InclusiveBetween_Simple", "Must be between {From} and {To}.");
    }
}