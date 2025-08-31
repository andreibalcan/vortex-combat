using VortexCombat.Application.DTOs.Master;
using VortexCombat.Application.DTOs.Student;

namespace VortexCombat.Application.DTOs.Workout;

public class WorkoutDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Room { get; set; }
    
    public List<SimplifiedStudentDto> Students { get; set; }
    public List<MasterSimplifiedDto> Masters { get; set; }
    public List<int> Exercises { get; set; }
}