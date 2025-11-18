namespace Frametux.Shared.Core.Domain.Exceptions;

public interface IExcHasErrorCode
{
    public string Code { get; }
    public string Message { get; }
}