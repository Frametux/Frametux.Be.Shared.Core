using Frametux.Shared.Core.Domain.ValueObjs;

namespace Frametux.Shared.Core.Domain.Entities;

public abstract class BaseEntity
{
    public Id Id { get; protected init; } = new();
    
    public CreatedAt CreatedAt { get; protected init; } = new();
}