namespace VortexCombat.Application.DTOs.Workout;

public class ScheduleWorkoutDto
{
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Room { get; set; }
    public List<int> Exercises { get; set; } = new();
}