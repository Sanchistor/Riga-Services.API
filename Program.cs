using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MediatR;
using riga.services.DbContext;
using riga.services.riga.services.authentication.IRepositories;
using riga.services.riga.services.authentication.Repositories;
using riga.services.riga.services.authentication.Services;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Repositories;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to use the Bearer token
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApiDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<AuthGuard>();
builder.Services.AddScoped<ICardDataRepository, CardDataRepository>();
builder.Services.AddScoped<IUpdateBalanceRepository, UpdateBalanceRepository>();

// JWT Authentication setup
var key = "Yh2k7QSu418CZg5p6X3Pna9L0Miy4D3Bvt0JVr87Uc0j69Kqw5R2Nmf4FWs03Hdx";
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton<JwtAuthenticationManager>(new JwtAuthenticationManager(key, builder.Services.BuildServiceProvider().GetService<ApiDbContext>()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// Ensure routing is called before authentication and authorization
app.UseRouting(); // Move this line to the correct position

app.UseAuthentication(); // Authenticate requests before authorization
app.UseAuthorization(); // Then authorize requests

app.MapControllers();

app.Run("http://0.0.0.0:5000");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Application started and listening on port 5000.");
});
