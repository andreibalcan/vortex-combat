using Microsoft.AspNetCore.Identity;

namespace server.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }

    public Address? Address { get; set; }

    public string Nif { get; set; }

    public Gender Gender { get; set; }

    public DateTime Birthday { get; set; }

    public Belt Belt { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }
}