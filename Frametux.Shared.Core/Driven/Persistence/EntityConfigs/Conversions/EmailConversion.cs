using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frametux.Shared.Core.Driven.Persistence.EntityConfigs.Conversions;

public static class EmailConversion
{
    public static PropertyBuilder<Email> HasEmailConversion(this PropertyBuilder<Email> builder)
    {
        return builder.HasConversion<string>(
            i => i.Value,
            i => new Email(i)
        );
    }
    
    public static PropertyBuilder<Email?> HasEmailNullableConversion(this PropertyBuilder<Email?> builder)
    {
        return builder.HasConversion<string?>(
            i => i != null ? i.Value : null,
            i => i != null ? new Email(i) : null
        );
    }
}

