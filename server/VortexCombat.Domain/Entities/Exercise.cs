namespace VortexCombat.Domain.Entities;

public class Exercise
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public EExerciseDifficulty Difficulty { get; set; }

    public Belt Grade { get; set; }

    public Belt BeltLevelMin { get; set; }
    
    public Belt BeltLevelMax { get; set; }

    public string Duration { get; set; }

    public double MinYearsOfTraining { get; set; }
    
    public List<WorkoutExercise> WorkoutExercises { get; set; } = new();
}