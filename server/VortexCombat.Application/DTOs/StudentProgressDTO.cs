using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs;

public class StudentProgressDTO
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }

    public string Name { get; set; }
    public EGender EGender { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime EnrollDate { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }

    public Belt CurrentBelt { get; set; }
    public Belt NextBelt { get; set; }

    public double ProgressPercentage { get; set; }

    public List<SimplifiedExerciseDTO> CompletedExercises { get; set; } = new();

    public List<SimplifiedExerciseDTO> RemainingExercises { get; set; } = new();

    public List<WorkoutDTO> AttendedWorkouts { get; set; } = new();
}

public class SimplifiedExerciseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}