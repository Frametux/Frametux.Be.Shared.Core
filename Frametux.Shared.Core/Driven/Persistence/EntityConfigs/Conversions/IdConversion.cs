using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frametux.Shared.Core.Driven.Persistence.EntityConfigs.Conversions;

public static class IdConversion
{
    public static PropertyBuilder<Id> HasIdConversion(this PropertyBuilder<Id> builder)
    {
        return builder.HasConversion<string>(
            i => i.Value,
            i => new Id(i)
        );
    }
    
    public static PropertyBuilder<Id?> HasIdNullableConversion(this PropertyBuilder<Id?> builder)
    {
        return builder.HasConversion<string?>(
            i => i != null ? i.Value : null,
            i => i != null ? new Id(i) : null
        );
    }
}