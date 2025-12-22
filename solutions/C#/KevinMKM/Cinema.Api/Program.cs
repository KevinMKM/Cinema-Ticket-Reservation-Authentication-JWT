using Cinema.Application.Dtos;
using Cinema.Application.Interfaces;
using Cinema.Application.Services;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories;
using Cinema.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cinema.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);

// ✅ JWT Configuration from appsettings.json
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Database
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IJwtService, JwtService>();

// UseCases
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();

// Controllers
builder.Services.AddControllers();

// ✅ JWT Authentication with FULL validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ✅ SCOPE-BASED AUTHORIZATION - Custom Policy Handler
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireScope:ticket:reserve",
        policy => policy.Requirements.Add(new RequireScopeRequirement("ticket:reserve")));

    options.AddPolicy("RequireScope:screening:create",
        policy => policy.Requirements.Add(new RequireScopeRequirement("screening:create")));
});

// ✅ Custom Authorization Handler for Scope
builder.Services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await DatabaseSeed(app);
app.Run();

static async Task DatabaseSeed(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    await context.Database.EnsureCreatedAsync();
}
