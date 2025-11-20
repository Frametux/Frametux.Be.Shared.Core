using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.UserAggregate.Entities;
using TodoApi.Domain.UserAggregate.Exceptions;
using TodoApi.Domain.UserAggregate.ValueObjs;
using TodoApi.Driven.DomainDb;

namespace TodoApi.Domain.UserAggregate.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(Email email, Password password, CancellationToken cancellationToken);
}

public class UserService(DomainDbContext dbContext) : IUserService
{
    public async Task<User> CreateUserAsync(Email email, Password password, CancellationToken cancellationToken)
    {
        var duplicatedByEmailUser = await dbContext.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);

        if (duplicatedByEmailUser is not null)
            throw new DuplicatedUserEmailExc();
        
        var user = new User
        {
            Email = email,
            PasswordHash = password.Hash()
        };
        
        dbContext.Users.Add(user);

        return user;
    }
}