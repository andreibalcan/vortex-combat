using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Domain.Entities;

namespace  VortexCombat.Infrastructure.Services;

public class SeedDataService
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<string> { "PrimaryMaster", "Master", "Student" };

        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var defaultUser = await userManager.FindByEmailAsync("admin@vortexcombat.com");
        if (defaultUser == null)
        {
            var primaryMaster = new ApplicationUser
            {
                UserName = "admin@vortexcombat.com",
                Address = new Address
                {
                    Street = "Main Street",
                    Number = "123",
                    Floor = "2A",
                    City="Lisbon",
                    ZipCode = "12345"
                },
                Nif = "123456789",
                Email = "admin@vortexcombat.com",
                Name = "Rick Johnston",
                Belt = new Belt { Color = EBeltColor.Black, Degrees = 3 },
                EGender = EGender.M,
                Birthday = new DateTime(1985, 5, 12),
                Height = 183,
                Weight = 80,
            };

            var defaultPassword = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_PASSWORD");

            var result = await userManager.CreateAsync(primaryMaster, defaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(primaryMaster, "PrimaryMaster");
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

                var master = new Master
                {
                    ApplicationUserId = primaryMaster.Id,
                    HasTrainerCertificate = true
                };

                context.Masters.Add(master);
                await context.SaveChangesAsync();
            }
        }
    }
}