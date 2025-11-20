namespace TodoApi.Driven.DomainDb.Settings;

public record DomainDbSettings
{
    public required string ConnectionStr { get; init; }
}