# Value Object Generator Prompt Template

Use this prompt template with Cursor AI to generate value objects that follow the established patterns in this codebase.

---

## Generic Prompt Template

```
Create a C# value object named {NAME} with the following specifications:

**Basic Information:**
- Value object name: {NAME}
- Underlying type: {TYPE} (e.g., string, DateTime, int, decimal)
- Namespace: {NAMESPACE} (e.g., Frametux.Shared.Core.Domain.ValueObjs or [YourProject].Domain.[Aggregate].ValueObjs)

**Validation Rules:**
{VALIDATION_RULES}
(Examples:
- NotEmpty
- MinimumLength: {MIN_LENGTH}
- MaximumLength: {MAX_LENGTH}
- Must be a valid email address
- Must be greater than zero
- Must be in UTC
- Custom validation: {CUSTOM_RULE})

**Optional Features:**
- [ ] Include parameterless constructor with default value: {DEFAULT_VALUE_LOGIC}
- [ ] Include value normalization before validation: {NORMALIZATION_LOGIC}
- [ ] Include domain-specific methods: {METHOD_SIGNATURES}
- [ ] Include multiple properties (for complex value objects): {PROPERTY_DEFINITIONS}

**Pattern Requirements:**
Follow the established value object pattern in this codebase:
1. Use C# record type
2. Include public property named "Value" (or domain-specific property names)
3. Define validation constants as public const (e.g., MaxLength, MinLength)
4. Create static InlineValidator<T> property with FluentValidation rules
5. Implement primary constructor that:
   - Applies any normalization if specified
   - Validates using Validator.ValidateAndThrow()
   - Assigns to Value property
6. Add implicit conversion operators to/from the underlying type
7. Use appropriate using statements (FluentValidation is required)

**Additional Notes:**
{ADDITIONAL_NOTES}
```

---

## Example Usage

### Example 1: Simple String Value Object

```
Create a C# value object named Username with the following specifications:

**Basic Information:**
- Value object name: Username
- Underlying type: string
- Namespace: Frametux.Shared.Core.Domain.ValueObjs

**Validation Rules:**
- NotEmpty
- MinimumLength: 3
- MaximumLength: 50
- Must match pattern: ^[a-zA-Z0-9_-]+$ (alphanumeric, underscores, and hyphens only)

**Optional Features:**
- [x] Include value normalization before validation: ToLowerInvariant()
- [ ] Include parameterless constructor with default value
- [ ] Include domain-specific methods
- [ ] Include multiple properties

**Pattern Requirements:**
Follow the established value object pattern in this codebase.

**Additional Notes:**
This username will be used for user authentication across the system.
```

### Example 2: Numeric Value Object with Default Constructor

```
Create a C# value object named Age with the following specifications:

**Basic Information:**
- Value object name: Age
- Underlying type: int
- Namespace: MyApp.Domain.UserAggregate.ValueObjs

**Validation Rules:**
- GreaterThanOrEqualTo: 0
- LessThanOrEqualTo: 150

**Optional Features:**
- [x] Include parameterless constructor with default value: 0
- [ ] Include value normalization before validation
- [ ] Include domain-specific methods
- [ ] Include multiple properties

**Pattern Requirements:**
Follow the established value object pattern in this codebase.

**Additional Notes:**
Age should never be negative and should be reasonable for a human.
```

### Example 3: DateTime Value Object with Normalization

```
Create a C# value object named UpdatedAt with the following specifications:

**Basic Information:**
- Value object name: UpdatedAt
- Underlying type: DateTime
- Namespace: Frametux.Shared.Core.Domain.ValueObjs

**Validation Rules:**
- Must be in UTC (DateTimeKind.Utc)
- LessThanOrEqualTo: DateTime.UtcNow (cannot be in the future)

**Optional Features:**
- [x] Include parameterless constructor with default value: DateTime.UtcNow
- [x] Include value normalization before validation: Convert to UTC using switch expression (handle Utc, Local, and Unspecified kinds)
- [ ] Include domain-specific methods
- [ ] Include multiple properties

**Pattern Requirements:**
Follow the established value object pattern in this codebase.

**Additional Notes:**
Similar to CreatedAt value object but for tracking update timestamps.
```

### Example 4: Complex Value Object with Multiple Properties

