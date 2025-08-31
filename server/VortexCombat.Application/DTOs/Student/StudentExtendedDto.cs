using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs.Student;

public class ExtendedStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address? Address { get; set; }
    public EGender Gender { get; set; }
    public DateTime Birthday { get; set; }
    public Belt Belt { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public DateTime EnrollDate { get; set; }
}