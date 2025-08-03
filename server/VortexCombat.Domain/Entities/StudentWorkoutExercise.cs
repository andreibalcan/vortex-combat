namespace VortexCombat.Domain.Entities;

public class StudentWorkoutExercise
{
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
}