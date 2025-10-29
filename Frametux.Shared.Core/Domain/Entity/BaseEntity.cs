using Frametux.Shared.Core.Domain.ValueObj;

namespace Frametux.Shared.Core.Domain.Entity;

public abstract class BaseEntity
{
    public Id Id { get; init; } = new();
    
    public CreatedAt CreatedAt { get; init; } = new();
}