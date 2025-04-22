using server.Models;

namespace server.DTOs;

public class RegisterDTO
{
    public string Name { get; set; }
    public Address? Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Nif { get; set; }
    public Gender Gender { get; set; }
    public DateTime Birthday { get; set; }
    public Belt Belt { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? EnrollDate { get; set; }
    public bool? HasTrainerCertificate { get; set; }
}