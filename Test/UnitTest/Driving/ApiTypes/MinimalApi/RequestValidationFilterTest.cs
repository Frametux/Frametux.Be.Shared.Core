using FluentValidation;
using FluentValidation.Results;
using Frametux.Shared.Core.Driving.ApiTypes.MinimalApi.RequestValidation;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTest.Driving.ApiTypes.MinimalApi;

[TestFixture]
public class RequestValidationFilterTest
{
    #region Test Request Class

    public class TestRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    #endregion

    #region No Validator Scenario Tests

    [Test]
    public async Task InvokeAsync_WithNullValidator_ShouldCallNext()
    {
        // Arrange
        var filter = new RequestValidationFilter<TestRequest>(validator: null);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        
        var nextCalled = false;
        var expectedResult = Results.Ok("Success");
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx)
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(expectedResult);
        }

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(nextCalled, Is.True);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task InvokeAsync_WithNullValidator_ShouldNotCallValidator()
    {
        // Arrange
        var filter = new RequestValidationFilter<TestRequest>(validator: null);
        
        var testRequest = new TestRequest { Name = "Test", Email = "test@example.com", Age = 25 };
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        var expectedResult = Results.Ok("Success");
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(expectedResult);

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    #endregion

    #region Valid Request Scenario Tests

    [Test]
    public async Task InvokeAsync_WithValidRequest_ShouldCallNext()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "John", Email = "john@example.com", Age = 30 };
        var validationResult = new ValidationResult();
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        var cancellationTokenSource = new CancellationTokenSource();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(cancellationTokenSource.Token);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(testRequest, cancellationTokenSource.Token))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        var nextCalled = false;
        var expectedResult = Results.Ok("Success");
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx)
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(expectedResult);
        }

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(nextCalled, Is.True);
        Assert.That(result, Is.EqualTo(expectedResult));
        mockValidator.Verify(v => v.ValidateAsync(testRequest, cancellationTokenSource.Token), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithValidRequest_ShouldReturnNextResult()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "Jane", Email = "jane@example.com", Age = 25 };
        var validationResult = new ValidationResult();
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        var expectedData = new { Id = 123, Message = "Created" };
        var expectedResult = Results.Created("/api/resource/123", expectedData);
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(expectedResult);

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    #endregion

    #region Invalid Request Scenario Tests

    [Test]
    public async Task InvokeAsync_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "", Email = "invalid-email", Age = -5 };
        
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required") { ErrorCode = "NotEmpty" }
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        var nextCalled = false;
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx)
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(Results.Ok());
        }

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(nextCalled, Is.False);
        Assert.That(result, Is.InstanceOf<IResult>());
        
        var httpResult = result as IResult;
        Assert.That(httpResult, Is.Not.Null);
    }

    [Test]
    public async Task InvokeAsync_WithInvalidRequest_ShouldReturnErrorWithDetails()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "", Email = "invalid-email", Age = -5 };
        
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required") { ErrorCode = "NotEmpty" },
            new ValidationFailure("Email", "Invalid email format") { ErrorCode = "EmailValidator" }
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(result, Is.InstanceOf<IResult>());
    }

    [Test]
    public async Task InvokeAsync_WithInvalidRequest_ShouldNotCallNext()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "", Email = "", Age = 0 };
        
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required") { ErrorCode = "NotEmpty" }
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        var nextCalled = false;
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx)
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(Results.Ok());
        }

        // Act
        await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(nextCalled, Is.False);
    }

    #endregion

    #region Multiple Validation Errors Tests

    [Test]
    public async Task InvokeAsync_WithMultipleErrors_ShouldGroupByField()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "", Email = "bad", Age = -1 };
        
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required") { ErrorCode = "NotEmpty" },
            new ValidationFailure("Name", "Name must be at least 2 characters") { ErrorCode = "MinimumLength" },
            new ValidationFailure("Email", "Invalid email format") { ErrorCode = "EmailValidator" },
            new ValidationFailure("Age", "Age must be positive") { ErrorCode = "GreaterThanValidator" }
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(result, Is.InstanceOf<IResult>());
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task InvokeAsync_WithMultipleFieldErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "A", Email = "not-an-email", Age = 200 };
        
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name too short") { ErrorCode = "MinimumLength" },
            new ValidationFailure("Email", "Email is invalid") { ErrorCode = "EmailValidator" },
            new ValidationFailure("Age", "Age is too high") { ErrorCode = "LessThanValidator" }
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        var result = await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IResult>());
    }

    #endregion

    #region Cancellation Token Tests

    [Test]
    public async Task InvokeAsync_ShouldPassCancellationToken()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "Test", Email = "test@example.com", Age = 25 };
        var validationResult = new ValidationResult();
        
        var cancellationTokenSource = new CancellationTokenSource();
        var expectedToken = cancellationTokenSource.Token;
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(expectedToken);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        CancellationToken capturedToken = default;
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .Callback<TestRequest, CancellationToken>((_, token) => capturedToken = token)
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(capturedToken, Is.EqualTo(expectedToken));
        mockValidator.Verify(v => v.ValidateAsync(testRequest, expectedToken), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithCancelledToken_ShouldPassCancellationToValidator()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "Test", Email = "test@example.com", Age = 25 };
        
        var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        var cancelledToken = cancellationTokenSource.Token;
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(cancelledToken);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { testRequest });
        
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act & Assert
        Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await filter.InvokeAsync(mockContext.Object, Next);
        });
    }

    #endregion

    #region Request Extraction Tests

    [Test]
    public async Task InvokeAsync_ShouldExtractRequestFromArguments()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var testRequest = new TestRequest { Name = "John", Email = "john@example.com", Age = 30 };
        var validationResult = new ValidationResult();
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> 
        { 
            "some string",  // Other arguments
            42,             // Other arguments
            testRequest     // The request we want
        });
        
        TestRequest? capturedRequest = null;
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .Callback<TestRequest, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(capturedRequest, Is.Not.Null);
        Assert.That(capturedRequest, Is.EqualTo(testRequest));
        Assert.That(capturedRequest!.Name, Is.EqualTo("John"));
        Assert.That(capturedRequest.Email, Is.EqualTo("john@example.com"));
        Assert.That(capturedRequest.Age, Is.EqualTo(30));
    }

    [Test]
    public async Task InvokeAsync_WithMultipleRequestsOfSameType_ShouldExtractFirst()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestRequest>>();
        var firstRequest = new TestRequest { Name = "First", Email = "first@example.com", Age = 25 };
        var secondRequest = new TestRequest { Name = "Second", Email = "second@example.com", Age = 30 };
        var validationResult = new ValidationResult();
        
        var mockContext = new Mock<EndpointFilterInvocationContext>();
        var mockHttpContext = new Mock<HttpContext>();
        
        mockHttpContext.Setup(c => c.RequestAborted).Returns(CancellationToken.None);
        mockContext.Setup(c => c.HttpContext).Returns(mockHttpContext.Object);
        mockContext.Setup(c => c.Arguments).Returns(new List<object?> { firstRequest, secondRequest });
        
        TestRequest? capturedRequest = null;
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .Callback<TestRequest, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(validationResult);
        
        var filter = new RequestValidationFilter<TestRequest>(mockValidator.Object);
        
        ValueTask<object?> Next(EndpointFilterInvocationContext ctx) => ValueTask.FromResult<object?>(Results.Ok());

        // Act
        await filter.InvokeAsync(mockContext.Object, Next);

        // Assert
        Assert.That(capturedRequest, Is.EqualTo(firstRequest));
        Assert.That(capturedRequest!.Name, Is.EqualTo("First"));
    }

    #endregion
}

