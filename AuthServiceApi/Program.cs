using Application.Interfaces.RoleInterfaces;
using Application.Interfaces.UserInterfaces;
using Application.UseCase.RoleUseCase;
using Application.UseCase.UserUseCase;
using Infrastructure.Commands.UserCommand;
using Infrastructure.Persistence;
using Infrastructure.Querys.RoleQuery;
using Infrastructure.Querys.UserQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// load key from config (appsettings.json)
var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection["Secret"]; // put in secret manager / env var en prod
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Autentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserCommand, UserCommand>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleQuery, RoleQuery>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
