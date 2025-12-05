using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frametux.Shared.Core.Driven.Persistence.EntityConfigs.Conversions;

public static class UtcDateTimeConversion
{
    public static PropertyBuilder<UtcDateTime> HasUtcDateTimeConversion(this PropertyBuilder<UtcDateTime> builder)
    {
        return builder.HasConversion(
            i => i.Value,
            i => new UtcDateTime(i)
        );
    }
    
    public static PropertyBuilder<UtcDateTime?> HasUtcDateTimeNullableConversion(this PropertyBuilder<UtcDateTime?> builder)
    {
        return builder.HasConversion<DateTime?>(
            i => i != null ? i.Value : null,
            i => i != null ? new UtcDateTime(i.Value) : null
        );
    }
}

