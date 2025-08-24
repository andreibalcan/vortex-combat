using VortexCombat.Application.DTOs;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.Mappings;

public static class MappingExtensions
{
    // RegisterDTO -> ApplicationUser
    public static ApplicationUser ToApplicationUser(this RegisterDTO dto) =>
        new()
        {
            UserName = dto.Email,
            Email = dto.Email,
            Name = dto.Name,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Nif = dto.Nif,
            EGender = dto.EGender,
            Birthday = dto.Birthday,
            Belt = dto.Belt,
            Height = dto.Height,
            Weight = dto.Weight
        };

    // RegisterDTO -> Student
    public static Student ToStudent(this RegisterDTO dto, string applicationUserId) =>
        new()
        {
            ApplicationUserId = applicationUserId,
            EnrollDate = dto.EnrollDate ?? DateTime.UtcNow
            // Removed HasTrainerCertificate because it doesn't exist in Student entity
        };

    // ScheduleWorkoutDTO -> Workout
    public static Workout ToWorkout(this ScheduleWorkoutDTO dto, List<Exercise> exercises) =>
        new()
        {
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Room = dto.Room,
            WorkoutExercises = exercises.Select(e => new WorkoutExercise { Exercise = e }).ToList()
        };

    // Workout -> WorkoutDTO
    public static WorkoutDTO ToDto(this Workout workout) =>
        new()
        {
            Id = workout.Id,
            Description = workout.Description,
            StartDate = workout.StartDate,
            EndDate = workout.EndDate,
            Room = workout.Room,
            Students = workout.WorkoutStudents.Select(ws => ws.Student).ToList(),
            Masters = workout.WorkoutMasters.Select(wm => wm.Master).ToList(),
            Exercises = workout.WorkoutExercises.Select(we => we.Exercise).ToList()
        };

    // Student -> StudentProgressDTO
    public static StudentProgressDTO ToProgressDto(this Student student, Belt nextBelt,
        List<Exercise> completed, List<Exercise> remaining, List<Workout> attended) =>
        new()
        {
            Id = student.Id,
            ApplicationUserId = student.ApplicationUserId,
            Name = student.ApplicationUser.Name,
            EGender = student.ApplicationUser.EGender,
            Birthday = student.ApplicationUser.Birthday,
            EnrollDate = student.EnrollDate,
            Height = student.ApplicationUser.Height,
            Weight = student.ApplicationUser.Weight,
            CurrentBelt = student.ApplicationUser.Belt,
            NextBelt = nextBelt,
            ProgressPercentage = (completed.Count + remaining.Count) == 0
                ? 0
                : (double)completed.Count / (completed.Count + remaining.Count) * 100,
            CompletedExercises = completed.Select(e => new SimplifiedExerciseDTO { Id = e.Id, Name = e.Name }).ToList(),
            RemainingExercises = remaining.Select(e => new SimplifiedExerciseDTO { Id = e.Id, Name = e.Name }).ToList(),
            AttendedWorkouts = attended.Select(w => w.ToDto()).ToList()
        };
}
