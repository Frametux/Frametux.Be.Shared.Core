using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.UserAggregate.Entities;

namespace TodoApi.Driven.DomainDb;

public class DomainDbContext(DbContextOptions<DomainDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}