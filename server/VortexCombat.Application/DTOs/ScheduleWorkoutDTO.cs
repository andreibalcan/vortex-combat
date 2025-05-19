namespace VortexCombat.Application.DTOs;

public class ScheduleWorkoutDTO
{
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Room { get; set; }
}