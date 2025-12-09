# EF Core Value Object Conversion Generator Prompt

Generate an EF Core Value Object Conversion class for {ValueObjectName} with underlying primitive type {PrimitiveType}.

## Requirements

- Static class named `{ValueObjectName}Conversion`
- Extension method `Has{ValueObjectName}Conversion` on `PropertyBuilder<{ValueObjectName}>`
- Extension method `Has{ValueObjectName}NullableConversion` on `PropertyBuilder<{ValueObjectName}?>`
- Value Object has a `.Value` property returning the primitive type
- Value Object constructor accepts the primitive type

## Template

```csharp
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class {ValueObjectName}Conversion
{
    public static PropertyBuilder<{ValueObjectName}> Has{ValueObjectName}Conversion(this PropertyBuilder<{ValueObjectName}> builder)
    {
        return builder.HasConversion<{PrimitiveType}>(
            i => i.Value,
            i => new {ValueObjectName}(i)
        );
    }
    
    public static PropertyBuilder<{ValueObjectName}?> Has{ValueObjectName}NullableConversion(this PropertyBuilder<{ValueObjectName}?> builder)
    {
        return builder.HasConversion<{PrimitiveType}?>(
            i => i != null ? i.Value : null,
            i => i != null ? new {ValueObjectName}(i.Value) : null
        );
    }
}
```

Add appropriate namespace and using statements based on the project structure.

## Usage Examples

- "Generate an EF Core Value Object Conversion class for `Email` with underlying primitive type `string`"
- "Generate an EF Core Value Object Conversion class for `Amount` with underlying primitive type `decimal`"
- "Generate an EF Core Value Object Conversion class for `UserId` with underlying primitive type `Guid`"

