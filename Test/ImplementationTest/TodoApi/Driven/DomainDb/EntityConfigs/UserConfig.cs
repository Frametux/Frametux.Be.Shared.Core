using Frametux.Shared.Core.Domain.ValueObjs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Domain.UserAggregate.Entities;
using TodoApi.Domain.UserAggregate.ValueObjs;

namespace TodoApi.Driven.DomainDb.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Id)
            .HasMaxLength(Id.MaxLength)
            .HasConversion(
                id => (string)id,
                id => new Id(id)
            );
        
        builder
            .Property(u => u.CreatedAt)
            .IsRequired()
            .HasConversion(
                createdAt => (DateTime)createdAt,
                createdAt => new CreatedAt(createdAt)
            );

        builder
            .Property(u => u.Email)
            .HasMaxLength(Email.MaxLength)
            .IsRequired()
            .HasConversion(
                email => (string)email,
                email => new Email(email)
            );

        // builder
        //     .Property(u => u.PasswordHash)
        //     .HasColumnType("jsonb")
        //     .IsRequired()
        //     .HasConversion(
        //         passwordHash => JsonSerializer.Serialize(passwordHash, new JsonSerializerOptions { WriteIndented = true }),
        //         passwordHash => JsonSerializer.Deserialize<PasswordHash>(passwordHash, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
        //     );
        
        // 1. Tell EF Core that 'User' owns the 'PasswordHash' object
        builder.OwnsOne(u => u.PasswordHash, passwordHashOwned =>
        {
            // 2. Specify the column names for the owned type's properties
            passwordHashOwned.Property(p => p.Hash)
                .HasColumnName("PasswordHash") // Maps Hash property to PasswordHash column
                .HasMaxLength(PasswordHash.HashMaxLength) // Good practice for hash length
                .IsRequired();
                
            passwordHashOwned.Property(p => p.SaltStr)
                .HasColumnName("PasswordSalt") // Maps Salt property to PasswordSalt column
                .HasMaxLength(PasswordHash.SaltStrMaxLength) // Good practice for salt length
                .IsRequired();
        });
    }
}