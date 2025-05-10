using VortexCombat.Application.DTOs;
using VortexCombat.Domain.Entities;
using VortexCombat.Infrastructure.Services;

namespace server.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtService _jwtService;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        JwtService jwtService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
            Address = model.Address,
            Nif = model.Nif,
            EGender = model.EGender,
            Birthday = model.Birthday,
            PhoneNumber = model.PhoneNumber,
            Belt = model.Belt,
            Height = model.Height,
            Weight = model.Weight
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var roleExist = await _roleManager.RoleExistsAsync("Student");

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Student"));
                if (!roleResult.Succeeded)
                {
                    return BadRequest("Failed to create 'Student' role");
                }
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return Ok(new { message = "User registered successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return BadRequest(new { message = "Invalid email or password." });
        }

        var token = await _jwtService.GenerateToken(user, _userManager);

        return Ok(new
        {
            token,
            expiration = DateTime.UtcNow.AddMinutes(30)
        });
    }
}