using System.IO;
using IdentityServer.Data;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(connectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();
var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("CacheSettings:ConnectionString"));
builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys-IdentityServer")
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });


builder.Services.AddIdentityServer()
     .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));
        })
    .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));
        })
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
SeedIdentityData.InitializeDatabase(app);
app.UseIdentityServer();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
