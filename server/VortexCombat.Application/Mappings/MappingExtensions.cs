using VortexCombat.Application.DTOs.Authentication;
using VortexCombat.Application.DTOs.Master;
using VortexCombat.Application.DTOs.Student;
using VortexCombat.Application.DTOs.Workout;
using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.Mappings;

public static class MappingExtensions
{
    //// RegisterDto -> ApplicationUser
    //public static ApplicationUser ToApplicationUserDto(this RegisterDto dto) =>
    //    new()
    //    {
    //        UserName = dto.Email,
    //        Email = dto.Email,
    //        Name = dto.Name,
    //        Address = dto.Address,
    //        PhoneNumber = dto.PhoneNumber,
    //        Nif = dto.Nif,
    //        EGender = dto.EGender,
    //        Birthday = dto.Birthday,
    //        Belt = dto.Belt,
    //        Height = dto.Height,
    //        Weight = dto.Weight
    //    };

    public static User ToDomainUser(this RegisterDto dto)
        => new User
        {
            Id = new UserId(Guid.NewGuid()),
            Name = dto.Name,
            Address = dto.Address,   
            Nif = dto.Nif ?? string.Empty,
            EGender = dto.EGender,
            Birthday = dto.Birthday,
            Belt = dto.Belt,
            Height = dto.Height,
            Weight = dto.Weight
        };


    //// RegisterDto -> Student
    //public static Student ToStudentDto(this RegisterDto dto, string applicationUserId) =>
    //    new()
    //    {
    //        ApplicationUserId = applicationUserId,
    //        EnrollDate = dto.EnrollDate ?? DateTime.UtcNow
    //        // Removed HasTrainerCertificate because it doesn't exist in Student entity
    //    };

    // RegisterDto -> Student
    public static Student ToStudent(this RegisterDto dto, User User) =>
        new()
        {
            UserId = User.Id,
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
                Name = ws.Student.User.Name,
                Belt = ws.Student.User.Belt
            }).ToList(),
            Masters = workout.WorkoutMasters.Select(wm => new MasterSimplifiedDto
            {
                Id = wm.Master.Id,
                Name = wm.Master.User.Name,
                Belt = wm.Master.User.Belt
            }).ToList(),
            Exercises = workout.WorkoutExercises.Select(we => we.Exercise.Id).ToList()
        };

    // Student -> StudentProgressDto
    public static StudentProgressDto ToProgressDto(this Student student, Belt nextBelt,
        List<Exercise> completed, List<Exercise> remaining, List<Workout> attended) =>
        new()
        {
            Id = student.Id,
            UserId = student.UserId,
            Name = student.User.Name,
            EGender = student.User.EGender,
            Birthday = student.User.Birthday,
            EnrollDate = student.EnrollDate,
            Height = student.User.Height,
            Weight = student.User.Weight,
            CurrentBelt = student.User.Belt,
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
            Name = student.User.Name,
            Address = student.User.Address,
            Gender = student.User.EGender,
            Birthday = student.User.Birthday,
            Belt = student.User.Belt,
            Height = student.User.Height,
            Weight = student.User.Weight,
            EnrollDate = student.EnrollDate
        };
    
    public static MasterExtendedDto ToExtendedMasterDto(this Master master) =>
        new MasterExtendedDto
        {
            Id = master.Id,
            Name = master.User.Name,
            Address = master.User.Address,
            Gender = master.User.EGender,
            Birthday = master.User.Birthday,
            Belt = master.User.Belt,
            Height = master.User.Height,
            Weight = master.User.Weight,
            HasTrainerCertificate = master.HasTrainerCertificate,
        };
}