using Authentication.Contexts;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Authentication.Repositories;
using Authentication.Services;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IStatusService, StatusService>();



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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();