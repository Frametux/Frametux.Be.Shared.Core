using Frametux.Shared.Core.Domain.Exceptions;

namespace TodoApi.Domain.UserAggregate.Exceptions;

public class DuplicatedUserEmailExc() 
    : Exception("Email already taken."), IExcHasErrorCode
{
    public string Code => "DuplicatedUserEmailExc";
}