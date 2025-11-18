using System.Text.Json;
using FluentValidation.Results;
using Frametux.Shared.Core.Domain.Exceptions;
using Frametux.Shared.Core.Driving.Responses.Error;

namespace UnitTest.Driving.Response;

[TestFixture]
public class ErrorWithDetailsResponseTest
{
    #region Test Helper Classes

    private class MockException : Exception, IExcHasErrorCode
    {
        public string Code { get; }
        public override string Message { get; }

        public MockException(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    #endregion

    #region Constructor and Inheritance Tests

    [Test]
    public void Constructor_ShouldSetIsSuccessToFalse()
    {
        // Arrange & Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Error message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void Constructor_WithValidationFailed_ShouldInitializeAllProperties()
    {
        // Arrange
        const string expectedMessage = "Validation failed";
        const ErrorType expectedType = ErrorType.ValidationFailed;
        var expectedErrors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST001", Message = "Test error" }]
            }
        };

        // Act
        var response = new ErrorWithDetailsResponse
        {
            Message = expectedMessage,
            ErrorType = expectedType,
            Errors = expectedErrors
        };

        // Assert
        Assert.That(response.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo(expectedMessage));
        Assert.That(response.ErrorType, Is.EqualTo(expectedType));
        Assert.That(response.Errors, Is.EqualTo(expectedErrors));
    }

