using VortexCombat.Application.DTOs.Authentication;
using VortexCombat.Application.DTOs.Master;
using VortexCombat.Application.DTOs.Student;
using VortexCombat.Application.DTOs.Workout;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.Mappings;

public static class MappingExtensions
{
    // RegisterDto -> ApplicationUser
    public static ApplicationUser ToApplicationUserDto(this RegisterDto dto) =>
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

    // RegisterDto -> Student
    public static Student ToStudentDto(this RegisterDto dto, string applicationUserId) =>
        new()
        {
            ApplicationUserId = applicationUserId,
            EnrollDate = dto.EnrollDate ?? DateTime.UtcNow
            // Removed HasTrainerCertificate because it doesn't exist in Student entity
        };

    // Workout -> WorkoutDto
    public static WorkoutDto ToWorkoutDto(this Workout workout) =>
        new()
        {
            Id = workout.Id,
            Description = workout.Description,
            StartDate = workout.StartDate,
            EndDate = workout.EndDate,
            Room = workout.Room,
            Students = workout.WorkoutStudents.Select(ws => new SimplifiedStudentDto
            {
                Id = ws.Student.Id,
                Name = ws.Student.ApplicationUser.Name,
                Belt = ws.Student.ApplicationUser.Belt
            }).ToList(),
            Masters = workout.WorkoutMasters.Select(wm => new MasterSimplifiedDto
            {
                Id = wm.Master.Id,
                Name = wm.Master.ApplicationUser.Name,
                Belt = wm.Master.ApplicationUser.Belt
            }).ToList(),
            Exercises = workout.WorkoutExercises.Select(we => we.Exercise.Id).ToList()
        };

    // Student -> StudentProgressDto
    public static StudentProgressDto ToProgressDto(this Student student, Belt nextBelt,
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
            AttendedWorkouts = attended.Select(w => w.ToWorkoutDto()).ToList()
        };
    
    public static ExtendedStudentDto ToExtendedStudentDto(this Student student) =>
        new ExtendedStudentDto
        {
            Id = student.Id,
            Name = student.ApplicationUser.Name,
            Address = student.ApplicationUser.Address,
            Gender = student.ApplicationUser.EGender,
            Birthday = student.ApplicationUser.Birthday,
            Belt = student.ApplicationUser.Belt,
            Height = student.ApplicationUser.Height,
            Weight = student.ApplicationUser.Weight,
            EnrollDate = student.EnrollDate
        };
    
    public static MasterExtendedDto ToExtendedMasterDto(this Master master) =>
        new MasterExtendedDto
        {
            Id = master.Id,
            Name = master.ApplicationUser.Name,
            Address = master.ApplicationUser.Address,
            Gender = master.ApplicationUser.EGender,
            Birthday = master.ApplicationUser.Birthday,
            Belt = master.ApplicationUser.Belt,
            Height = master.ApplicationUser.Height,
            Weight = master.ApplicationUser.Weight,
            HasTrainerCertificate = master.HasTrainerCertificate,
        };
}