using FluentValidation;
using FluentValidation.AspNetCore;
using Hozify.API.Middleware;
using Hozify.Application.Common.Settings;
using Hozify.Application.Features.Auth.Interfaces;
using Hozify.Application.Features.Categories.Interfaces;
using Hozify.Application.Features.Categories.Mappings;
using Hozify.Application.Features.Categories.Validators;
using Hozify.Application.Features.Partners.Interfaces;
using Hozify.Application.Features.Services.Interfaces;
using Hozify.Domain.Constants;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.BackgroundServices;
using Hozify.Infrastructure.Data;
using Hozify.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Database
// =============================
builder.Services.AddDbContext<HozifyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =============================
// JWT Settings
// =============================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

// =============================
// Dependency Injection
// =============================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IBackgroundCleanupService, BackgroundCleanupService>();

builder.Services.AddHostedService<CleanupHostedService>();

// =============================
// AutoMapper
// =============================
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);

// =============================
// Authentication
// =============================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// =============================
// Controllers
// =============================
builder.Services.AddControllers();

// =============================
// Fluent Validation
// =============================
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

// =============================
// Custom Validation Response
// =============================
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => char.ToLowerInvariant(x.Key[0]) + x.Key.Substring(1),
                x => x.Value!.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray());

        var response = new ApiResponse<object>
        {
            StatusCode = (int)ApiStatusCode.BadRequest,
            Success = false,
            Message = ApiMessages.ValidationFailed,
            Data = null,
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});

// =============================
// Swagger
// =============================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// =============================
// Middleware Pipeline
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
