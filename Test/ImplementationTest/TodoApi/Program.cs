using Frametux.Shared.Core;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TodoApi.Domain.UserAggregate.Services;
using TodoApi.Driven.DomainDb;
using TodoApi.Driven.DomainDb.Settings;
using TodoApi.Driving.Users.RegisterUser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DomainDbSettings>(
    builder.Configuration.GetSection(nameof(DomainDbSettings)));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DomainDbContext>(opt 
    => opt.UseNpgsql(builder.Configuration[$"{nameof(DomainDbSettings)}:{nameof(DomainDbSettings.ConnectionStr)}"]));

builder.Services.AddSharedCore(typeof(Program).Assembly);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RegisterUserService, RegisterUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

RegisterUserService.RegisterRestfulApi(app);

app.UseHttpsRedirection();

app.Run();