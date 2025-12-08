namespace Frametux.Shared.Core.Domain.ValueObjs.Base;

public interface ISinglePropValueObj<out TValue>
{
    public TValue Value { get; }
}