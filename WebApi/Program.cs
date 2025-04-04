using Authentication.Contexts;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Repositories;
using Authentication.Services;
using Data.Contexts;
using Data.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Interfaces;
using Business.Services;
using WebApi.Extensions.Middlewares;
using Authentication.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AlphaDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDb")));
builder.Services.AddDbContext<UserDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaUserDb")));
builder.Services.AddIdentity<AppUserEntity, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

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

builder.Services.AddCors(x =>
{
    //Strict beh�vs f�r VG
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
builder.Services.AddSwaggerGen();
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
        x.SaveToken = true;
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
            ValidateAudience = false,
        };
    });

var app = builder.Build();

app.UseCors("AllowAll");
app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API");
    x.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseMiddleware<DefaultAPiKeyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();