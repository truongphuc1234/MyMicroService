using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddOcelot();
builder.Services.AddAuthentication().AddJwtBearer("Bearer", o =>
{
    o.RequireHttpsMetadata = false;
    o.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
    o.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
});
var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();

app.Run();