    [Test]
    public void Constructor_WithEmptyErrorsList_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Assert
        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Message, Is.EqualTo("Test message"));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
    }

    [Test]
    public void Constructor_WithMultipleErrors_ShouldInitializeAllErrors()
    {
        // Arrange
        var expectedErrors = new List<Error>
        {
            new Error
            {
                Field = "Field1",
                Errors = [new ErrorContent { Code = "ERR001", Message = "Error 1" }]
            },
            new Error
            {
                Field = "Field2",
                Errors = [new ErrorContent { Code = "ERR002", Message = "Error 2" }]
            }
        };

        // Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Multiple errors",
            ErrorType = ErrorType.ValidationFailed,
            Errors = expectedErrors
        };

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(2));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Field1"));
        Assert.That(response.Errors[1].Field, Is.EqualTo("Field2"));
    }

    #endregion

    #region Errors Property Tests

    [Test]
    public void Errors_WithSingleError_ShouldReturnCorrectValue()
    {
        // Arrange
        var error = new Error
        {
            Field = "TestField",
            Errors = [new ErrorContent { Code = "TEST001", Message = "Test error" }]
        };

        // Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test",
            ErrorType = ErrorType.ValidationFailed,
            Errors = [error]
        };

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0], Is.EqualTo(error));
    }

    [Test]
    public void Errors_WithMultipleErrorsPerField_ShouldReturnAllErrors()
    {
        // Arrange
        var error = new Error
        {
            Field = "Password",
            Errors =
            [
                new ErrorContent { Code = "MIN_LENGTH", Message = "Password too short" },
                new ErrorContent { Code = "SPECIAL_CHAR", Message = "Missing special character" },
                new ErrorContent { Code = "UPPERCASE", Message = "Missing uppercase letter" }
            ]
        };

        // Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed",
            ErrorType = ErrorType.ValidationFailed,
            Errors = [error]
        };

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Errors.Count, Is.EqualTo(3));
        Assert.That(response.Errors[0].Errors[0].Code, Is.EqualTo("MIN_LENGTH"));
        Assert.That(response.Errors[0].Errors[1].Code, Is.EqualTo("SPECIAL_CHAR"));
        Assert.That(response.Errors[0].Errors[2].Code, Is.EqualTo("UPPERCASE"));
    }

    [Test]
    public void Errors_WithMultipleFields_ShouldMaintainOrder()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error { Field = "Email", Errors = [new ErrorContent { Code = "INVALID", Message = "Invalid email" }] },
            new Error { Field = "Username", Errors = [new ErrorContent { Code = "REQUIRED", Message = "Username required" }] },
            new Error { Field = "Password", Errors = [new ErrorContent { Code = "WEAK", Message = "Weak password" }] }
        };

        // Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(3));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Email"));
        Assert.That(response.Errors[1].Field, Is.EqualTo("Username"));
        Assert.That(response.Errors[2].Field, Is.EqualTo("Password"));
    }

    #endregion

    #region Static Factory Method: ToErrorResponse(ValidationResult) Tests

    [Test]
    public void ToErrorResponse_WithEmptyValidationResult_ShouldReturnEmptyErrors()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(validationResult);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Validation Failed."));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void ToErrorResponse_WithSingleValidationError_ShouldMapCorrectly()
    {
        // Arrange
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Email", "Email is required")
            {
                ErrorCode = "REQUIRED"
            }
        ]);

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(validationResult);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Validation Failed."));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Email"));
        Assert.That(response.Errors[0].Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Errors[0].Code, Is.EqualTo("REQUIRED"));
        Assert.That(response.Errors[0].Errors[0].Message, Is.EqualTo("Email is required"));
    }

    [Test]
    public void ToErrorResponse_WithMultipleErrorsOnSameField_ShouldGroupCorrectly()
    {
        // Arrange
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Password", "Password is too short")
            {
                ErrorCode = "MIN_LENGTH"
            },
            new ValidationFailure("Password", "Password must contain uppercase")
            {
                ErrorCode = "UPPERCASE"
            },
            new ValidationFailure("Password", "Password must contain special character")
            {
                ErrorCode = "SPECIAL_CHAR"
            }
        ]);

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(validationResult);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Validation Failed."));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Password"));
        Assert.That(response.Errors[0].Errors.Count, Is.EqualTo(3));
        
        var errorCodes = response.Errors[0].Errors.Select(e => e.Code).ToList();
        Assert.That(errorCodes, Does.Contain("MIN_LENGTH"));
        Assert.That(errorCodes, Does.Contain("UPPERCASE"));
        Assert.That(errorCodes, Does.Contain("SPECIAL_CHAR"));
    }

    [Test]
    public void ToErrorResponse_WithErrorsOnDifferentFields_ShouldCreateSeparateGroups()
    {
        // Arrange
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Email", "Email is required")
            {
                ErrorCode = "REQUIRED"
            },
            new ValidationFailure("Username", "Username is too short")
            {
                ErrorCode = "MIN_LENGTH"
            },
            new ValidationFailure("Password", "Password is required")
            {
                ErrorCode = "REQUIRED"
            }
        ]);

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(validationResult);

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(3));
        
        var fieldNames = response.Errors.Select(e => e.Field).ToList();
        Assert.That(fieldNames, Does.Contain("Email"));
        Assert.That(fieldNames, Does.Contain("Username"));
        Assert.That(fieldNames, Does.Contain("Password"));
    }

    [Test]
    public void ToErrorResponse_WithComplexValidationResult_ShouldHandleCorrectly()
    {
        // Arrange
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Email", "Email is invalid")
            {
                ErrorCode = "INVALID_FORMAT"
            },
            new ValidationFailure("Email", "Email already exists")
            {
                ErrorCode = "DUPLICATE"
            },
            new ValidationFailure("Password", "Password too short")
            {
                ErrorCode = "MIN_LENGTH"
            },
            new ValidationFailure("Username", "Username required")
            {
                ErrorCode = "REQUIRED"
            }
        ]);

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(validationResult);

        // Assert
        Assert.That(response.Errors.Count, Is.EqualTo(3));
        
        var emailErrors = response.Errors.First(e => e.Field == "Email");
        Assert.That(emailErrors.Errors.Count, Is.EqualTo(2));
        
        var passwordErrors = response.Errors.First(e => e.Field == "Password");
        Assert.That(passwordErrors.Errors.Count, Is.EqualTo(1));
        
        var usernameErrors = response.Errors.First(e => e.Field == "Username");
        Assert.That(usernameErrors.Errors.Count, Is.EqualTo(1));
    }

    #endregion

    #region Static Factory Method: ToErrorResponse<TExc> Tests

    [Test]
    public void ToErrorResponse_WithException_ShouldMapAllProperties()
    {
        // Arrange
        var exception = new MockException("DUPLICATE_EMAIL", "Email already exists");
        const string fieldName = "Email";
        const ErrorType errorType = ErrorType.BusinessLogicFailed;

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, errorType, exception);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Email already exists"));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.BusinessLogicFailed));
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Email"));
        Assert.That(response.Errors[0].Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Errors[0].Code, Is.EqualTo("DUPLICATE_EMAIL"));
        Assert.That(response.Errors[0].Errors[0].Message, Is.EqualTo("Email already exists"));
        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void ToErrorResponse_WithException_NotFoundErrorType_ShouldSetCorrectErrorType()
    {
        // Arrange
        var exception = new MockException("NOT_FOUND", "User not found");
        const string fieldName = "UserId";
        const ErrorType errorType = ErrorType.NotFound;

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, errorType, exception);

        // Assert
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.NotFound));
        Assert.That(response.Message, Is.EqualTo("User not found"));
    }

    [Test]
    public void ToErrorResponse_WithException_ValidationFailedErrorType_ShouldSetCorrectErrorType()
    {
        // Arrange
        var exception = new MockException("INVALID_FORMAT", "Invalid email format");
        const string fieldName = "Email";
        const ErrorType errorType = ErrorType.ValidationFailed;

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, errorType, exception);

        // Assert
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(response.Message, Is.EqualTo("Invalid email format"));
    }

    [Test]
    public void ToErrorResponse_WithException_ShouldMapFieldNameCorrectly()
    {
        // Arrange
        var exception = new MockException("WEAK_PASSWORD", "Password is too weak");
        const string fieldName = "Password";

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, ErrorType.BusinessLogicFailed, exception);

        // Assert
        Assert.That(response.Errors[0].Field, Is.EqualTo("Password"));
    }

    [Test]
    public void ToErrorResponse_WithException_ShouldMapErrorCodeCorrectly()
    {
        // Arrange
        var exception = new MockException("EXPIRED_TOKEN", "Token has expired");
        const string fieldName = "Token";

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, ErrorType.BusinessLogicFailed, exception);

        // Assert
        Assert.That(response.Errors[0].Errors[0].Code, Is.EqualTo("EXPIRED_TOKEN"));
    }

    [Test]
    public void ToErrorResponse_WithException_ShouldMapErrorMessageCorrectly()
    {
        // Arrange
        var exception = new MockException("INSUFFICIENT_PERMISSIONS", "User lacks required permissions");
        const string fieldName = "Permissions";

        // Act
        var response = ErrorWithDetailsResponse.ToErrorResponse(fieldName, ErrorType.BusinessLogicFailed, exception);

        // Assert
        Assert.That(response.Errors[0].Errors[0].Message, Is.EqualTo("User lacks required permissions"));
    }

    #endregion

    #region Error and ErrorContent Record Tests

    [Test]
    public void Error_Initialization_ShouldSetAllProperties()
    {
        // Arrange & Act
        var error = new Error
        {
            Field = "TestField",
            Errors =
            [
                new ErrorContent { Code = "TEST001", Message = "Test error" }
            ]
        };

        // Assert
        Assert.That(error.Field, Is.EqualTo("TestField"));
        Assert.That(error.Errors.Count, Is.EqualTo(1));
        Assert.That(error.Errors[0].Code, Is.EqualTo("TEST001"));
        Assert.That(error.Errors[0].Message, Is.EqualTo("Test error"));
    }

    [Test]
    public void ErrorContent_Initialization_ShouldSetAllProperties()
    {
        // Arrange & Act
        var errorContent = new ErrorContent
        {
            Code = "ERR001",
            Message = "Error message"
        };

        // Assert
        Assert.That(errorContent.Code, Is.EqualTo("ERR001"));
        Assert.That(errorContent.Message, Is.EqualTo("Error message"));
    }

    [Test]
    public void Error_Equality_WithSameValues_ShouldNotBeEqual_DifferentListInstances()
    {
        // Arrange
        var error1 = new Error
        {
            Field = "TestField",
            Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
        };
        var error2 = new Error
        {
            Field = "TestField",
            Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
        };

        // Act & Assert
        // Records compare List<T> by reference, not by value
        Assert.That(error1, Is.Not.EqualTo(error2));
        Assert.That(error1 != error2, Is.True);
    }

    [Test]
    public void Error_Equality_WithSameListReference_ShouldBeEqual()
    {
        // Arrange
        var sharedErrors = new List<ErrorContent> { new ErrorContent { Code = "TEST", Message = "Test" } };
        var error1 = new Error
        {
            Field = "TestField",
            Errors = sharedErrors
        };
        var error2 = new Error
        {
            Field = "TestField",
            Errors = sharedErrors
        };

        // Act & Assert
        Assert.That(error1, Is.EqualTo(error2));
        Assert.That(error1 == error2, Is.True);
    }

    [Test]
    public void Error_Equality_WithDifferentField_ShouldNotBeEqual()
    {
        // Arrange
        var error1 = new Error
        {
            Field = "Field1",
            Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
        };
        var error2 = new Error
        {
            Field = "Field2",
            Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
        };

        // Act & Assert
        Assert.That(error1, Is.Not.EqualTo(error2));
        Assert.That(error1 != error2, Is.True);
    }

    [Test]
    public void ErrorContent_Equality_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var content1 = new ErrorContent { Code = "ERR001", Message = "Error" };
        var content2 = new ErrorContent { Code = "ERR001", Message = "Error" };

        // Act & Assert
        Assert.That(content1, Is.EqualTo(content2));
        Assert.That(content1 == content2, Is.True);
    }

    [Test]
    public void ErrorContent_Equality_WithDifferentCode_ShouldNotBeEqual()
    {
        // Arrange
        var content1 = new ErrorContent { Code = "ERR001", Message = "Error" };
        var content2 = new ErrorContent { Code = "ERR002", Message = "Error" };

        // Act & Assert
        Assert.That(content1, Is.Not.EqualTo(content2));
        Assert.That(content1 != content2, Is.True);
    }

    [Test]
    public void ErrorContent_Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var content1 = new ErrorContent { Code = "ERR001", Message = "Error 1" };
        var content2 = new ErrorContent { Code = "ERR001", Message = "Error 2" };

        // Act & Assert
        Assert.That(content1, Is.Not.EqualTo(content2));
        Assert.That(content1 != content2, Is.True);
    }

    #endregion

    #region Record Equality Tests

    [Test]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test error" }]
            }
        };

        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
        Assert.That(response1 == response2, Is.True);
        Assert.That(response1.GetHashCode(), Is.EqualTo(response2.GetHashCode()));
    }

    [Test]
    public void Equality_WithDifferentErrors_ShouldNotBeEqual()
    {
        // Arrange
        var errors1 = new List<Error>
        {
            new Error
            {
                Field = "Field1",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var errors2 = new List<Error>
        {
            new Error
            {
                Field = "Field2",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors1
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors2
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentMessage_ShouldNotBeEqual()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Message 1",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Message 2",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithDifferentErrorType_ShouldNotBeEqual()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.BusinessLogicFailed,
            Errors = errors
        };

        // Act & Assert
        Assert.That(response1, Is.Not.EqualTo(response2));
        Assert.That(response1 != response2, Is.True);
    }

    [Test]
    public void Equality_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Act & Assert
        Assert.That(response, Is.Not.EqualTo(null));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.That(response is null, Is.False);
    }

    [Test]
    public void Equality_SameReference_ShouldBeEqual()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };
        var sameReference = response;

        // Act & Assert
        Assert.That(response, Is.EqualTo(sameReference));
        // ReSharper disable once EqualExpressionComparison
        Assert.That(response == sameReference, Is.True);
        Assert.That(ReferenceEquals(response, sameReference), Is.True);
    }

    [Test]
    public void Equality_WithEmptyErrorsLists_ShouldNotBeEqual_DifferentListInstances()
    {
        // Arrange
        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Act & Assert
        // Records compare List<T> by reference, not by value
        Assert.That(response1, Is.Not.EqualTo(response2));
    }

    [Test]
    public void Equality_WithSameEmptyListReference_ShouldBeEqual()
    {
        // Arrange
        var sharedErrors = new List<Error>();

        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test",
            ErrorType = ErrorType.ValidationFailed,
            Errors = sharedErrors
        };

        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test",
            ErrorType = ErrorType.ValidationFailed,
            Errors = sharedErrors
        };

        // Act & Assert
        Assert.That(response1, Is.EqualTo(response2));
    }

    #endregion

    #region Record With Expression Tests

    [Test]
    public void WithExpression_ModifyingErrors_ShouldCreateNewInstance()
    {
        // Arrange
        var originalErrors = new List<Error>
        {
            new Error
            {
                Field = "Field1",
                Errors = [new ErrorContent { Code = "ERR001", Message = "Error 1" }]
            }
        };

        var original = new ErrorWithDetailsResponse
        {
            Message = "Original message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = originalErrors
        };

        var newErrors = new List<Error>
        {
            new Error
            {
                Field = "Field2",
                Errors = [new ErrorContent { Code = "ERR002", Message = "Error 2" }]
            }
        };

        // Act
        var modified = original with { Errors = newErrors };

        // Assert
        Assert.That(modified.Errors, Is.EqualTo(newErrors));
        Assert.That(original.Errors, Is.EqualTo(originalErrors));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(modified.ErrorType, Is.EqualTo(original.ErrorType));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingMessage_ShouldCreateNewInstance()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var original = new ErrorWithDetailsResponse
        {
            Message = "Original message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Act
        var modified = original with { Message = "Modified message" };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(modified.Errors, Is.EqualTo(original.Errors));
        Assert.That(modified.ErrorType, Is.EqualTo(original.ErrorType));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingErrorType_ShouldCreateNewInstance()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var original = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Act
        var modified = original with { ErrorType = ErrorType.BusinessLogicFailed };

        // Assert
        Assert.That(modified.ErrorType, Is.EqualTo(ErrorType.BusinessLogicFailed));
        Assert.That(original.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(modified.Message, Is.EqualTo(original.Message));
        Assert.That(modified.Errors, Is.EqualTo(original.Errors));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_ModifyingAllProperties_ShouldCreateNewInstance()
    {
        // Arrange
        var originalErrors = new List<Error>
        {
            new Error
            {
                Field = "Field1",
                Errors = [new ErrorContent { Code = "ERR001", Message = "Error 1" }]
            }
        };

        var original = new ErrorWithDetailsResponse
        {
            Message = "Original message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = originalErrors
        };

        var newErrors = new List<Error>
        {
            new Error
            {
                Field = "Field2",
                Errors = [new ErrorContent { Code = "ERR002", Message = "Error 2" }]
            }
        };

        // Act
        var modified = original with
        {
            Message = "Modified message",
            ErrorType = ErrorType.BusinessLogicFailed,
            Errors = newErrors
        };

        // Assert
        Assert.That(modified.Message, Is.EqualTo("Modified message"));
        Assert.That(modified.ErrorType, Is.EqualTo(ErrorType.BusinessLogicFailed));
        Assert.That(modified.Errors, Is.EqualTo(newErrors));
        Assert.That(original.Message, Is.EqualTo("Original message"));
        Assert.That(original.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(original.Errors, Is.EqualTo(originalErrors));
        Assert.That(ReferenceEquals(original, modified), Is.False);
    }

    [Test]
    public void WithExpression_WithoutChanges_ShouldCreateNewInstance()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error
            {
                Field = "TestField",
                Errors = [new ErrorContent { Code = "TEST", Message = "Test" }]
            }
        };

        var original = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = errors
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.That(copy.Message, Is.EqualTo(original.Message));
        Assert.That(copy.ErrorType, Is.EqualTo(original.ErrorType));
        Assert.That(copy.Errors, Is.EqualTo(original.Errors));
        Assert.That(copy, Is.EqualTo(original));
        Assert.That(ReferenceEquals(original, copy), Is.False);
    }

    #endregion

    #region JSON Serialization Tests

    [Test]
    public void JsonSerialization_WithSingleError_ShouldSerializeCorrectly()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed.",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Email",
                    Errors = [new ErrorContent { Code = "REQUIRED", Message = "Email is required" }]
                }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"ValidationFailed\""));
        Assert.That(json, Does.Contain("\"Email\""));
        Assert.That(json, Does.Contain("\"REQUIRED\""));
        Assert.That(json, Does.Contain("\"Email is required\""));
        Assert.That(json, Does.Contain("\"Validation Failed.\""));
    }

    [Test]
    public void JsonSerialization_WithMultipleErrors_ShouldSerializeCorrectly()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed.",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Email",
                    Errors = [new ErrorContent { Code = "REQUIRED", Message = "Email is required" }]
                },
                new Error
                {
                    Field = "Password",
                    Errors =
                    [
                        new ErrorContent { Code = "MIN_LENGTH", Message = "Password too short" },
                        new ErrorContent { Code = "UPPERCASE", Message = "Missing uppercase" }
                    ]
                }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        Assert.That(json, Does.Contain("\"Email\""));
        Assert.That(json, Does.Contain("\"Password\""));
        Assert.That(json, Does.Contain("\"MIN_LENGTH\""));
        Assert.That(json, Does.Contain("\"UPPERCASE\""));
    }

    [Test]
    public void JsonDeserialization_WithSingleError_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = @"{
            ""IsSuccess"": false,
            ""Message"": ""Validation Failed."",
            ""ErrorType"": ""ValidationFailed"",
            ""Errors"": [
                {
                    ""Field"": ""Email"",
                    ""Errors"": [
                        {
                            ""Code"": ""REQUIRED"",
                            ""Message"": ""Email is required""
                        }
                    ]
                }
            ]
        }";

        // Act
        var response = JsonSerializer.Deserialize<ErrorWithDetailsResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo("Validation Failed."));
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
        Assert.That(response.Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Email"));
        Assert.That(response.Errors[0].Errors[0].Code, Is.EqualTo("REQUIRED"));
        Assert.That(response.Errors[0].Errors[0].Message, Is.EqualTo("Email is required"));
    }

    [Test]
    public void JsonDeserialization_WithMultipleErrors_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = @"{
            ""IsSuccess"": false,
            ""Message"": ""Validation Failed."",
            ""ErrorType"": ""ValidationFailed"",
            ""Errors"": [
                {
                    ""Field"": ""Email"",
                    ""Errors"": [
                        {
                            ""Code"": ""INVALID"",
                            ""Message"": ""Invalid email""
                        }
                    ]
                },
                {
                    ""Field"": ""Password"",
                    ""Errors"": [
                        {
                            ""Code"": ""WEAK"",
                            ""Message"": ""Weak password""
                        },
                        {
                            ""Code"": ""SHORT"",
                            ""Message"": ""Too short""
                        }
                    ]
                }
            ]
        }";

        // Act
        var response = JsonSerializer.Deserialize<ErrorWithDetailsResponse>(json);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Errors.Count, Is.EqualTo(2));
        Assert.That(response.Errors[0].Field, Is.EqualTo("Email"));
        Assert.That(response.Errors[0].Errors.Count, Is.EqualTo(1));
        Assert.That(response.Errors[1].Field, Is.EqualTo("Password"));
        Assert.That(response.Errors[1].Errors.Count, Is.EqualTo(2));
    }

    [Test]
    public void JsonRoundTrip_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var original = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed.",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Email",
                    Errors =
                    [
                        new ErrorContent { Code = "REQUIRED", Message = "Email is required" },
                        new ErrorContent { Code = "INVALID", Message = "Email is invalid" }
                    ]
                }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<ErrorWithDetailsResponse>(json);

        // Assert
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized!.Message, Is.EqualTo(original.Message));
        Assert.That(deserialized.ErrorType, Is.EqualTo(original.ErrorType));
        Assert.That(deserialized.Errors.Count, Is.EqualTo(original.Errors.Count));
        Assert.That(deserialized.Errors[0].Field, Is.EqualTo(original.Errors[0].Field));
        Assert.That(deserialized.Errors[0].Errors.Count, Is.EqualTo(original.Errors[0].Errors.Count));
    }

    [Test]
    public void JsonSerialization_WithEmptyErrors_ShouldSerializeCorrectly()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ErrorWithDetailsResponse>(json);

        // Assert
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized!.Errors, Is.Empty);
    }

    #endregion

    #region ToString Tests

    [Test]
    public void ToString_WithSingleError_ShouldContainRelevantInformation()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Validation Failed.",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Email",
                    Errors = [new ErrorContent { Code = "REQUIRED", Message = "Email is required" }]
                }
            ]
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorWithDetailsResponse"));
        Assert.That(result, Does.Contain("Validation Failed."));
        Assert.That(result, Does.Contain("ValidationFailed"));
    }

    [Test]
    public void ToString_WithMultipleErrors_ShouldContainRelevantInformation()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Multiple validation errors",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Email",
                    Errors = [new ErrorContent { Code = "INVALID", Message = "Invalid email" }]
                },
                new Error
                {
                    Field = "Password",
                    Errors = [new ErrorContent { Code = "WEAK", Message = "Weak password" }]
                }
            ]
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorWithDetailsResponse"));
        Assert.That(result, Does.Contain("Multiple validation errors"));
        Assert.That(result, Does.Contain("ValidationFailed"));
    }

    [Test]
    public void ToString_WithEmptyErrors_ShouldHandleGracefully()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.BusinessLogicFailed,
            Errors = []
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorWithDetailsResponse"));
        Assert.That(result, Does.Contain("Test message"));
        Assert.That(result, Does.Contain("BusinessLogicFailed"));
    }

    [Test]
    public void ToString_WithComplexErrorStructure_ShouldHandleCorrectly()
    {
        // Arrange
        var response = new ErrorWithDetailsResponse
        {
            Message = "Complex validation errors",
            ErrorType = ErrorType.ValidationFailed,
            Errors =
            [
                new Error
                {
                    Field = "Password",
                    Errors =
                    [
                        new ErrorContent { Code = "MIN_LENGTH", Message = "Too short" },
                        new ErrorContent { Code = "UPPERCASE", Message = "Missing uppercase" },
                        new ErrorContent { Code = "SPECIAL_CHAR", Message = "Missing special char" }
                    ]
                }
            ]
        };

        // Act
        var result = response.ToString();

        // Assert
        Assert.That(result, Does.Contain("ErrorWithDetailsResponse"));
        Assert.That(result, Is.Not.Empty);
    }

    #endregion

    #region Inheritance Tests

    [Test]
    public void Inheritance_ShouldInheritFromErrorResponse()
    {
        // Arrange & Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Assert
        Assert.That(response, Is.InstanceOf<ErrorResponse>());
        Assert.That(response, Is.InstanceOf<Frametux.Shared.Core.Driving.Responses.BaseResponse>());
    }

    [Test]
    public void Inheritance_IsSuccessProperty_ShouldAlwaysBeFalse()
    {
        // Arrange
        var response1 = new ErrorWithDetailsResponse
        {
            Message = "Test message 1",
            ErrorType = ErrorType.NotFound,
            Errors = []
        };
        var response2 = new ErrorWithDetailsResponse
        {
            Message = "Test message 2",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };
        var response3 = new ErrorWithDetailsResponse
        {
            Message = "Test message 3",
            ErrorType = ErrorType.BusinessLogicFailed,
            Errors = []
        };

        // Act & Assert
        Assert.That(response1.IsSuccess, Is.False);
        Assert.That(response2.IsSuccess, Is.False);
        Assert.That(response3.IsSuccess, Is.False);
    }

    [Test]
    public void Inheritance_ErrorTypeProperty_ShouldBeAccessible()
    {
        // Arrange & Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Test message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Assert - Can access ErrorType from base class
        Assert.That(response.ErrorType, Is.EqualTo(ErrorType.ValidationFailed));
    }

    [Test]
    public void Inheritance_MessageProperty_ShouldBeAccessible()
    {
        // Arrange & Act
        var response = new ErrorWithDetailsResponse
        {
            Message = "Inherited message",
            ErrorType = ErrorType.ValidationFailed,
            Errors = []
        };

        // Assert - Can access Message from base class
        Assert.That(response.Message, Is.EqualTo("Inherited message"));
    }

    #endregion
}

