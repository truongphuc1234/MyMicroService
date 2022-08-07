using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using User.API.Entities;

namespace User.API.Data;

public class SeedData
{
    public static async Task SeedAsync(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (context == null)
            {
                return;
            }

            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var hasUser = userMgr.Users.AnyAsync(x => true).Result;
            if (!hasUser)
            {
                var users = new List<ApplicationUser>{new ApplicationUser
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    PhoneNumber = "09011223334"

                },
                new ApplicationUser {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    PhoneNumber = "0905169944"
                }};


                foreach (var user in users)
                {
                   await userMgr.CreateAsync(user, "Pa$$w0rd123");
                }
            }
        }
    }
}