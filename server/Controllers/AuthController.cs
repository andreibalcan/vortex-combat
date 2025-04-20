using server.DTOs;
using server.Models;
using server.Services;

namespace server.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtService _jwtService;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
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
            Gender = Enum.Parse<Gender>(model.Gender, ignoreCase: true),
            Birthday = model.Birthday,
            PhoneNumber = model.PhoneNumber,
            Belt = model.Belt,
            Height = model.Height,
            Weight = model.Weight
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
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