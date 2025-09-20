using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Infrastructure.Identity;

namespace VortexCombat.Infrastructure.Services;

public class SeedDataService
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var logger = serviceProvider.GetService<ILogger<SeedDataService>>();

        // Existing user/role seeding code...
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
            var domainUser = new User
            {
                Id = new UserId(Guid.NewGuid()),
                Address = new Address
                {
                    Street = "Main Street",
                    Number = "123",
                    Floor = "2A",
                    City = "Lisbon",
                    ZipCode = "12345"
                },
                Nif = "123456789",
                Name = "Rick Johnston",
                Belt = new Belt { Color = EBeltColor.Black, Degrees = 3 },
                EGender = EGender.M,
                Birthday = new DateTime(1985, 5, 12),
                Height = 183,
                Weight = 80,
            };

            var primaryMaster = new ApplicationUser
            {
                UserName = "admin@vortexcombat.com",
                Email = "admin@vortexcombat.com",
                DomainUserId = domainUser.Id.Value,
            };

            var defaultPassword = Environment.GetEnvironmentVariable("DEFAULT_ADMIN_PASSWORD");
            var result = await userManager.CreateAsync(primaryMaster, defaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(primaryMaster, "PrimaryMaster");
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                context.Users.Add(domainUser);
                await context.SaveChangesAsync();
                var master = new Master
                {
                    UserId = domainUser.Id,
                    HasTrainerCertificate = true
                };
                context.Masters.Add(master);
                await context.SaveChangesAsync();
            }
        }

        // Exercise seeding with improved path resolution
        await SeedExercises(serviceProvider, logger);
    }

    private static async Task SeedExercises(IServiceProvider serviceProvider, ILogger<SeedDataService>? logger)
    {
        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if exercises already exist
            if (context.Exercises.Any())
            {
                logger?.LogInformation("[Seed] Exercises already exist, skipping seed.");
                return;
            }

            // Try multiple possible paths for the JSON file
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "SeedData", "exercises.json"),
                Path.Combine(AppContext.BaseDirectory, "SeedData", "exercises.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "..", "VortexCombat.Shared", "SeedData",
                    "exercises.json"),
                Path.Combine(AppContext.BaseDirectory, "..", "VortexCombat.Shared", "SeedData", "exercises.json")
            };

            string? jsonFile = null;
            foreach (var path in possiblePaths)
            {
                logger?.LogInformation($"[Seed] Checking path: {path}");
                if (File.Exists(path))
                {
                    jsonFile = path;
                    logger?.LogInformation($"[Seed] Found exercises.json at: {path}");
                    break;
                }
            }

            if (jsonFile == null)
            {
                logger?.LogWarning("[Seed] exercises.json not found in any expected location, skipping exercise seed.");
                logger?.LogInformation($"[Seed] Current directory: {Directory.GetCurrentDirectory()}");
                logger?.LogInformation($"[Seed] Base directory: {AppContext.BaseDirectory}");
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFile);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var exercises = JsonSerializer.Deserialize<List<Exercise>>(json, options);

            if (exercises == null || exercises.Count == 0)
            {
                logger?.LogWarning("[Seed] No exercises found in JSON, skipping.");
                return;
            }

            // Add exercises to context
            await context.Exercises.AddRangeAsync(exercises);
            await context.SaveChangesAsync();

            logger?.LogInformation($"[Seed] Successfully seeded {exercises.Count} exercises.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "[Seed] Exercise seeding failed");
        }
    }
}