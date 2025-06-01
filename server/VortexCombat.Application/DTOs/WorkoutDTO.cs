using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs;

public class WorkoutDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Room { get; set; }
    
    public List<Student> Students { get; set; }
    public List<Master> Masters { get; set; }
}