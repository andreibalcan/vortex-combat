using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs.Student;

public class SimplifiedStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Belt Belt { get; set; }
}