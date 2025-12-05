using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frametux.Shared.Core.Driven.Persistence.EntityConfigs.Conversions;

public static class CreatedAtConversion
{
    public static PropertyBuilder<CreatedAt> HasCreatedAtConversion(this PropertyBuilder<CreatedAt> builder)
    {
        return builder.HasConversion(
            i => i.Value,
            i => new CreatedAt(i)
        );
    }
    
    public static PropertyBuilder<CreatedAt?> HasCreatedAtNullableConversion(this PropertyBuilder<CreatedAt?> builder)
    {
        return builder.HasConversion<DateTime?>(
            i => i != null ? i.Value : null,
            i => i != null ? new CreatedAt(i.Value) : null
        );
    }
}