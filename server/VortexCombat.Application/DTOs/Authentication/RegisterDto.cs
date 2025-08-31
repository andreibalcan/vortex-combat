using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs.Authentication;

public class RegisterDto
{
    public string Name { get; set; }
    public Address? Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Nif { get; set; }
    public EGender EGender { get; set; }
    public DateTime Birthday { get; set; }
    public Belt Belt { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? EnrollDate { get; set; }
    public bool? HasTrainerCertificate { get; set; }
}