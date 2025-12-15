using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cinema.Application.Authorization;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

var key = Environment.GetEnvironmentVariable("JWT__SigningKey") ?? throw new Exception("JWT__SigningKey missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "cinema-auth",
            ValidAudience = "cinema-api",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("ReserveTicket", p => p.RequireClaim("scope", "tickets:reserve"));
    o.AddPolicy("CreateScreening", p => p.RequireRole("Admin")
        .RequireClaim("scope", "screenings:write"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();