```
Create a C# value object named Address with the following specifications:

**Basic Information:**
- Value object name: Address
- Underlying type: Multiple properties
- Namespace: MyApp.Domain.CustomerAggregate.ValueObjs

**Validation Rules:**
- Street: NotEmpty, MaximumLength: 200
- City: NotEmpty, MaximumLength: 100
- PostalCode: NotEmpty, MaximumLength: 20
- Country: NotEmpty, MaximumLength: 100

**Optional Features:**
- [ ] Include parameterless constructor with default value
- [x] Include value normalization before validation: Trim all string values
- [ ] Include domain-specific methods
- [x] Include multiple properties:
  - public string Street { get; }
  - public string City { get; }
  - public string PostalCode { get; }
  - public string Country { get; }

**Pattern Requirements:**
Follow the established value object pattern in this codebase.
Note: For multiple properties, create separate validators for each property and validate them individually in the constructor.

**Additional Notes:**
This value object encapsulates a complete postal address. Do not include implicit operators for complex value objects with multiple properties.
```

### Example 5: Value Object with Domain Methods

```
Create a C# value object named PhoneNumber with the following specifications:

**Basic Information:**
- Value object name: PhoneNumber
- Underlying type: string
- Namespace: Frametux.Shared.Core.Domain.ValueObjs

**Validation Rules:**
- NotEmpty
- MaximumLength: 20
- Must match pattern: ^\+?[1-9]\d{1,14}$ (E.164 format)

**Optional Features:**
- [ ] Include parameterless constructor with default value
- [x] Include value normalization before validation: Remove spaces, dashes, and parentheses
- [x] Include domain-specific methods:
  - public string GetFormatted() => Format the phone number for display
  - public string GetCountryCode() => Extract country code from the number
- [ ] Include multiple properties

**Pattern Requirements:**
Follow the established value object pattern in this codebase.

**Additional Notes:**
Phone numbers should be stored in E.164 format but provide helper methods for display and parsing.
```

---

## Quick Reference: Common Validation Rules

### String Validations
- `NotEmpty()` - Cannot be null or empty
- `MinimumLength(n)` - Minimum character length
- `MaximumLength(n)` - Maximum character length
- `Length(min, max)` - Length range
- `EmailAddress()` - Valid email format
- `Matches("regex")` - Match regex pattern
- `Must(predicate)` - Custom validation logic

### Numeric Validations
- `GreaterThan(n)` - Strictly greater than
- `GreaterThanOrEqualTo(n)` - Greater than or equal
- `LessThan(n)` - Strictly less than
- `LessThanOrEqualTo(n)` - Less than or equal
- `InclusiveBetween(min, max)` - Value within range
- `ExclusiveBetween(min, max)` - Value within range (exclusive)

### DateTime Validations
- `LessThanOrEqualTo(_ => DateTime.UtcNow)` - Not in future
- `GreaterThanOrEqualTo(_ => DateTime.UtcNow)` - Not in past
- `Must(dt => dt.Kind == DateTimeKind.Utc)` - Must be UTC

### Custom Validations
- `Must(predicate).WithMessage("error message")` - Custom validation with error message
- `Must(predicate).WithErrorCode("ERROR_CODE")` - Custom validation with error code

---

## Pattern Checklist

When creating a value object, ensure it includes:

- [ ] `using FluentValidation;` statement
- [ ] Correct namespace declaration
- [ ] `public record {Name}` declaration
- [ ] Public property/properties with getter only
- [ ] Validation constants (if applicable): `public const int MaxLength = ...`
- [ ] Static validator: `public static InlineValidator<T> Validator { get; } = new() { ... }`
- [ ] Primary constructor with parameter matching underlying type
- [ ] Normalization logic in constructor (if needed)
- [ ] `Validator.ValidateAndThrow(value)` call in constructor
- [ ] Property assignment in constructor
- [ ] Parameterless constructor (if needed)
- [ ] Implicit operators (for single-value objects): `public static implicit operator {Type}({Name} obj) => obj.Value;`
- [ ] Implicit operators (for single-value objects): `public static implicit operator {Name}({Type} value) => new(value);`
- [ ] Domain methods (if needed)
- [ ] Proper formatting and code organization

---

## Notes

1. **Single Responsibility**: Each value object should represent one cohesive concept
2. **Immutability**: Value objects are immutable - use `{ get; }` or `{ get; private init; }`
3. **Validation**: Always validate in constructor using FluentValidation
4. **Implicit Operators**: Only include for simple single-value objects; skip for complex multi-property value objects
5. **Records**: Use C# record type for automatic structural equality
6. **Constants**: Define validation constants for reusability and testability
7. **Normalization**: Apply before validation to ensure consistent data
8. **UTC Dates**: Always work with UTC DateTime values for consistency

