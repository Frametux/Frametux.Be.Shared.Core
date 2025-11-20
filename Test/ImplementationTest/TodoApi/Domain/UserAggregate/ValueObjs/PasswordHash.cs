using System.Security.Cryptography;
using FluentValidation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TodoApi.Domain.UserAggregate.ValueObjs;

public record PasswordHash
{
    #region Hash

    public string Hash { get; private init; }
    
    const int HashByteLength = 32; // Hash byte length = 256 / 8 = 32 bytes
    public static readonly int HashMaxLength = CalculateBase64Length(HashByteLength) + 10; // Add 10 for some extra space just in case
    
    public static InlineValidator<string> HashValidator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MaximumLength(HashMaxLength)
    };    
    
    #endregion

    #region Salt

    public string SaltStr { get; private init; }
    
    const int SaltBytesLength = 16; // Salt byte length = 128 / 8 = 16 bytes
    public static readonly int SaltStrMaxLength = CalculateBase64Length(SaltBytesLength) + 10; // Add 10 for some extra space just in case

    public static InlineValidator<string> SaltStrValidator { get; } = new()
    {
        v => v.RuleFor(s => s)
            .NotEmpty()
            .MaximumLength(SaltStrMaxLength)
    };
    
    #endregion

    public PasswordHash(Password password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(SaltBytesLength);
        var hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: HashByteLength
            )
        );
        
        // No validation needed here, since the value is generated internally.
        
        SaltStr = Convert.ToBase64String(saltBytes);
        Hash = hash;
    }

    public PasswordHash(string hash, string saltStr)
    {
        HashValidator.ValidateAndThrow(hash);
        SaltStrValidator.ValidateAndThrow(saltStr);
        
        Hash = hash;
        SaltStr = saltStr;
    }
    
    private static int CalculateBase64Length(int byteLength)
    {
        // In C#, the integer division equivalent for ceiling(a/b) is (a + b - 1) / b 
        // when a and b are positive integers.
        // We use Math.Ceiling here for clarity, but the integer math is slightly more efficient.

        // Standard formula: 4 * ceiling(byteLength / 3)
        var predictedLength = (int)Math.Ceiling(byteLength / 3.0) * 4;
        return predictedLength;
    }
}
