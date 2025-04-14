using Authentication.Contexts;
using Authentication.Entities;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Repositories;
using Authentication.Services;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Domain.Handlers;
using Domain.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AlphaDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDb")));
builder.Services.AddDbContext<UserDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaUserDb")));
builder.Services.AddIdentity<AppUserEntity, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();
builder.Services.AddAuthorization();

//DI stuff
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RoleHandler>();
builder.Services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

var azureConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
var azureContainerName = "images";
builder.Services.AddScoped<IFileHandler>(_ => new AzureFileHandler(azureConnectionString!, azureContainerName));


builder.Services.AddCors(x =>
{
    //Strict behövs för VG
    x.AddPolicy("Strict", x =>
    {
        x.WithOrigins("URL HERE").WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Content-Type", "Authorization").AllowCredentials();
    });

    x.AddPolicy("AllowAll", x =>
    {
        x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.EnableAnnotations();
    options.ExampleFilters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v. 1.0",
        Title = "Alpha API Documentation",
        Description = "This is the standard documentation for Alpha Portal.",
    });

    var jwtBearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtBearerScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtBearerScheme, new List<string>() }
    });

var apiAdminScheme = new OpenApiSecurityScheme
    {
        Name = "X-ADM-API-KEY",
        Description = "Admin Api-Key Required",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Id = "AdminApiKey",
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("AdminApiKey", apiAdminScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiAdminScheme, new List<string>() }
    });

});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!);
        var issuer = builder.Configuration["Jwt:Issuer"]!;
        var audience = builder.Configuration["Jwt:Audience"]!;

        //Change this for production
        x.RequireHttpsMetadata = false;
        //SaveToken är för identity.
        //x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            //Change this for production
            ValidateAudience = true,
            ValidAudience = audience
        };
    });

var app = builder.Build();

app.UseCors("AllowAll");
app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API v.1");
    x.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();