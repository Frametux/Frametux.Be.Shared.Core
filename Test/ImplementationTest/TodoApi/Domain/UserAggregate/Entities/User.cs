using Frametux.Shared.Core.Domain.Entities;
using Frametux.Shared.Core.Domain.ValueObjs;
using TodoApi.Domain.UserAggregate.ValueObjs;

namespace TodoApi.Domain.UserAggregate.Entities;

public class User : BaseEntity
{
    public required Email Email { get; set; }
    public required PasswordHash PasswordHash { get; set; }
}